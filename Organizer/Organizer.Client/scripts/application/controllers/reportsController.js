app.controller('ReportsController', function ($scope, reports, REPORT_TYPE) {
    //Category reports
    $scope.category = 0;

    $scope.initReports = function () {
        reports.init();

        $scope.categories = JSON.parse(categoriesCtrl.getAll());
        if ($scope.categories.length && $scope.category == 0) {
            $scope.category = $scope.categories[0].Id;
        }

        createCategoryReport($scope.categories[0].Id);
    }

    $scope.categoryChanged = function () {
        createCategoryReport($scope.category);
    }

    function createCategoryReport(id) {
        var data = {
            labels: [],
            items: []
        };
        var productivityItems = reportsCtrl.loadProductivityReports($scope.category);
        var itemList = JSON.parse(productivityItems);

        for (var i in itemList) {
            data.labels.push(itemList[i].DisplayLabel);
            data.items.push(itemList[i].ActualTime);
        }
        var category = JSON.parse(categoriesCtrl.getCategory($scope.category));
        var categoryMin = category ? category.MinHoursPerWeek : 0;
        var categoryMax = category ? category.MaxHoursPerWeek : 10;
        $("#myChart").remove();
        $("#chartWrapper").append('<canvas id="myChart"></canvas>');
        var ctx = document.getElementById("myChart").getContext('2d');
        reports.create(ctx, REPORT_TYPE.bar, data, 'Productivity', {
            "horizontalLine": [{
                "y": categoryMax,
                "style": "rgb(105,105,229)",
                "text": "Ideal"
            }, {
                "y": categoryMin,
                "style": "rgba(255, 0, 0, .4)",
                "text": "Min"
            }],
            scales: {
                yAxes: [{
                    display: true,
                    ticks: {
                        beginAtZero: true,
                        steps: 1,
                        max: categoryMax + 2
                    }
                }]
            }
        });
    }

    //TAGS Reports
    $scope.tag = 0;

    $scope.tagChanged = function () {
        createTagsReport($scope.tag);
    }

    $scope.initTagsReports = function () {
        var tags = todosCtrl.getTags();
        $scope.tags = JSON.parse(tags);
        
        if (tags.length && $scope.tag == 0) {
            $scope.tag = $scope.tags[0].Id;
            createTagsReport($scope.tag);
        }
    }

    function createTagsReport(id) {
        var data = {
            labels: [],
            items: []
        };
        var tagItems = reportsCtrl.loadTagsReports(id);
        var itemList = JSON.parse(tagItems);

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
    }
});