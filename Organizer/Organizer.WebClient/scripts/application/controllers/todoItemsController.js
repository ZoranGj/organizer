app.controller('TodoItemsController', function ($scope, goals, tasks) {
    $scope.todoItem = {
        ActivityId: 0,
        Deadline: new Date(),
        Tags: []
    }

    $scope.filter = {
        goal: 0,
        status: 0
    };
  
    $scope.initializeTodoItems = function () {
        tasks.getTagNames().then(function (response) {
            $scope.allTag = response.data;
        }, function (error) {
        });

        goals.getGoals().then(function (response) {
            $scope.goals = response.data;

            if ($scope.goals.length) {
                $scope.filter.goal = $scope.goals[0].Id;
                $scope.initializeTodoItemsByGoal();
            }
        }, function (error) {
        });
    }

    $scope.goalChanged = function () {
        $scope.initializeTodoItemsByGoal();
    }

    $scope.initializeTodoItemsByGoal = function (goalId) {
        var goalId = $scope.filter.goal || 0;

        tasks.getAll(goalId).then(function (response) {
            $scope.todoItems = response.data;
        }, function (error) {
        });

        goals.getActivityItems(goalId).then(function (response) {
            $scope.activityItems = response.data;
        }, function (error) {
        });
    }

    $scope.deleteTodoItem = function (todoItemId) {
        tasks.delete(todoItemId).then(function (response) {
            $scope.initializeTodoItems();
        }, function (error) {
        });
    }

    $scope.resolveItem = function (id) {
        tasks.resolve(id, this.item.Resolved).then(function (response) {
            $scope.$parent.initialize();
        }, function (error) {
        });
    }

    $scope.todoItemClass = function (item) {
        return item.Resolved ? 'resolved' : '';
    }

    $scope.todoItemDialog = function () {
        $scope.initTodoItem($scope.todoItem.ActivityId);
        $("#addTodoItemDialog").modal();
    }

    $scope.initTodoItem = function (activityId, clear) {
        var todoItem = newTodoItem(activityId);
        $scope.todoItem = todoItem;
    }

    function newTodoItem(activityId) {
        return {
            ActivityId: activityId,
            Description: '',
            Duration: 0,
            Resolved: false,
            Deadline: new Date()
        };
    }

    //Multiple TodoItems dialog:
    $scope.deadlineTime = new Date();
    $scope.dateFormat = 'dd.MM.yyyy';
    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.openDatePicker = function (item) {
        item.PickerOpened = true;
    };

    $scope.multipleTodoItemsDialog = function () {
        initMultipleTodoItems();
        $("#multipleTodoItemsDialog").modal();
    }

    $scope.saveMultipleTodoItems = function () {
        //todo fix
        angular.forEach($scope.multipleTodoItems, function (item) {
            if (item.ActivityId && item.Duration && item.Deadline) {
                tasks.add(item.Description, item.Deadline, parseInt(item.ActivityId), parseInt(item.Duration), item.Resolved).then(function (response) {
                }, function (error) {
                });
            }
        });
        $scope.refreshTodoItems();
    }

    $scope.refreshTodoItems = function () {
        $scope.initializeTodoItemsByGoal();
        $scope.$parent.initialize();
    }

    function initMultipleTodoItems () {
        var multipleTodoItems = [];
        for (var i = 0; i < 15; i++) {
            multipleTodoItems.push(newTodoItem(0));
        }
        $scope.multipleTodoItems = multipleTodoItems;
    }

    $scope.updateTodoItem = function (item) {
        tasks.update(item.Id, item.Notes, item.Tags.join(',')).then(function (response) {
        }, function (error) {
        });
    }

    $scope.updateTodoDescription = function (id, value) {
        if (!id || !value) return;
        tasks.updateDescription(id, value).then(function (response) {
        }, function (error) {
        });
    }

    $scope.updateTodoDuration = function (id, value) {
        if (!id || !value || isNaN(value)) return;
        tasks.updateDuration(id, parseInt(value)).then(function (response) {
        }, function (error) {
        });
    }
});