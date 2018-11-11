vmscheduler (under construction)
===========

# Overview

An application (and learning project) for scheduling virtual machines in
Azure.

The backend consists of Azure Scheduler, Queue Storage and Functions with
an Angular frontend to manage schedules.

# Development
```
choco install dotnetcore-sdk
choco install azure-functions-core-tools
```

## Azure Functions Core Tools
```
# Update local.settings.json
func azure functionapp fetch-app-settings %functionappname%
(local.settings.json must be present or publishing will not work)

# Deploy
func azure functionapp publish %functionappname%

# Stream logs
func azure functionapp logstream %functionappname%
```

## Frontend - running locally
```
docker pull johnpapa/angular-cli
docker run --rm -it -p 4200:4200 -v .\frontend:/tmp/frontend -w /tmp/frontend johnpapa/angular-cli /bin/sh -c "npm install && ng serve --host 0.0.0.0 --watch --poll 1000"
```