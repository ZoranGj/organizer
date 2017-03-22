app.controller('TodoItemsController', function ($scope) {
    $scope.todoItem = {
        ActivityId: 0,
        Deadline: new Date()
    }

    $scope.filter = {
        category: 0,
        status: 0
    };

    $scope.initializeTodoItems = function () {
        var categories = appController.getCategories();
        $scope.categories = JSON.parse(categories);

        $scope.initializeTodoItemsByCategory($scope.filter.category);
    }

    $scope.categoryChanged = function () {
        $scope.initializeTodoItemsByCategory($scope.filter.category);
    }

    $scope.statusChanged = function () {

    }

    $scope.initializeTodoItemsByCategory = function (categoryId) {
        var todoItems = appController.getTodoItems(categoryId);
        $scope.todoItems = JSON.parse(todoItems);

        var activityItems = appController.getActivityItems(categoryId);
        $scope.activityItems = JSON.parse(activityItems);
    }

    $scope.addTodoItem = function () {
        if ($scope.todoItem.ActivityId) {
            appController.addTodoItem($scope.todoItem.Description, new Date($scope.todoItem.Deadline), $scope.todoItem.ActivityId, parseInt($scope.todoItem.Duration));
            $scope.initializeTodoItems();
        }
    }

    $scope.deleteTodoItem = function (todoItemId) {
        appController.deleteTodoItem(todoItemId);
        $scope.initializeTodoItems();
    }

    $scope.onTimeSet = function (newDate, oldDate) {
        $scope.todoItem.Deadline = newDate;
    }

    $scope.resolveItem = function (id) {
        appController.resolveTodoItem(id, this.item.Resolved);
    }

    $scope.todoItemClass = function (item) {
        return item.Resolved ? 'resolved' : '';
    }
});