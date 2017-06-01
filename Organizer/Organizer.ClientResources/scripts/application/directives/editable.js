app.directive('editable', [function ($timeout) {
    return {
        element: 'EA',
        scope: {
            id: '=',
            color: '=',
            model: '=value',
            save: '&onEdit',
        },
        link: function (scope, element, attributes) {
            var previousValue;
            scope.edit = function () {
                scope.editInput = true;
                previousValue = scope.model;

                setTimeout(function () {
                    element.find('input')[0].focus();
                }, 300);
            }

            scope.cancel = function () {
                scope.model = previousValue;
                scope.editInput = false;
            }

            scope.saveValue = function () {
                scope.save({ id: scope.id, value: scope.model });
                scope.editInput = false;
            }
        },
        templateUrl: 'templates/editableInput.html',
    };
}]);