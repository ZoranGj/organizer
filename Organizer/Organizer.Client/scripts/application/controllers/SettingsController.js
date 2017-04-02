app.controller('SettingsController', function ($scope) {
    $scope.init = function () {
        var categories = appController.getCategories();
        $scope.categories = JSON.parse(categories);
    }

    $scope.updateCategory = function (category) {
        appController.updateCategoryData(category.Id, parseInt(category.MinHoursPerWeek), parseInt(category.MaxHoursPerWeek));
    }
});