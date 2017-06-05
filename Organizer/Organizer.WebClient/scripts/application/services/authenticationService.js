app.service('authenticationService', ['$http', function ($http) {
    var service = {};

    service.login = function (userLogin) {
        return $http.post('/Account/Login', 'POST', userLogin);
    }

    service.register = function (user) {
        return $http.post('/Account/Register', 'POST', user);
    }

    return service;
}]);