app.service('notificationService', ['$uibModal', '$sce', function ($uibModal, $sce) {
    var modalDefaults = {
        backdrop: true,
        keyboard: true,
        modalFade: true,
        templateUrl: 'templates/modal.html'
    };

    var modalOptions = {
        closeButtonText: 'Close',
        actionButtonText: 'OK',
        headerText: 'Information',
        bodyText: 'Data'
    };

    this.showModal = function (headerText, bodyParams) {
        modalOptions.headerText = headerText;
        modalOptions.bodyText = getBodyText(bodyParams);
        return this.show(modalDefaults, modalOptions);
    };

    function getBodyText(params) {
        var bodyHtml = "<div>";
        for (var i in params) {
            bodyHtml += "<p><strong>" + i + ":</strong> " + params[i] + "</p>";
        }
        bodyHtml += "</div>";
        return $sce.trustAsHtml(bodyHtml);
    }

    this.show = function (customModalDefaults, customModalOptions) {
        //Create temp objects to work with since we're in a singleton service
        var tempModalDefaults = {};
        var tempModalOptions = {};

        //Map angular-ui modal custom defaults to modal defaults defined in service
        angular.extend(tempModalDefaults, modalDefaults, customModalDefaults);

        //Map modal.html $scope custom properties to defaults defined in service
        angular.extend(tempModalOptions, modalOptions, customModalOptions);

        if (!tempModalDefaults.controller) {
            tempModalDefaults.controller = function ($scope, $uibModalInstance) {
                $scope.modalOptions = tempModalOptions;
                $scope.modalOptions.ok = function (result) {
                    $uibModalInstance.close(result);
                };
                $scope.modalOptions.close = function (result) {
                    $uibModalInstance.dismiss('cancel');
                };
            }
        }

        return $uibModal.open(tempModalDefaults).result;
    };
}]);