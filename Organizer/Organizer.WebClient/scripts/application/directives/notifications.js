app.directive('notifications', [
    function () {
        return {
            restrict: 'E',
            scope: {
                expired: '=',
                upcoming: '='
            },
            link: function(scope, element, attributes){

            },
            templateUrl: '/scripts/application/views/templates/todoItemNotifications.html',
            replace: true
        };
    }
]);