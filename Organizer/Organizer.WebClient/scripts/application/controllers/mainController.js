app.controller('MainController', function ($scope, $route, $location, $window, reports, authenticationService) {
    $scope.todoItemNotifications = [];
    $scope.isAuthenticated = false;

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

        var token = $window.sessionStorage.getItem('accessToken');
        if (token) {
            $scope.isAuthenticated = true;
        }
    }

    $scope.logout = function () {
        var logoutPromise = authenticationService.logout();

        logoutPromise.then(function (response) {
            $window.sessionStorage.removeItem('accessToken')
            $scope.isAuthenticated = false;
            $location.path('/');
        }, function (error) {
            alert('error on logout');
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