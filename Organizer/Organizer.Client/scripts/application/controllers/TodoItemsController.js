app.controller('TodoItemsController', function ($scope) {
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
        $scope.allTags = JSON.parse(todosCtrl.getTagNames());

        var goals = goalsCtrl.getAll();
        $scope.goals = JSON.parse(goals);

        $scope.initializeTodoItemsByGoal($scope.filter.goal);
    }

    $scope.goalChanged = function () {
        $scope.initializeTodoItemsByGoal($scope.filter.goal);
    }

    $scope.initializeTodoItemsByGoal = function (goalId) {
        var todoItems = todosCtrl.getAll(goalId);
        $scope.todoItems = JSON.parse(todoItems);

        var activityItems = goalsCtrl.getActivityItems(goalId);
        $scope.activityItems = JSON.parse(activityItems);
    }

    $scope.deleteTodoItem = function (todoItemId) {
        todosCtrl.delete(todoItemId);
        $scope.initializeTodoItems();
    }

    $scope.resolveItem = function (id) {
        todosCtrl.resolve(id, this.item.Resolved);
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
        angular.forEach($scope.multipleTodoItems, function (item) {
            if (item.ActivityId && item.Duration && item.Deadline) {
                todosCtrl.add(item.Description, item.Deadline, parseInt(item.ActivityId), parseInt(item.Duration), item.Resolved);
            }
        });
    }

    function initMultipleTodoItems () {
        var multipleTodoItems = [];
        for (var i = 0; i < 15; i++) {
            multipleTodoItems.push(newTodoItem(0));
        }
        $scope.multipleTodoItems = multipleTodoItems;
    }
});