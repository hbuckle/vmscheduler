resource "random_string" "random_string" {
  length  = 12
  special = false
  upper   = false
}

resource "azurerm_resource_group" "resource_group" {
  name     = "testvms-${random_string.random_string.result}"
  location = "northeurope"
}

resource "azurerm_virtual_network" "virtual_network" {
  name                = "vnet"
  address_space       = ["10.0.0.0/16"]
  location            = "${azurerm_resource_group.resource_group.location}"
  resource_group_name = "${azurerm_resource_group.resource_group.name}"
}

resource "azurerm_subnet" "subnet" {
  name                 = "subnet"
  resource_group_name  = "${azurerm_resource_group.resource_group.name}"
  virtual_network_name = "${azurerm_virtual_network.virtual_network.name}"
  address_prefix       = "10.0.2.0/24"
}

resource "azurerm_network_interface" "network_interface" {
  count               = 5
  name                = "nic-${count.index}"
  location            = "${azurerm_resource_group.resource_group.location}"
  resource_group_name = "${azurerm_resource_group.resource_group.name}"

  ip_configuration {
    name                          = "configuration"
    subnet_id                     = "${azurerm_subnet.subnet.id}"
    private_ip_address_allocation = "dynamic"
  }
}

resource "azurerm_virtual_machine" "virtual_machine" {
  count                            = 5
  name                             = "vm-${count.index}"
  location                         = "${azurerm_resource_group.resource_group.location}"
  resource_group_name              = "${azurerm_resource_group.resource_group.name}"
  network_interface_ids            = ["${azurerm_network_interface.network_interface.*.id[count.index]}"]
  vm_size                          = "Standard_DS1_v2"
  delete_os_disk_on_termination    = true
  delete_data_disks_on_termination = true

  storage_image_reference {
    publisher = "Canonical"
    offer     = "UbuntuServer"
    sku       = "16.04-LTS"
    version   = "latest"
  }

  storage_os_disk {
    name              = "osdisk-${count.index}"
    caching           = "ReadWrite"
    create_option     = "FromImage"
    managed_disk_type = "Standard_LRS"
  }

  os_profile {
    computer_name  = "hostname"
    admin_username = "testadmin"
    admin_password = "Password1234!"
  }

  os_profile_linux_config {
    disable_password_authentication = false
  }
}
