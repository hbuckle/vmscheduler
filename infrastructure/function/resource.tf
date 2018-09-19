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
  app_settings              = "${var.app_settings}"

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_role_definition" "role_definition" {
  name  = "VirtualMachinePower"
  scope = "${data.azurerm_subscription.subscription.id}"

  permissions {
    actions = [
      "Microsoft.Authorization/*/read",
      "Microsoft.Resources/subscriptions/resourceGroups/read",
      "Microsoft.Compute/virtualMachines/read",
      "Microsoft.Compute/virtualMachines/instanceView/read",
      "Microsoft.Compute/virtualMachines/deallocate/action",
      "Microsoft.Compute/virtualMachines/start/action",
      "Microsoft.Compute/virtualMachines/restart/action",
    ]

    not_actions = []
  }

  assignable_scopes = [
    "${data.azurerm_subscription.subscription.id}",
  ]
}

resource "azurerm_role_assignment" "role_assignment_scheduler_job_collection" {
  scope              = "${var.scheduler_job_collection_id}"
  role_definition_id = "${data.azurerm_subscription.subscription.id}${data.azurerm_builtin_role_definition.builtin_role_definition_owner.id}"
  principal_id       = "${azurerm_function_app.function_app.identity.0.principal_id}"
}

resource "azurerm_role_assignment" "role_assignment_virtual_machine_power" {
  scope              = "${data.azurerm_subscription.subscription.id}"
  role_definition_id = "${azurerm_role_definition.role_definition.id}"
  principal_id       = "${azurerm_function_app.function_app.identity.0.principal_id}"
}
