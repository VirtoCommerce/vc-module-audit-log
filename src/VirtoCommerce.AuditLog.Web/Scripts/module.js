// Call this to register your module to main application
var moduleName = 'AuditLog';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.AuditLogState', {
                    url: '/AuditLog',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'AuditLog.helloWorldController',
                                template: 'Modules/$(VirtoCommerce.AuditLog)/Scripts/blades/hello-world.html',
                                isClosingDisabled: true,
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', '$state',
        function (mainMenuService, $state) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/AuditLog',
                icon: 'fa fa-cube',
                title: 'AuditLog',
                priority: 100,
                action: function () { $state.go('workspace.AuditLogState'); },
                permission: 'AuditLog:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
