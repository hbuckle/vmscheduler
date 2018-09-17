resource "azurerm_scheduler_job" "scheduler_job_1" {
  name                = "job1"
  resource_group_name = "${azurerm_resource_group.resource_group.name}"
  job_collection_name = "${azurerm_scheduler_job_collection.scheduler_job_collection.name}"
  state               = "enabled"

  action_storage_queue = {
    storage_account_name = "${var.storage_account_name}"
    storage_queue_name   = "${var.storage_queue_name}"
    sas_token            = "${var.sas_token}"
    message              = "${base64encode(file("${path.module}/message_job1.json"))}"
  }

  recurrence {
    frequency = "Minute"
    interval  = 15
  }
}