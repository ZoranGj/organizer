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
            stop: function (evt, ui) {
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

    $scope.cancelAddingCategory = function () {
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
        appController.saveActivity(this.activity.CategoryId, this.activity.Name, this.activity.Id || 0);
    }

    $scope.addActivity = function (category) {
        category.Activities.push({
            CategoryId: category.Id,
            Name: 'Activity name..',
            Id: 0,
        });
        console.log($("[data-id=" + category.Id + "]"));
        console.log($("[data-id=" + category.Id + "] .activity:last-of-type"));
        $("[data-id=" + category.Id + "] .activity:last-of-type").next().focus();
    }

    $scope.deleteActivity = function (id) {
        appController.deleteActivity(id);
        $scope.initializeCategories();
    }
});