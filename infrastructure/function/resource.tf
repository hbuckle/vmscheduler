resource "azurerm_resource_group" "resource_group" {
  name     = "function"
  location = "northeurope"
}

resource "random_string" "random_string" {
  length  = 24
  special = false
  upper   = false
}

resource "azurerm_storage_account" "storage_account" {
  name                     = "${random_string.random_string.result}"
  resource_group_name      = "${azurerm_resource_group.resource_group.name}"
  location                 = "${azurerm_resource_group.resource_group.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "app_service_plan" {
  name                = "appserviceplan"
  resource_group_name = "${azurerm_resource_group.resource_group.name}"
  location            = "${azurerm_resource_group.resource_group.location}"
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "function_app" {
  name                      = "${random_string.random_string.result}"
  resource_group_name       = "${azurerm_resource_group.resource_group.name}"
  location                  = "${azurerm_resource_group.resource_group.location}"
  app_service_plan_id       = "${azurerm_app_service_plan.app_service_plan.id}"
  storage_connection_string = "${azurerm_storage_account.storage_account.primary_connection_string}"
  version                   = "~2"

  app_settings {
    queue_scheduler       = "${var.queue_scheduler_connection}"
    queue_start           = "${var.queue_start_connection}"
    queue_stop            = "${var.queue_stop_connection}"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_role_assignment" "role_assignment" {
  scope              = "${data.azurerm_subscription.subscription.id}"
  role_definition_id = "${data.azurerm_subscription.subscription.id}${data.azurerm_builtin_role_definition.builtin_role_definition.id}"
  principal_id       = "${azurerm_function_app.function_app.identity.0.principal_id}"
}
