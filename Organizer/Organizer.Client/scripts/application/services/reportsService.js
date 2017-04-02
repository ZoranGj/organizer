app.factory('reports', [function () {
    var labels;
    var items;

    return {
        init: function (categoryId) {
            labels = [];
            items = [];

            var productivityItems = appController.loadProductivityReports(categoryId);
            var itemList = JSON.parse(productivityItems);
            var plannedTime = 0;

            for (var i in itemList) {
                labels.push(itemList[i].DisplayLabel);
                items.push(itemList[i].ActualTime);
            }
        },

        labels: function () {
            return labels;
        },

        items: function () {
            return items;
        }
    };
}]);