app.controller('GoalsController', function ($scope, $timeout, COLORS) {
    $scope.goal = {
        name: '',
        color: 'rgb(92,184,92)',
    };
    $scope.activity = {};
    $scope.activities = [];
    $scope.showicons = false;
    $scope.showNewGoal = false;
    $scope.editableColor = COLORS.white;

    $scope.initializeGoals = function () {
        var goals = goalsCtrl.getAll();
        $scope.goals = JSON.parse(goals);

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
                    goalsCtrl.updatePriority(goalId, priority + 1);
                }
            }
        });
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
            goalsCtrl.add($scope.goal.name.toString(), priority);
            $scope.initializeGoals();
        }
    }

    $scope.deleteGoal = function (goal, index) {
        var goal = $scope.goals.splice(index, 1)[0];
        if (goal && goal.Id != 0) {
            goalsCtrl.delete(goal.Id);
        }
    }

    $scope.saveActivity = function () {
        goalsCtrl.saveActivity(this.activity.GoalId, this.activity.Name, this.activity.Id || 0);
        $scope.initializeGoals();
        $scope.$parent.initialize();
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
            goalsCtrl.deleteActivity(removed.Id);
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
        goalsCtrl.updateSetting(goal.Id, parseInt(goal.MinHoursPerWeek), parseInt(goal.MaxHoursPerWeek), goal.Color);
    }

    $scope.updateGoalName = function (id, value) {
        if (!id || !value) return;
        goalsCtrl.updateName(id, value);
    }
});