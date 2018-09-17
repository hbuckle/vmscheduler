vmscheduler
===========

# Development
```
choco install dotnetcore-sdk
choco install azure-functions-core-tools --pre
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