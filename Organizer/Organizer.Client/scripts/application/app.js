﻿var app = angular.module('ngApp', ['ui.bootstrap.datetimepicker', 'ui.calendar', 'ui-notification']);

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

app.directive('autoFocus', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            //var model = attrs.changeModel;
            //scope.$watch(model, function () {
            //    alert();
            //    return scope.$eval(attrs.autoFocus);
            //}, function (newValue) {
            //    if (newValue === true) {
            //        element[0].focus();
            //        element[0].select();
            //        //scope.showicons = true;
            //    }
            //});
        }
    };
});

//app.config(function(NotificationProvider) {
//    NotificationProvider.setOptions({
//        delay: 10000,
//        startTop: 20,
//        startRight: 10,
//        verticalSpacing: 20,
//        horizontalSpacing: 20,
//        positionX: 'left',
//        positionY: 'bottom'
//    });
//});