app.controller('MainController', function ($scope, $route, reports) {
    $scope.todoItemNotifications = [];

    $scope.initialize = function () {
        reports.loadTodoItemNotifications().then(function (response) {
            var data = response.data;
            if (data && data.length) {
                $scope.todoItemNotifications = data;
                $scope.expiredNotifications = data.filter(function (item) {
                    return item.Expired == true;
                });
                $scope.upcomingNotifications = data.filter(function (item) {
                    return item.Expired == false;
                });
            }
        }, function (error) {
        });
    }

    $scope.colorOptions = {
        format: ['hexString'],
    };

    $scope.isActiveView = function (viewName) {
        if (!viewName || !$route.current) return false;
        return $route.current.activeView == viewName;
    }

    $scope.initialize();
});