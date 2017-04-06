app.controller('ReportsController', function ($scope, reports, REPORT_TYPE) {
    $scope.categoryChanged = function () {
        $scope.initReports($scope.category);
    }

    $scope.category = 0;

    $scope.initReports = function () {
        var categories = categoriesCtrl.getAll();
        $scope.categories = JSON.parse(categories);

        if (categories.length && $scope.category == 0) {
            $scope.category = $scope.categories[0].Id;
        }

        if ($scope.category != 0) {
            reports.init($scope.category);

            //todo: create directive for report
            var ctx = document.getElementById("myChart").getContext('2d');
            var horizonalLinePlugin = {
                afterDraw: function (chartInstance) {
                    var yScale = chartInstance.scales["y-axis-0"];
                    var canvas = chartInstance.chart;
                    var ctx = canvas.ctx;
                    var index;
                    var line;
                    var style;

                    if (chartInstance.options.horizontalLine) {
                        for (index = 0; index < chartInstance.options.horizontalLine.length; index++) {
                            line = chartInstance.options.horizontalLine[index];

                            if (!line.style) {
                                style = "rgba(169,169,169, .6)";
                            } else {
                                style = line.style;
                            }

                            if (line.y) {
                                yValue = yScale.getPixelForValue(line.y);
                            } else {
                                yValue = 0;
                            }

                            ctx.lineWidth = 3;

                            if (yValue) {
                                ctx.beginPath();
                                ctx.moveTo(0, yValue);
                                ctx.lineTo(canvas.width, yValue);
                                ctx.strokeStyle = style;
                                ctx.stroke();
                            }

                            if (line.text) {
                                ctx.fillStyle = style;
                                ctx.fillText(line.text, 0, yValue + ctx.lineWidth);
                            }
                        }
                        return;
                    };
                }
            };
            Chart.pluginService.register(horizonalLinePlugin);

            var categoryMin = parseInt($scope.category.minHoursPerWeek);
            var categoryMax = parseInt($scope.category.maxHoursPerWeek);
            var myChart = new Chart(ctx, {
                type: REPORT_TYPE.bar,
                data: {
                    labels: reports.labels(),
                    datasets: [{
                        label: 'Productivity',
                        data: reports.items(),
                        backgroundColor: "rgba(153,255,51,1)",
                        fill: false,
                        lineTension: 0.1,
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: 'miter',
                        pointBorderColor: "rgba(75,192,192,1)",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(75,192,192,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 100,
                    }],
                },
                options: {
                    "horizontalLine": [{
                        "y": categoryMax,
                        "style": "blue",
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
                }
            });
        }
    }

    $scope.tagChanged = function () {
        $scope.initReportsTags($scope.tag);
    }

    $scope.tag = 0;

    $scope.initReportsTags = function () {
        var tags = todosCtrl.getTags();
        $scope.tags = JSON.parse(tags);
        
        if (tags.length && $scope.tag == 0) {
            $scope.tag = $scope.tags[0].Id;
        }

        if ($scope.tag != 0) {
            var rlabels = [];
            var ritems = [];

            var rItems = reportsCtrl.loadTagsReports($scope.tag);
            var ritemList = JSON.parse(rItems);

            for (var i in ritemList) {
                rlabels.push(ritemList[i].DisplayLabel);
                ritems.push(ritemList[i].ActualTime);
            }

            var ctx = document.getElementById("myChart").getContext('2d');
            var horizonalLinePlugin = {
                afterDraw: function (chartInstance) {
                    var yScale = chartInstance.scales["y-axis-0"];
                    var canvas = chartInstance.chart;
                    var ctx = canvas.ctx;
                    var index;
                    var line;
                    var style;

                    if (chartInstance.options.horizontalLine) {
                        for (index = 0; index < chartInstance.options.horizontalLine.length; index++) {
                            line = chartInstance.options.horizontalLine[index];

                            if (!line.style) {
                                style = "rgba(169,169,169, .6)";
                            } else {
                                style = line.style;
                            }

                            if (line.y) {
                                yValue = yScale.getPixelForValue(line.y);
                            } else {
                                yValue = 0;
                            }

                            ctx.lineWidth = 3;

                            if (yValue) {
                                ctx.beginPath();
                                ctx.moveTo(0, yValue);
                                ctx.lineTo(canvas.width, yValue);
                                ctx.strokeStyle = style;
                                ctx.stroke();
                            }

                            if (line.text) {
                                ctx.fillStyle = style;
                                ctx.fillText(line.text, 0, yValue + ctx.lineWidth);
                            }
                        }
                        return;
                    };
                }
            };
            Chart.pluginService.register(horizonalLinePlugin);
            var myChart = new Chart(ctx, {
                type: REPORT_TYPE.bar,
                data: {
                    labels: rlabels,
                    datasets: [{
                        label: 'Tags',
                        data: ritems,
                        backgroundColor: "rgba(153,255,51,1)",
                        fill: false,
                        lineTension: 0.1,
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: 'miter',
                        pointBorderColor: "rgba(75,192,192,1)",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(75,192,192,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 100,
                    }],
                },
            });
        }
    }
});