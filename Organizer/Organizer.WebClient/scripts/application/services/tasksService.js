app.factory('tasks', ['$http', function ($http) {
    var urlBase = '/tasks';
    var tasksService = {};

    tasksService.getAll = function (goalId) {
        return $http.get(urlBase + '/GetAll?goalId='+goalId);
    }

    tasksService.get = function (id) {
        return $http.get(urlBase + '/Get?todoItemId='+ id);
    }

    tasksService.delete = function (taskId) {
        return $http.post(urlBase + '/Delete', {
            todoItemId: taskId
        });
    }

    tasksService.resolve = function (id, resolved) {
        return $http.post(urlBase + '/Resolve', {
            id: id,
            resolved: resolved
        });
    }

    tasksService.add = function (description, deadline, activityId, duration, resolved) {
        return $http.post(urlBase + '/Add', {
            description: description,
            deadline: deadline,
            activityId: activityId,
            duration: duration,
            resolved: resolved
        });
    }

    tasksService.update = function (id, notes, tags) {
        return $http.post(urlBase + '/Update', {
            id: id,
            notes: notes,
            tags: tags
        });
    }

    tasksService.updateDescription = function (id, value) {
        return $http.post(urlBase + '/UpdateDescription', {
            id: id,
            value: value,
        });
    }

    tasksService.updateDuration = function (id, value) {
        return $http.post(urlBase + '/UpdateDuration', {
            id: id,
            value: value
        });
    }

    tasksService.getTags = function () {
        return $http.get(urlBase + '/GetTags');
    }

    tasksService.getTagNames = function () {
        return $http.get(urlBase + '/GetTagNames');
    }

    return tasksService;
}]);