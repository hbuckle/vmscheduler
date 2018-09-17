output "storage_account_name" {
  value = "${azurerm_storage_account.storage_account.name}"
}

output "primary_connection_string" {
  value = "${azurerm_storage_account.storage_account.primary_connection_string}"
}

output "storage_account_sas" {
  value = "${data.azurerm_storage_account_sas.storage_account_sas.sas}"
}
