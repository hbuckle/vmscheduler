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

# Deploy
func azure functionapp publish %functionappname%

# Stream logs
func azure functionapp logstream %functionappname%
```