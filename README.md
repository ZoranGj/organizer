# Organizer

The endless strive for improving my daily productivity and managing my personal long distance goals, activities and daily tasks led me and  motivated me to create this tool - Organizer.
The application flow is as follows:
1. Register in less than 1 minute
2. Create your goals
3. Goal settings - add minimum and maximum planned working hours per week and customize goals (colors etc..)
4. Create your goal activities
5. Add tasks on the activities and as time goes, resolve them

After that, you can track the results:
1. Productivity reports: see if the time of the resolved tasks for a goal matches the planned time (minimum and maximum planned hours)
2. Track productivity in an interactive calendar
3. Productivity notifications: track tasks that are about to expire


# Project Architecture

The solution consists of a project that combines data access and logic, and two client projects (desktop and web).

## Database and Access

The database is modeled using **Entity Framework Model First**, and the data access is implemented using **The Repository Pattern** and generic repository. All the logic is placed in the project **Organizer.Model**.

## Web client

The solution has a web application named **Organizer.WebClient** which combines **ASP.NET MVC** framework and **AngularJS** framework on client side.

*soon to be released*

## Desktop client

The first client added was a desktop client which is implemented using **CefSharp framework** which enables you to use web technologies (HTML, CSS, Javascript) for constructing desktop solutions and **AngularJS** framework.
