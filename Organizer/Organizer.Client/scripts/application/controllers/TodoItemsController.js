app.controller('TodoItemsController', function ($scope, $compile, uiCalendarConfig) {
    $scope.todoItem = {
        ActivityId: 0,
        Deadline: new Date()
    }

    $scope.filter = {
        category: 0,
        status: 0
    };

    $scope.initializeTodoItems = function () {
        var categories = appController.getCategories();
        $scope.categories = JSON.parse(categories);

        $scope.initializeTodoItemsByCategory($scope.filter.category);
    }

    $scope.categoryChanged = function () {
        $scope.initializeTodoItemsByCategory($scope.filter.category);
    }

    $scope.statusChanged = function () {

    }

    $scope.initializeTodoItemsByCategory = function (categoryId) {
        var todoItems = appController.getTodoItems(categoryId);
        $scope.todoItems = JSON.parse(todoItems);

        var activityItems = appController.getActivityItems(categoryId);
        $scope.activityItems = JSON.parse(activityItems);
    }

    $scope.addTodoItem = function () {
        if ($scope.todoItem.ActivityId) {
            appController.addTodoItem($scope.todoItem.Description, new Date($scope.todoItem.Deadline), $scope.todoItem.ActivityId, parseInt($scope.todoItem.Duration));
            $scope.initializeTodoItems();
        }
    }

    $scope.deleteTodoItem = function (todoItemId) {
        appController.deleteTodoItem(todoItemId);
        $scope.initializeTodoItems();
    }

    $scope.onTimeSet = function (newDate, oldDate) {
        $scope.todoItem.Deadline = newDate;
    }

    $scope.resolveItem = function (id) {
        appController.resolveTodoItem(id, this.item.Resolved);
    }

    $scope.todoItemClass = function (item) {
        return item.Resolved ? 'resolved' : '';
    }

    $scope.updateTodoItem = function (item) {
        appController.updateTodoItem(item.Id, item.Notes);
    }

    $scope.init = function () {
        var todoItems = appController.getTodoItems(0);
        var t = JSON.parse(todoItems);
        for (var i in t) {
            var y = new Date(t[i].Deadline);
            y.setHours(y.getHours() - t[i].Duration)
            $scope.events.push({
                title: t[i].Description,
                start: y,
                end: t[i].Deadline,
            });
        }


        gapi.load('client:auth2', function(){
            

            gapi.client.init({
                apiKey: 'AIzaSyA3dxTjqHLI3PKMmG06jwBnA7Jpbd2ehlU',
                clientId: '731395181410-s9c1ndqht9e6mnkf20v4tvcjm9efu39j.apps.googleusercontent.com',
            }).then(function () {
                // Listen for sign-in state changes.
                gapi.auth2.getAuthInstance().isSignedIn.listen(updateSigninStatus);

                // Handle the initial sign-in state.
                updateSigninStatus(gapi.auth2.getAuthInstance().isSignedIn.get());

                if (!gapi.auth2.getAuthInstance().isSignedIn.get()) {
                    gapi.auth2.getAuthInstance().signIn();
                }

                var event = {
                    'summary': 'Google I/O 2015',
                    'location': '800 Howard St., San Francisco, CA 94103',
                    'description': 'A chance to hear more about Google\'s developer products.',
                    'start': {
                        'dateTime': '2015-05-28T09:00:00-07:00',
                        'timeZone': 'America/Los_Angeles'
                    },
                    'end': {
                        'dateTime': '2015-05-28T17:00:00-07:00',
                        'timeZone': 'America/Los_Angeles'
                    },
                    'recurrence': [
                      'RRULE:FREQ=DAILY;COUNT=2'
                    ],
                    'attendees': [
                      { 'email': 'lpage@example.com' },
                      { 'email': 'sbrin@example.com' }
                    ],
                    'reminders': {
                        'useDefault': false,
                        'overrides': [
                          { 'method': 'email', 'minutes': 24 * 60 },
                          { 'method': 'popup', 'minutes': 10 }
                        ]
                    }
                };

                var request = gapi.client.calendar.events.insert({
                    'calendarId': 'zoran.gjuroski@gmail.com',
                    'resource': event
                });

                request.execute(function (event) {
                    appendPre('Event created: ' + event.htmlLink);
                });
            });
        });
    }

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $scope.changeTo = 'Hungarian';
    /* event source that contains custom events on the scope */

    $scope.events = [];
    //$scope.events = [
    //  { title: 'All Day Event', start: new Date(y, m, 1) },
    //  { title: 'Long Event', start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) },
    //  { id: 999, title: 'Repeating Event', start: new Date(y, m, d - 3, 16, 0), allDay: false },
    //  { id: 999, title: 'Repeating Event', start: new Date(y, m, d + 4, 16, 0), allDay: false },
    //  { title: 'Birthday Party', start: new Date(y, m, d + 1, 19, 0), end: new Date(y, m, d + 1, 22, 30), allDay: false },
    //  { title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
    //];
    /* event source that calls a function on every view switch */
    $scope.eventsF = function (start, end, timezone, callback) {
        var s = new Date(start).getTime() / 1000;
        var e = new Date(end).getTime() / 1000;
        var m = new Date(start).getMonth();
        var events = [{ title: 'Feed Me ' + m, start: s + (50000), end: s + (100000), allDay: false, className: ['customFeed'] }];
        callback(events);
    };

    $scope.calEventsExt = {
        color: '#f00',
        textColor: 'yellow',
        events: [
           { type: 'party', title: 'Lunch', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
           { type: 'party', title: 'Lunch 2', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
           { type: 'party', title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
        ]
    };
    /* alert on eventClick */
    $scope.alertOnEventClick = function (date, jsEvent, view) {
        $scope.alertMessage = (date.title + ' was clicked ');
    };
    /* alert on Drop */
    $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
        $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
    };
    /* alert on Resize */
    $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
        $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
    };
    /* add and removes an event source of choice */
    $scope.addRemoveEventSource = function (sources, source) {
        var canAdd = 0;
        angular.forEach(sources, function (value, key) {
            if (sources[key] === source) {
                sources.splice(key, 1);
                canAdd = 1;
            }
        });
        if (canAdd === 0) {
            sources.push(source);
        }
    };
    /* add custom event*/
    $scope.addEvent = function () {
        $scope.events.push({
            title: 'Open Sesame',
            start: new Date(y, m, 28),
            end: new Date(y, m, 29),
            className: ['openSesame']
        });
    };
    /* remove event */
    $scope.remove = function (index) {
        $scope.events.splice(index, 1);
    };
    /* Change View */
    $scope.changeView = function (view, calendar) {
        uiCalendarConfig.calendars[calendar].fullCalendar('changeView', view);
    };
    /* Change View */
    $scope.renderCalender = function (calendar) {
        if (uiCalendarConfig.calendars[calendar]) {
            uiCalendarConfig.calendars[calendar].fullCalendar('render');
        }
    };
    /* Render Tooltip */
    $scope.eventRender = function (event, element, view) {
        element.attr({
            'tooltip': event.title,
            'tooltip-append-to-body': true
        });
        $compile(element)($scope);
    };
    /* config object */
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

    $scope.changeLang = function () {
        if ($scope.changeTo === 'Hungarian') {
            $scope.uiConfig.calendar.dayNames = ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"];
            $scope.uiConfig.calendar.dayNamesShort = ["Vas", "Hét", "Kedd", "Sze", "Csüt", "Pén", "Szo"];
            $scope.changeTo = 'English';
        } else {
            $scope.uiConfig.calendar.dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $scope.uiConfig.calendar.dayNamesShort = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            $scope.changeTo = 'Hungarian';
        }
    };
    /* event sources array*/
    $scope.eventSources = [$scope.events, $scope.eventSource, $scope.eventsF];
    $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];
});