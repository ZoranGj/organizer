var app = angular.module('ngApp', ['ngRoute', 'ui.bootstrap', 'ui.calendar', 'ui-notification', 'color.picker']);

appCtrl.showDevTools();
app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/goals', {
            templateUrl: 'goals.html',
            controller: 'GoalsController',
            activeView: 'goals'
        }).
        when('/todos', {
            templateUrl: 'todos.html',
            controller: 'TodoItemsController',
            activeView: 'todos'
        }).
        when('/calendar', {
            templateUrl: 'calendar.html',
            controller: 'CalendarController',
            activeView: 'calendar'
        }).
        when('/reports', {
            templateUrl: 'reports.html',
            controller: 'ReportsController',
            activeView: 'reports'
        }).
        when('/reports_tags', {
            templateUrl: 'reports_tags.html',
            controller: 'ReportsController',
            activeView: 'reports'
        }).
        otherwise({
            redirectTo: '/goals'
        });
  }]);

app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix('');
}]);