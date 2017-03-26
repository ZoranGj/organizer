app.controller('ReportsController', function ($scope, reports, REPORT_TYPE) {
    $scope.categoryChanged = function () {
        $scope.initReports($scope.category);
    }

    $scope.category = 0;

    $scope.initReports = function () {
        var categories = appController.getCategories();
        $scope.categories = JSON.parse(categories);

        if (categories.length && $scope.category == 0) {
            $scope.category = $scope.categories[0].Id;
        }

        if ($scope.category != 0) {
            reports.init($scope.category);

            //plannedTime = itemList[0].PlannedTime;

            //todo: create directive for report
            var ctx = document.getElementById("myChart").getContext('2d');
            var myChart = new Chart(ctx, {
                type: REPORT_TYPE.bar,
                data: {
                    labels: reports.labels(),
                    datasets: [{
                        label: 'Productivity',
                        data: reports.items(),
                        backgroundColor: "rgba(153,255,51,1)"
                    }
                    //, {
                    //    label: 'oranges',
                    //    data: [30, 29, 5, 5, 20, 3, 10],
                    //    backgroundColor: "rgba(255,153,0,1)"
                    //}
                    ]
                }
            });
        }
    }
});