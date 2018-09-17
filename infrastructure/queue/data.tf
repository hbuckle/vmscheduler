data "azurerm_storage_account_sas" "storage_account_sas" {
  connection_string = "${azurerm_storage_account.storage_account.primary_connection_string}"
  https_only        = true

  resource_types {
    service   = true
    container = true
    object    = true
  }

  services {
    blob  = false
    queue = true
    table = false
    file  = false
  }

  start  = "2018-09-14"
  expiry = "2020-09-15"

  permissions {
    read    = true
    write   = true
    delete  = true
    list    = true
    add     = true
    create  = true
    update  = true
    process = true
  }
}
