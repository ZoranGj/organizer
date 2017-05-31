app.controller('CalendarController', ['$scope', '$compile', 'uiCalendarConfig', 'notificationService',
function ($scope, $compile, uiCalendarConfig, notificationService) {
    
    $scope.events = [];
    $scope.calendarGoal = 0;

    $scope.loadTodoItemData = function (id) {
        var data = JSON.parse(todosCtrl.get(id));
        notificationService.showModal('Todo item information', {
            "Description": data.Description,
            "Activity": data.Activity,
            "Activity is from goal": data.Goal
        });
        return data;
    }

    $scope.initCalendarView = function () {
        var goals = goalsCtrl.getAll();
        $scope.calendargoals = JSON.parse(goals);
        $scope.calendarGoalChanged();
    }

    $scope.initializeCalendar = function (todos) {
        if ($scope.events.length) {
            $scope.events = [];
        }
        for (var i in todos) {
            var start = new Date(todos[i].Deadline);
            start.setHours(start.getHours() - todos[i].Duration)
            $scope.events.push({
                id: todos[i].Id,
                title: todos[i].Description,
                start: start,
                activity: todos[i].Activity,
                end: new Date(todos[i].Deadline),
                duration: todos[i].Duration,
                addedOn: todos[i].AddedOn,
                goal: todos[i].Goal,
                backgroundColor: todos[i].Color + ' !important',
            });
        }
    }

    $scope.calendarGoalChanged = function () {
        var todoItems = todosCtrl.getAll($scope.calendarGoal);
        $scope.initializeCalendar(JSON.parse(todoItems));
    }

    $scope.alertOnEventClick = function (date, jsEvent, view) {
        $scope.alertMessage = (date.title + ' was clicked ');
    };

    $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
        $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
    };

    $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
        $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
    };

    /* Render Tooltip */
    $scope.eventRender = function (event, element, view) {
        element.attr({
            'calendar-event' : '',
            'data': 'loadTodoItemData',
            'id': event.id,
        });
        $(element).css({
            'background-color': event.Color + ' !important'
        });
        $(element).tooltip({
            title: '<strong class="green">'+ event.goal +'</strong><br /><strong>' + event.activity + '</strong>. Duration: ' + event.duration + ' hours.',
            placement: 'bottom',
            html: true
        });
        $compile(element)($scope);
    };

    $scope.uiConfig = {
        calendar: {
            height: 450,
            editable: true,
            header: {
                left: 'title',
                center: '',
                right: 'today prev,next'
            },
            eventClick: $scope.alertOnEventClick,
            eventDrop: $scope.alertOnDrop,
            eventResize: $scope.alertOnResize,
            eventRender: $scope.eventRender
        }
    };

    $scope.changeView = function (view, calendar) {
        uiCalendarConfig.calendars.calendar.fullCalendar('changeView', view);
    };

    $scope.renderCalender = function (calendar) {
        if (uiCalendarConfig.calendars.calendar) {
            uiCalendarConfig.calendars.calendar.fullCalendar('render');
        }
    };

    $scope.eventSources = [$scope.events, $scope.eventSource, $scope.eventsF];
    $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];
}]);