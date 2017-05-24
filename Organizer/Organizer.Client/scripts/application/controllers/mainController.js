app.controller('MainController', function ($scope) {
    $scope.todoItemNotifications = [];

    $scope.initialize = function () {
        var data = JSON.parse(reportsCtrl.loadTodoItemNotifications());
        if (data && data.length) {
            $scope.todoItemNotifications = data;
            $scope.expiredNotifications = data.filter(function (item) {
                return item.Expired == true;
            }).length;
        }
    }

    $scope.initialize();
});