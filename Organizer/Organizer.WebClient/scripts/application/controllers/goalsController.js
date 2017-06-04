app.controller('GoalsController', function ($scope, $timeout, COLORS, $http, goals) {
    $scope.goal = {
        name: '',
        color: COLORS.green,
    };
    $scope.activity = {};
    $scope.activities = [];
    $scope.showicons = false;
    $scope.showNewGoal = false;
    $scope.editableColor = COLORS.white;

    getGoals();

    function getGoals() {
        goals.getGoals()
            .then(function (response) {
                $scope.goals = response.data;
                $("#goals").sortable({
                    handle: '.fa-arrows',
                    tolerance: 'pointer',
                    revert: 'invalid',
                    placeholder: 'col-md-4 card placeholder',
                    forceHelperSize: true,
                    stop: function (evt, ui) {
                        var priority = $('#goals > div').index(ui.item);
                        var goalId = $(ui.item).data('id');
                        if (priority > -1) {
                            goals.updatePriority(goalId, priority + 1);
                        }
                    }
                });
            }, function (error) {
                alert(error.message);
            });

    }

    $scope.initializeGoals = function () {
        getGoals();

        $scope.showNewGoal = false;
        $scope.goal.name = null;
    }

    $scope.addGoal = function () {
        $scope.showNewGoal = true;
        $('#goalName').focus();

        setTimeout(function () {
            if ($scope.goal.name == null) {
                $scope.cancelAddingGoal();
            }
        }, 5000);
    }

    $scope.cancelAddingGoal = function () {
        $scope.showNewGoal = false;
        $scope.goal.name = null;
    }

    $scope.saveGoal = function ($event) {
        if ($scope.goal.name) {
            var priority = $scope.goals ? $scope.goals.length + 1 : 0;
            goals.add($scope.goal.name.toString(), priority).then(function (response) {
                $scope.initializeGoals();
            }, function (error) {

            });
        }
    }

    $scope.deleteGoal = function (goal, index) {
        var goal = $scope.goals.splice(index, 1)[0];
        if (goal && goal.Id != 0) {
            goals.delete(goal.Id).then(function (response) {
            }, function (error) {
            });
        }
    }

    $scope.saveActivity = function () {
        goals.saveActivity(this.activity.GoalId, this.activity.Name, this.activity.Id || 0).then(function (response) {
            $scope.initializeGoals();
            $scope.$parent.initialize();
        }, function (error) {
        });
    }

    $scope.addActivity = function (goal) {
        if (goal.Activities.filter(function (item) {
                return item.Draft == true;
        }).length > 0) {
            focusActivity(goal);
            return;
        }

        $scope.activity.GoalId = goal.Id;
        $scope.activity.Name = 'Activity name..';
        $scope.activity.Draft = true;
        $scope.activity.Id = 0;
        goal.Activities.push($scope.activity);
        focusActivity(goal);
    }

    function focusActivity(goal) {
        $timeout(function () {
            var $elem = $("[data-id=" + goal.Id + "] input:last-of-type");
            $elem.focus();
            $elem.select();
        }, 0);
    }

    $scope.deleteActivity = function (goal, index) {
        var removed = goal.Activities.splice(index, 1)[0];
        if (removed && removed.Id != 0) {
            goals.deleteActivity(removed.Id).then(function (response) {
            }, function (error) {
            });
        }
    }

    $scope.todoItemDialog = function (activity) {
        $scope.initTodoItem(activity.Id);
        $("#addTodoItemDialog").modal();
    }

    $scope.initTodoItem = function (activityId, clear) {
        $scope.todoItem = {
            ActivityId: activityId,
            Description: '',
            Duration: '',
            Resolved: false,
            Deadline: new Date()
        };
    }

    $scope.updateGoal = function (goal, valid) {
        if (valid === false) return;
        goals.updateSetting(goal.Id, parseInt(goal.MinHoursPerWeek), parseInt(goal.MaxHoursPerWeek), goal.Color).then(function (response) {
        }, function (error) {
        });
    }

    $scope.updateActivity = function (activity, valid) {
        if (valid === false) return;
        var plannedCompletionDate = activity.PlannedCompletionDate || null;
        var startDate = activity.StartDate || null;
        goals.updateActivity(activity.Id, activity.Name, startDate, activity.PlannedCompletionDate).then(function (response) {
        }, function (error) {
        });
    }

    $scope.datePickerStart = function (activity) {
        activity.StartEdit = true;
    }

    $scope.datePickerEnd = function (activity) {
        activity.EndEdit = true;
    }

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.dateFormat = 'dd.MM.yyyy';

    $scope.updateGoalName = function (id, value) {
        if (!id || !value) return;
        goals.updateName(id, value).then(function (response) {
        }, function (error) {
        });
    }
});