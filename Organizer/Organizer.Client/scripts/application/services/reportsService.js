app.factory('reports', [function (REPORT_TYPE) {
    var reports = {};

    reports.init = function () {
        return;
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
    }

    reports.create = function (ctx, type, data, label, options) {
        if (myChart) {
            myChart.destroy();
        }

        var myChart = new Chart(ctx, {
            type: type,
            data: {
                labels: data.labels,
                datasets: [{
                    label: label,
                    data: data.items,
                    backgroundColor: "rgba(92,184,92,1)",
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
            options: options
        });
    }

    return reports;
}]);