﻿var app = angular.module('ngApp', ['ngRoute', 'ui.bootstrap', 'ui.calendar', 'ui-notification']);

appCtrl.showDevTools();
app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/goals', {
            templateUrl: 'goals.html',
            controller: 'GoalsController'
        }).
        when('/todos', {
            templateUrl: 'todos.html',
            controller: 'TodoItemsController'
        }).
        when('/calendar', {
            templateUrl: 'calendar.html',
            controller: 'CalendarController'
        }).
        when('/reports', {
            templateUrl: 'reports.html',
            controller: 'ReportsController'
        }).
        when('/reports_tags', {
            templateUrl: 'reports_tags.html',
            controller: 'ReportsController'
        }).
        otherwise({
            redirectTo: '/goals'
        });
  }]);

app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix('');
}]);