variable "subscription_id" {
  description = "Azure Subscription ID"
  type        = string
  default     = "fc778605-59f1-4fa6-9f4f-838f91bf9d59"
}

variable "tenant_id" {
  description = "Azure Tenant ID"
  type        = string
  default     = "477d0a40-9bcf-45a2-8368-afff7c8bca25"
}

variable "resource_group_name" {
  description = "Name of the Azure Resource Group"
  type        = string
}

variable "location" {
  description = "Azure region where resources will be deployed"
  type        = string
  default     = "West Europe"
}

variable "app_service_plan_name" {
  description = "Name of the App Service Plan"
  type        = string
}

variable "web_app_name" {
  description = "Name of the Web App"
  type        = string
}
