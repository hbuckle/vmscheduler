module "queue" {
  source = "./queue"
}

module "scheduler" {
  source               = "./scheduler"
  storage_account_name = "${module.queue.storage_account_name}"
  storage_queue_name   = "scheduler"
  sas_token            = "${module.queue.storage_account_sas}"
}

module "function" {
  source                     = "./function"
  queue_scheduler_connection = "${module.queue.primary_connection_string}"
  queue_start_connection     = "${module.queue.primary_connection_string}"
  queue_stop_connection      = "${module.queue.primary_connection_string}"
}
