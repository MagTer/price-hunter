variable "resource_group_name" {
  type        = string
  description = "The name of the resource group"
  default     = "price-hunter-rg"
}

variable "location" {
  type        = string
  description = "The Azure region to deploy resources"
  default     = "westeurope"
}

variable "app_service_plan_name" {
  type        = string
  description = "The name of the App Service Plan"
  default     = "price-hunter-asp"
}

variable "web_app_name" {
  type        = string
  description = "The name of the Web App"
  default     = "price-hunter-app"
}

variable "storage_account_name" {
  type        = string
  description = "The name of the Storage Account for Terraform state"
}

variable "subscription_id" {
  type        = string
  description = "The Azure subscription ID"
}

variable "tenant_id" {
  type        = string
  description = "The Azure tenant ID"
}
