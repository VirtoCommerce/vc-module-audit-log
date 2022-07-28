angular.module('AuditLog')
    .controller('AuditLog.auditLogRecordDetailsController', ['$scope', 'platformWebApp.bladeNavigationService', 'AuditLog.webApi', function ($scope, bladeNavigationService, api) {
        var blade = $scope.blade;

        blade.refresh = function () {
            api.get({ id: blade.auditLogRecordId }, function (auditLogRecord) {
                blade.currentEntity = auditLogRecord;
                blade.isLoading = false;
            });
        }

        blade.refresh();
    }]);
