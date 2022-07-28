using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.AuditLog.Core;
using VirtoCommerce.AuditLog.Core.Handlers;
using VirtoCommerce.AuditLog.Core.Services;
using VirtoCommerce.AuditLog.Data.Handlers;
using VirtoCommerce.AuditLog.Data.Repositories;
using VirtoCommerce.AuditLog.Data.Services;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.AuditLog.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // Initialize database
            var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ??
                                   Configuration.GetConnectionString("VirtoCommerce");

            serviceCollection.AddDbContext<AuditLogDbContext>(options => options.UseSqlServer(connectionString));

            // Register services
            serviceCollection.AddTransient<IAuditLogRepository, AuditLogRepository>();
            serviceCollection.AddTransient<Func<IAuditLogRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IAuditLogRepository>());
            serviceCollection.AddSingleton<IGenericEventHandlerManager, GenericEventHandlerManager>();
            serviceCollection.AddTransient<IChangeLogService, AuditLogService>();
            serviceCollection.AddTransient<IChangeLogSearchService, AuditLogService>();
            serviceCollection.AddTransient<IAuditLogService, AuditLogService>();
            serviceCollection.AddTransient<IDomainEventHandler, DomainEventHandler>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            var serviceProvider = appBuilder.ApplicationServices;

            // Register settings
            var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // Register permissions
            var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
            permissionsRegistrar.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions
                .Select(x => new Permission { ModuleId = ModuleInfo.Id, GroupName = "AuditLog", Name = x })
                .ToArray());

            // Register generic events handler
            var genericEventHandlerManager = appBuilder.ApplicationServices.GetService<IGenericEventHandlerManager>();
            genericEventHandlerManager.RegisterGenericHandler();

            // Apply migrations
            using var serviceScope = serviceProvider.CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetRequiredService<AuditLogDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }

        public void Uninstall()
        {
        }
    }
}
