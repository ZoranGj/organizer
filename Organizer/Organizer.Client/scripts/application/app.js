var app = angular.module('ngApp', []);
appController.showDevTools();

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: 'templates/index.html',
            controller: 'HomeController'
        }).
        when('/categories', {
            templateUrl: 'templates/categories.html',
            controller: 'CategoriesController'
        }).
        when('/todos', {
            templateUrl: 'templates/todos.html',
            controller: 'TodoItemsController'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);

app.controller('HomeController', function ($scope) {
    $scope.message = 'message from home controller';
});

app.controller('CategoriesController', function ($scope) {
    $scope.category = {
        name: '',
    }

    $scope.initializeCategories = function () {
        var categories = appController.getCategories();
        $scope.categories = JSON.parse(categories);

        $("#categories").sortable({
            tolerance: 'pointer',
            revert: 'invalid',
            placeholder: 'col-md-3 card placeholder',
            forceHelperSize: true,
            stop: function(evt, ui) {
                var priority = $('#categories > div').index(ui.item);
                var categoryId = $(ui.item).data('id');
                if (priority > -1) {
                    appController.updateCategoryPriority(categoryId, priority + 1);
                }
            }
        });
        $('#newCategory').hide();
        $scope.category.name = null;

        $scope.activity = {
            categoryId: null,
            categoryName: null,
            name: null,
            description: null,
        }
    }

    $scope.addCategory = function () {
        $('#newCategory').show();
        $('#categoryName').focus();

        setTimeout(function () {
            if ($scope.category.name == null) {
                $scope.cancelAddingCategory();
            }
        }, 5000);
    }

    $scope.cancelAddingCategory = function() {
        $('#newCategory').hide();
        $('#categoryName').val('');
    }

    $scope.saveCategory = function ($event) {
        if ($scope.category.name) {
            var priority = $scope.categories.length + 1;
            appController.addCategory($scope.category.name.toString(), priority);
            $scope.initializeCategories();
        }
    }

    $scope.deleteCategory = function (id) {
        appController.deleteCategory(id);
        $scope.initializeCategories();
    }

    $scope.showicons = false;

    $scope.saveActivity = function() {
        appController.saveActivity($scope.activity.categoryId, $scope.activity.name, $scope.activity.description);
        $scope.initializeCategories();
    }

    $scope.addActivity = function (categoryId, categoryName) {
        $scope.activity.categoryId = categoryId;
        $scope.activity.categoryName = categoryName;
        $('#activityModal').modal();
    }

    $scope.deleteActivity = function(id) {
        appController.deleteActivity(id);
        $scope.initializeCategories();
    }
});

app.controller('TodoItemsController', function($scope) {
    $scope.todoItem = {
        ActivityId: 0
    }

    $scope.initializeTodoItems = function () {
        var todoItems = appController.getTodoItems();
        $scope.todoItems = JSON.parse(todoItems);

        var activityItems = appController.getActivityItems();
        $scope.activityItems = JSON.parse(activityItems);
    }

    $scope.addTodoItem = function () {
        if ($scope.todoItem.ActivityId) {
            appController.addTodoItem($scope.todoItem.ActivityId);
            $scope.initializeTodoItems();
        }
    }

    $scope.deleteTodoItem = function (todoItemId) {
        appController.deleteTodoItem(todoItemId);
        $scope.initializeTodoItems();
    }
});