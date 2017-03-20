app.controller('ReportsController', function ($scope) {
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
            var productivityItems = appController.loadProductivityReports($scope.category);
            var itemList = JSON.parse(productivityItems);

            var labels = [];
            var items = [];
            var plannedTime = 0;

            for (var i in itemList) {
                labels.push('Week ' + i + ' - ' + itemList[i].From);
                items.push(itemList[i].ActualTime);
            }

            plannedTime = itemList[0].PlannedTime;

            var ctx = document.getElementById("myChart").getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Productivity',
                        data: items,
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