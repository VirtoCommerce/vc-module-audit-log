angular.module('AuditLog')
    .controller('AuditLog.auditLogRecordFieldsWidgetController', ['$scope', '$translate', 'platformWebApp.bladeNavigationService',
        function ($scope, $translate, bladeNavigationService) {
            var blade = $scope.widget.blade;

            $scope.$watch('widget.blade.currentEntity', function (auditLogRecord) {
                $scope.auditLogRecord = auditLogRecord;
            });

            $scope.openItemsBlade = function () {

                var newBlade = {
                    id: 'auditLogRecordFields',
                    currentEntity: $scope.auditLogRecord,
                    controller: 'AuditLog.auditLogRecordFieldsController',
                    template: 'Modules/$(VirtoCommerce.AuditLog)/Scripts/blades/auditLogRecord-fields.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

        }]);
