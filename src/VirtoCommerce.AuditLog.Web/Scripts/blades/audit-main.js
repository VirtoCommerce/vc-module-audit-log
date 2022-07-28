angular.module('AuditLog')
    .controller('AuditLog.mainController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
        $scope.selectedNodeId = null;

        function initializeBlade() {

            var entities = [{ id: '1', name: 'Audit Logs', entityName: 'logs', icon: 'fa-calendar-o' },
                { id: '2', name: 'System events', entityName: 'events', icon: 'fa-gears' }];

            $scope.blade.currentEntities = entities;
            $scope.blade.isLoading = false;
        }

        $scope.blade.openBlade = function (data) {
            $scope.selectedNodeId = data.id;

            var newBlade = {
                id: 'auditLogMainListChildren',
                title: data.name,
                subtitle: 'Audit log service',
                controller: data.controller || 'AuditLog.' + data.entityName + 'Controller',
                template: data.template || 'Modules/$(VirtoCommerce.AuditLog)/Scripts/blades/' + data.entityName + '.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        };

        $scope.blade.headIcon = 'fa fa-flag';

        initializeBlade();
    }]);
