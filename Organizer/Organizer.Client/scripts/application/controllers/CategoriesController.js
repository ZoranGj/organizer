app.controller('CategoriesController', function ($scope, $timeout) {
    $scope.category = {
        name: '',
    }
    $scope.activity = {};
    $scope.showicons = false;
    $scope.showNewCategory = false;

    $scope.initializeCategories = function () {
        var categories = categoriesCtrl.getAll();
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
                    categoriesCtrl.updatePriority(categoryId, priority + 1);
                }
            }
        });
        $scope.showNewCategory = false;
        $scope.category.name = null;
    }

    $scope.addCategory = function () {
        $scope.showNewCategory = true;
        $('#categoryName').focus();

        setTimeout(function () {
            if ($scope.category.name == null) {
                $scope.cancelAddingCategory();
            }
        }, 5000);
    }

    $scope.cancelAddingCategory = function () {
        $scope.showNewCategory = false;
        $scope.category.name = null;
    }

    $scope.saveCategory = function ($event) {
        if ($scope.category.name) {
            var priority = $scope.categories.length + 1;
            categoriesCtrl.add($scope.category.name.toString(), priority);
            $scope.initializeCategories();
        }
    }

    $scope.deleteCategory = function (category, index) {
        var category = $scope.categories.splice(index, 1)[0];
        if (category && category.Id != 0) {
            categoriesCtrl.delete(category.Id);
        }
    }

    $scope.saveActivity = function () {
        categoriesCtrl.saveActivity(this.activity.CategoryId, this.activity.Name, this.activity.Id || 0);
        $scope.initializeCategories();
    }

    $scope.addActivity = function (category) {
        if (category.Activities.filter(function (item) {
                return item.Draft == true;
            }).length > 0) {
            focusActivity(category);
            return;
        }

        $scope.activity.CategoryId = category.Id;
        $scope.activity.Name = 'Activity name..';
        $scope.activity.Draft = true;
        $scope.activity.Id = 0;
        category.Activities.push($scope.activity);
        focusActivity(category);
    }

    function focusActivity(category) {
        $timeout(function () {
            var $elem = $("[data-id=" + category.Id + "] input:last-of-type");
            $elem.focus();
            $elem.select();
        }, 0);
    }

    $scope.deleteActivity = function (category, index) {
        var removed = category.Activities.splice(index, 1)[0];
        if (removed && removed.Id != 0) {
            categoriesCtrl.deleteActivity(removed.Id);
        }
    }

    $scope.onTimeSet = function (newDate, oldDate) {
        $scope.todoItem.Deadline = newDate;
    }

    $scope.todoItemDialog = function (activity) {
        $scope.initTodoItem(activity.Id);
        $("#addTodoItemDialog").modal();
    }

    $scope.initTodoItem = function (activityId, clear) {
        $scope.todoItem = {
            ActivityId: activityId,
            Description: '',
            Duration: 0,
            Resolved: false,
            Deadline: new Date()
        };
    }

    $scope.saveTodoItem = function () {
        todosCtrl.add($scope.todoItem.Description, $scope.todoItem.Deadline, parseInt($scope.todoItem.ActivityId), parseInt($scope.todoItem.Duration), $scope.todoItem.Resolved);
    }

    $scope.deadlineTime = new Date();
    $scope.dateFormat = 'dd.MM.yyyy';
    $scope.deadlinePicker = {
        opened: false
    };
    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };
    $scope.todoItemMessage = "text-success";

    $scope.openDatePicker = function () {
        $scope.deadlinePicker.opened = true;
    };
    $scope.deadlineTimeChanged = function () {
        $scope.todoItem.Deadline.setHours($scope.deadlineTime.getHours(), $scope.deadlineTime.getMinutes(), $scope.deadlineTime.getSeconds());
    }
});