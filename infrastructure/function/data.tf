data "azurerm_subscription" "subscription" {}

data "azurerm_builtin_role_definition" "builtin_role_definition_reader" {
  name = "Reader"
}

data "azurerm_builtin_role_definition" "builtin_role_definition_owner" {
  name = "Owner"
}
