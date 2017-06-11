app.service('authenticationService', ['$http', function ($http) {
    var service = {};

    service.login = function (userLogin) {
        return $http({
            method: 'POST',
            url: '/Account/Login',
            contentType: 'application/x-www-form-urlencoded; charset-UTF-8',
            data: userLogin
        });
    }

    service.logout = function () {
        return $http({
            method: 'POST',
            url: '/Account/Logout',
        });
    }

    service.register = function (user) {
        return $http({
            method: 'POST',
            url: '/Account/Register',
            contentType: 'application/x-www-form-urlencoded; charset-UTF-8',
            data: user
        });
    }

    return service;
}]);