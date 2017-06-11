app.controller('LoginController', function ($scope, $window, $location, authenticationService) {
    initialize();

    function initialize() {
        initializeRegisterModel();
        initializeLoginModel();
    }

    function initializeRegisterModel() {
        $scope.userRegistration = {
            username: '',
            email: '',
            password: '',
        };
    }

    function initializeLoginModel() {
        $scope.userLogin = {
            email: '',
            password: ''
        }
    }

    $scope.register = function () {
        var registerPromise = authenticationService.register($scope.userRegistration);

        registerPromise.then(function (response) {
            initializeRegisterModel();
            sessionStorage.setItem('accessToken', response.data.accessToken);
            $scope.$parent.isAuthenticated = true;
            $location.path('/');
        }, function (error) {
            alert('error on register');
        });
    }

    $scope.login = function () {
        var loginPromise = authenticationService.login($scope.userLogin);

        loginPromise.then(function (response) {
            initializeLoginModel();
            sessionStorage.setItem('userName', response.data.userName);
            sessionStorage.setItem('accessToken', response.data.accessToken);
            sessionStorage.setItem('refreshToken', response.data.refreshToken);
            $scope.$parent.isAuthenticated = true;
            $location.path('/');
        }, function (error) {
            alert('error on login');
        });
    }
});