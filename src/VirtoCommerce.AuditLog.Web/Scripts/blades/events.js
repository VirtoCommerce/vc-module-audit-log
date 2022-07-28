angular.module('AuditLog')
    .controller('AuditLog.eventsController', ['$scope', 'platformWebApp.bladeNavigationService', 'AuditLog.webApi', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, api, dialogService) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:security:update';

        blade.refresh = function () {
            api.events(function (events) {
                initializeBlade(events);
            });
        }

        function initializeBlade(events) {
            blade.activeEntities = _.filter(events, function (o) { return o.active; });
            blade.unActiveEntities = _.filter(events, function (o) { return !o.active; });
            blade.isLoading = false;
        }

        $scope.removeEvents = function () {
            blade.isLoading = true;

            var events = _.filter(blade.activeEntities, function (o) { return !o.$selected; });
            api.updateEvents(events, function (result) {
                initializeBlade(result);
            });
        };

        $scope.addEvents = function () {
            blade.isLoading = true;

            var events = blade.activeEntities.concat(_.filter(blade.unActiveEntities, function (o) { return o.$selected; }));
            api.updateEvents(events, function (result) {
                initializeBlade(result);
            })
        };

        function isUnsubscribeItemsChecked() {
            return blade.activeEntities && _.any(blade.activeEntities, function (x) { return x.$selected; });
        }

        function isSubscribeItemsChecked() {
            return blade.unActiveEntities && _.any(blade.unActiveEntities, function (x) { return x.$selected; });
        }

        blade.headIcon = 'fas fa-gears';

        function initializeToolbar() {
            blade.toolbarCommands = [
                {
                    name: "AuditLog.blades.events.buttons.unsubscribe",
                    icon: 'fas fa-save',
                    executeMethod: $scope.removeEvents,
                    canExecuteMethod: isUnsubscribeItemsChecked,
                    permission: blade.updatePermission
                },
                {
                    name: "AuditLog.blades.events.buttons.subscribe",
                    icon: 'fas fa-save',
                    executeMethod: $scope.addEvents,
                    canExecuteMethod: isSubscribeItemsChecked,
                    permission: blade.updatePermission
                }
            ];
        }

        // actions on load
        initializeToolbar();
        blade.refresh();
    }]);
