module "queue" {
  source = "./queue"
}

module "scheduler" {
  source = "./scheduler"
}

module "function" {
  source                      = "./function"
  scheduler_job_collection_id = "${module.scheduler.scheduler_job_collection_id}"

  app_settings = {
    "scheduler_job_collection_id" = "${module.scheduler.scheduler_job_collection_id}"
    "queue_scheduler"             = "${module.queue.primary_connection_string}"
    "queue_start"                 = "${module.queue.primary_connection_string}"
    "queue_stop"                  = "${module.queue.primary_connection_string}"
  }
}

module "test_jobs" {
  source                        = "./test_jobs"
  storage_account_name          = "${module.queue.storage_account_name}"
  storage_queue_name            = "scheduler"
  sas_token                     = "${module.queue.storage_account_sas}"
  job_count                     = 5
  resource_group_name           = "${module.scheduler.resource_group_name}"
  scheduler_job_collection_name = "${module.scheduler.scheduler_job_collection_name}"
}
