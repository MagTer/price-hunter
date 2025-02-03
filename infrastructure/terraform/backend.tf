terraform {
  backend "azurerm" {
    resource_group_name  = "TerraformState"
    storage_account_name = "magterpricehunterstate"   # Namnet måste vara unikt och enbart små bokstäver
    container_name       = "tfstate"
    key                  = "price-hunter.terraform.tfstate"
  }
}

