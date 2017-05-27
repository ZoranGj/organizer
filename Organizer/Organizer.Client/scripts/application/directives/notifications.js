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
            templateUrl: 'templates/todoItemNotifications.html',
            replace: true
        };
    }
]);