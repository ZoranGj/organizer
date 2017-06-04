var app = angular.module('ngApp', ['ngRoute', 'ui.bootstrap', 'ui.calendar', 'ui-notification', 'color.picker']);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/', {
            templateUrl: '/scripts/application/views/goals.html',
            controller: 'GoalsController',
            activeView: 'goals'
        }).
        when('/todos', {
            templateUrl: '/scripts/application/views/todos.html',
            controller: 'TodoItemsController',
            activeView: 'todos'
        }).
        when('/calendar', {
            templateUrl: '/scripts/application/views/calendar.html',
            controller: 'CalendarController',
            activeView: 'calendar'
        }).
        when('/reports', {
            templateUrl: '/scripts/application/views/reports.html',
            controller: 'ReportsController',
            activeView: 'reports'
        }).
        when('/reports_tags', {
            templateUrl: '/scripts/application/views/reports_tags.html',
            controller: 'ReportsController',
            activeView: 'reports'
        }).
        otherwise({
            redirectTo: '/'
        });
  }]);

app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix('');
}]);