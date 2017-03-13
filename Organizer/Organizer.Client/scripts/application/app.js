var app = angular.module('ngApp', ['ui.bootstrap.datetimepicker']);
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
        when('/settings', {
            templateUrl: 'templates/settings.html',
            controller: 'SettingsController'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);

app.controller('SettingsController', function ($scope) {
});

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

    $scope.saveActivity = function () {
        appController.saveActivity(this.activity.CategoryId, this.activity.Name, this.activity.Id  || 0);
    }

    $scope.addActivity = function (category) {
        category.Activities.push({
            CategoryId: category.Id,
            Name: 'Activity name..',
            Id: 0,
        });
        $(".activity[data-id=0]").focus();
    }

    $scope.deleteActivity = function(id) {
        appController.deleteActivity(id);
        $scope.initializeCategories();
    }
});

app.controller('TodoItemsController', function($scope) {
    $scope.todoItem = {
        ActivityId: 0,
        Deadline: new Date()
    }

    $scope.initializeTodoItems = function () {
        var categories = appController.getCategories();
        $scope.categories = JSON.parse(categories);

        $scope.initializeTodoItemsByCategory($scope.categories[0].categoryId);
    }

    $scope.categoryChanged = function () {
        $scope.initializeTodoItemsByCategory($scope.selectedCategoryId);
    }

    $scope.initializeTodoItemsByCategory = function(categoryId){
        var todoItems = appController.getTodoItems(categoryId);
        $scope.todoItems = JSON.parse(todoItems);

        var activityItems = appController.getActivityItems(categoryId);
        $scope.activityItems = JSON.parse(activityItems);
    }

    $scope.addTodoItem = function () {
        if ($scope.todoItem.ActivityId) {
            appController.addTodoItem($scope.todoItem.Description, new Date($scope.todoItem.Deadline), $scope.todoItem.ActivityId);
            $scope.initializeTodoItems();
        }
    }

    $scope.deleteTodoItem = function (todoItemId) {
        appController.deleteTodoItem(todoItemId);
        $scope.initializeTodoItems();
    }

    $scope.onTimeSet = function (newDate, oldDate) {
        alert(newDate);
        $scope.todoItem.Deadline = newDate;
    }
});