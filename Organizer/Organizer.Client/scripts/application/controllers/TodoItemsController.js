app.controller('TodoItemsController', function ($scope) {
    $scope.todoItem = {
        ActivityId: 0,
        Deadline: new Date(),
        Tags: []
    }

    $scope.filter = {
        category: 0,
        status: 0
    };

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

    $scope.initializeTodoItems = function () {
        $scope.allTags = JSON.parse(todosCtrl.getTagNames());

        var categories = categoriesCtrl.getAll();
        $scope.categories = JSON.parse(categories);

        $scope.initializeTodoItemsByCategory($scope.filter.category);
    }

    $scope.categoryChanged = function () {
        $scope.initializeTodoItemsByCategory($scope.filter.category);
    }

    $scope.statusChanged = function () {

    }

    $scope.initializeTodoItemsByCategory = function (categoryId) {
        var todoItems = todosCtrl.getAll(categoryId);
        $scope.todoItems = JSON.parse(todoItems);

        var activityItems = categoriesCtrl.getActivityItems(categoryId);
        $scope.activityItems = JSON.parse(activityItems);
    }

    $scope.addTodoItem = function () {
        if ($scope.todoItem.ActivityId) {
            todosCtrl.add($scope.todoItem.Description, new Date($scope.todoItem.Deadline), $scope.todoItem.ActivityId, parseInt($scope.todoItem.Duration));
            $scope.initializeTodoItems();
        }
    }

    $scope.deleteTodoItem = function (todoItemId) {
        todosCtrl.delete(todoItemId);
        $scope.initializeTodoItems();
    }

    $scope.onTimeSet = function (newDate, oldDate) {
        $scope.todoItem.Deadline = newDate;
    }

    $scope.resolveItem = function (id) {
        todosCtrl.resolve(id, this.item.Resolved);
    }

    $scope.todoItemClass = function (item) {
        return item.Resolved ? 'resolved' : '';
    }

    $scope.updateTodoItem = function (item) {
        todosCtrl.update(item.Id, item.Notes, item.Tags.join(','));
    }
});