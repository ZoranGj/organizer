var app = angular.module('ngApp', ['ui.bootstrap.datetimepicker']);

appController.showDevTools();

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
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
        when('/reports', {
            templateUrl: 'templates/reports.html',
            controller: 'ReportsController'
        }).
        otherwise({
            redirectTo: '/categories'
        });
  }]);