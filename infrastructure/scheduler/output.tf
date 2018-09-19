output "scheduler_job_collection_id" {
  value = "${azurerm_scheduler_job_collection.scheduler_job_collection.id}"
}

output "resource_group_name" {
  value = "${azurerm_resource_group.resource_group.name}"
}

output "scheduler_job_collection_name" {
  value = "${azurerm_scheduler_job_collection.scheduler_job_collection.name}"
}
