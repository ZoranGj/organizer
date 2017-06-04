app.directive('todoItemDialog', [function () {
    return {
        restrict: 'E',
        scope: {
            todoItem: '=item',
            activities: '=activities',
            refreshAction: '&refreshAction'
        },
        controller: function ($scope) {
            $scope.deadlineTime = new Date();
            $scope.dateFormat = 'dd.MM.yyyy';
            $scope.deadlinePicker = {
                opened: false
            };
            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1
            };
            $scope.todoItemMessage = "text-success";

            $scope.openDatePicker = function () {
                $scope.deadlinePicker.opened = true;
            };
            $scope.deadlineTimeChanged = function () {
                $scope.todoItem.Deadline.setHours($scope.deadlineTime.getHours(), $scope.deadlineTime.getMinutes(), $scope.deadlineTime.getSeconds());
            }

            $scope.saveTodoItem = function () {
                todosCtrl.add($scope.todoItem.Description, $scope.todoItem.Deadline, parseInt($scope.todoItem.ActivityId), parseInt($scope.todoItem.Duration), $scope.todoItem.Resolved);
                $scope.refreshAction();
            }
        },
        templateUrl: 'templates/todoItem.html',
        replace: true,
    }
}]);