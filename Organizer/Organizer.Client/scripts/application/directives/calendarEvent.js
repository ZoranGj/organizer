app.directive('calendarEvent', [function () { //'notifications',
    return {
        restrict: 'A',
        scope: {
            eventData : '&data',
            eventId : '=id',
        },
        link: function (scope, element) {
            element.on('click', function () {
                var getDataFun = scope.eventData();
                var data = getDataFun(scope.eventId);
                console.log(data);
            })
        }
    }
}]);