resource "azurerm_resource_group" "resource_group" {
  name     = "schedule"
  location = "northeurope"
}

resource "azurerm_scheduler_job_collection" "scheduler_job_collection" {
  name                = "jobcollection"
  location            = "${azurerm_resource_group.resource_group.location}"
  resource_group_name = "${azurerm_resource_group.resource_group.name}"
  sku                 = "Standard"
  state               = "Enabled"

  quota {
    max_job_count            = 50
    max_recurrence_frequency = "Minute"
    max_recurrence_interval  = 1
  }
}
