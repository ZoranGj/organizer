﻿app.controller('SettingsController', function ($scope) {
    $scope.init = function () {
        var categories = categoriesCtrl.getAll();
        $scope.categories = JSON.parse(categories);
    }

    $scope.updateCategory = function (category) {
        categoriesCtrl.updateSetting(category.Id, parseInt(category.MinHoursPerWeek), parseInt(category.MaxHoursPerWeek));
    }
});