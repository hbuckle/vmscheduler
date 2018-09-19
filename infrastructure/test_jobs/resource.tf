resource "azurerm_scheduler_job" "scheduler_job" {
  count               = "${var.job_count}"
  name                = "job${count.index}"
  resource_group_name = "${var.resource_group_name}"
  job_collection_name = "${var.scheduler_job_collection_name}"
  state               = "enabled"

  action_storage_queue = {
    storage_account_name = "${var.storage_account_name}"
    storage_queue_name   = "${var.storage_queue_name}"
    sas_token            = "${var.sas_token}"
    message              = "${base64encode(file("${path.module}/message.json"))}"
  }

  recurrence {
    frequency = "Minute"
    interval  = 15
  }
}
