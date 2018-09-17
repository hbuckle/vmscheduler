resource "azurerm_resource_group" "resource_group" {
  name     = "queue"
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

resource "azurerm_storage_queue" "storage_queue_scheduler" {
  name                 = "scheduler"
  resource_group_name  = "${azurerm_resource_group.resource_group.name}"
  storage_account_name = "${azurerm_storage_account.storage_account.name}"
}

resource "azurerm_storage_queue" "storage_queue_stop" {
  name                 = "stop"
  resource_group_name  = "${azurerm_resource_group.resource_group.name}"
  storage_account_name = "${azurerm_storage_account.storage_account.name}"
}

resource "azurerm_storage_queue" "storage_queue_start" {
  name                 = "start"
  resource_group_name  = "${azurerm_resource_group.resource_group.name}"
  storage_account_name = "${azurerm_storage_account.storage_account.name}"
}
