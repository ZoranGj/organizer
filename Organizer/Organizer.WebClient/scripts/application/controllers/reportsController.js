app.controller('ReportsController', function ($scope, goals, tasks, reports, REPORT_TYPE) {
    //Goal reports
    $scope.goal = 0;

    $scope.initReports = function () {
        $scope.msgNoData = false;
        $scope.displayDiagram = true;
        reports.init();

        goals.getGoals().then(function (response) {
            $scope.goals  = response.data;
            if ($scope.goals.length && $scope.goal == 0) {
                $scope.goal = $scope.goals[0].Id;
            }

            createGoalReport($scope.goals[0].Id);
        }, function (error) {
        });
    }

    $scope.goalChanged = function () {
        createGoalReport($scope.goal);
    }

    function createGoalReport(id) {
        var data = {
            labels: [],
            items: []
        };
        reports.loadProductivityReports($scope.goal).then(function (response) {
            var itemList = response.data;

            if (itemList.length == 0){
                $scope.msgNoData = true;
                $scope.displayDiagram = false;
                return;
            }
            else {
                $scope.msgNoData = false;
                $scope.displayDiagram = true;
            }

            for (var i in itemList) {
                data.labels.push(itemList[i].DisplayLabel);
                data.items.push(itemList[i].ActualTime);
            }
            var firstItem = itemList[0];
            var goalMin = firstItem ? firstItem.MinHoursPerWeek : 0;
            var goalMax = firstItem ? firstItem.MaxHoursPerWeek : 10;
            $("#myChart").remove();
            $("#chartWrapper").append('<canvas id="myChart"></canvas>');
            var ctx = document.getElementById("myChart").getContext('2d');
            reports.create(ctx, REPORT_TYPE.bar, data, 'Productivity', {
                "horizontalLine": [{
                    "y": goalMax,
                    "style": "rgb(105,105,229)",
                    "text": "Ideal"
                }, {
                    "y": goalMin,
                    "style": "rgba(255, 0, 0, .4)",
                    "text": "Min"
                }],
                scales: {
                    yAxes: [{
                        display: true,
                        ticks: {
                            beginAtZero: true,
                            steps: 1,
                            max: goalMax + 2
                        }
                    }]
                }
            });
        }, function (error) {
        });
    }

    //TAGS Reports
    $scope.tag = 0;

    $scope.tagChanged = function () {
        createTagsReport($scope.tag);
    }

    $scope.initTagsReports = function () {
        $scope.msgNoTags = false;
        $scope.displayTagsDiagram = true;

        tasks.getTags().then(function (response) {
            var tags = response.data;
            if (tags.length && $scope.tag == 0) {
                $scope.tags = tags;
                $scope.tag = tags[0].Id;
                createTagsReport($scope.tag);
            }
        }, function (error) {
        });
    }

    function createTagsReport(id) {
        var data = {
            labels: [],
            items: []
        };
        var tagItems = reports.loadTagsReports(id).then(function (response) {
            var itemList = response.data;

            if (itemList.length == 0) {
                $scope.msgNoTags = true;
                $scope.displayTagsDiagram = false;
                return;
            }
            else {
                $scope.msgNoTags = false;
                $scope.displayTagsDiagram = true;
            }

            for (var i in itemList) {
                data.labels.push(itemList[i].DisplayLabel);
                data.items.push(itemList[i].ActualTime);
            }

            $("#myChart").remove();
            $("#chartWrapper").append('<canvas id="myChart"></canvas>');

            var ctx = document.getElementById("myChart").getContext('2d');
            reports.create(ctx, REPORT_TYPE.bar, data, 'Tags', {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                        }
                    }]
                }
            });
        }, function (error) {
        });
    }
});