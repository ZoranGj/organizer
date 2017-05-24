app.directive('notifications', [
    function () {
        return {
            restrict: 'E',
            scope: {
                items: '='
            },
            link: function(scope, element, attributes){

            },
            templateUrl: 'templates/todoItemNotifications.html',
            replace: true
        };
    }
]);