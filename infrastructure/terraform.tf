module "queue" {
  source = "./queue"
}

module "scheduler" {
  source = "./scheduler"
}

module "function" {
  source = "./function"

  app_settings = {
    "scheduler_job_collection_id" = "${module.scheduler.scheduler_job_collection_id}"
    "queue_scheduler"             = "${module.queue.primary_connection_string}"
    "queue_start"                 = "${module.queue.primary_connection_string}"
    "queue_stop"                  = "${module.queue.primary_connection_string}"
  }
}
