angular.module('AuditLog')
    .controller('AuditLog.helloWorldController', ['$scope', 'AuditLog.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'AuditLog';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'AuditLog.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
