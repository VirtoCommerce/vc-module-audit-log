angular.module('AuditLog')
    .factory('AuditLog.webApi', ['$resource', function ($resource) {
        return $resource('api/audit-log');
    }]);
