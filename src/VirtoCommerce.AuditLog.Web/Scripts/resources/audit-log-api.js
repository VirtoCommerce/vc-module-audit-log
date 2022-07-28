angular.module('AuditLog')
    .factory('AuditLog.webApi', ['$resource', function ($resource) {
        return $resource('api/audit-log/:id', { id: '@Id' }, {
            searchLogs: { url: 'api/audit-log', method: 'POST' },
            events: { url: 'api/audit-log/events', method: 'GET', isArray: true },
            updateEvents: { url: 'api/audit-log/events', method: 'POST', isArray: true },
        });
    }]);
