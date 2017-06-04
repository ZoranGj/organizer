app.factory('goals', ['$http', function ($http) {
    var urlBase = '/goals';
    var goalsService = {};

    goalsService.getGoals = function () {
        return $http.get(urlBase + '/GetAll');
    }

    goalsService.get = function (id) {
        return $http.get(urlBase + '/Get', {
            id: id,
            reload: true
        });
    }

    goalsService.updatePriority = function (goalId, priority){
        return $http.post(urlBase + '/UpdatePriority', {
            id: goalId,
            newPriority: priority
        });
    }

    goalsService.add = function (name, priority) {
        return $http.post(urlBase + '/Add', {
            name: name,
            priority: priority
        });
    }

    goalsService.delete = function (id) {
        return $http.post(urlBase + '/Delete', {
            id: id,
        });
    }

    goalsService.getActivityItems = function (id) {
        return $http.get(urlBase + '/GetActivityItems?goalId='+ id);
    }

    goalsService.saveActivity = function (goalId, name, activityId){
        return $http.post(urlBase + '/SaveActivity', {
            goalId: goalId,
            name: name,
            activityId: activityId || 0
        });
    }

    goalsService.deleteActivity = function (id) {
        return $http.post(urlBase + '/DeleteActivity', {
            id: id,
        });
    }

    goalsService.updateSetting = function (id, minHoursPerWeek, maxHoursPerWeek, color){
        return $http.post(urlBase + '/UpdateSetting', {
            id: id,
            minHoursPerWeek: minHoursPerWeek,
            maxHoursPerWeek: maxHoursPerWeek,
            color: color
        });
    }

    goalsService.updateActivity = function (id, name, startDate, plannedCompletionDate){
        return $http.post(urlBase + '/UpdateActivity', {
            id: id,
            name: name,
            startDate: startDate,
            plannedCompletionDate: plannedCompletionDate
        });
    }

    goalsService.updateName = function (id, value) {
        return $http.post(urlBase + '/UpdateName', {
            id: id,
            value: value,
        });
    }

    return goalsService;
}]);