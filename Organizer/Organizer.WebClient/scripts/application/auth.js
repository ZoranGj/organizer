app.factory('AuthInterceptor', function ($window, $location, $q) {
    return {
        request: function (config) {
            config.headers = config.headers || {};
            var accessToken = $window.sessionStorage.getItem('accessToken');
            if (accessToken) {
                config.headers.Authorization = 'Bearer ' + accessToken;
            }

            return config || $q.when(config);
        },
        response: function (response) {
            if (response.status == "401") {
                $location.path('/login');
            }
            return response || $q.when(response);
        },
        responseError: function (rejection) {
            if (rejection.status == "401") {
                $location.path('/login');
            }
            return $q.reject(rejection);
        }
    }
});