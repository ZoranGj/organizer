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

        if ($scope.goals.length) {
            $scope.filter.goal = $scope.goals[0].Id;
            $scope.initializeTodoItemsByGoal();
        }
    }

    $scope.goalChanged = function () {
        $scope.initializeTodoItemsByGoal();
    }

    $scope.initializeTodoItemsByGoal = function (goalId) {
        var goalId = $scope.filter.goal;

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
        $scope.$parent.initialize();
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
        todosCtrl.update(item.Id, item.Notes, item.Tags.join(','));
    }

    $scope.updateTodoDescription = function (id, value) {
        if (!id || !value) return;
        todosCtrl.updateDescription(id, value);
    }

    $scope.updateTodoDuration = function (id, value) {
        if (!id || !value || isNaN(value)) return;
        todosCtrl.updateDuration(id, parseInt(value));
    }
});