var app = angular.module('ngApp', ['ui.bootstrap.datetimepicker', 'ui.calendar', 'ui-notification']);

appCtrl.showDevTools();
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
        when('/calendar', {
            templateUrl: 'templates/calendar.html',
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
        when('/reports_tags', {
            templateUrl: 'templates/reports_tags.html',
            controller: 'ReportsController'
        }).
        otherwise({
            redirectTo: '/categories'
        });
  }]);