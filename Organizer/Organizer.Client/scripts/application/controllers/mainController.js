app.controller('MainController', function ($scope, $route) {
    $scope.todoItemNotifications = [];

    $scope.initialize = function () {
        var data = JSON.parse(reportsCtrl.loadTodoItemNotifications());
        if (data && data.length) {
            $scope.todoItemNotifications = data;
            $scope.expiredNotifications = data.filter(function (item) {
                return item.Expired == true;
            });
            $scope.upcomingNotifications = data.filter(function (item) {
                return item.Expired == false;
            });
        }
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