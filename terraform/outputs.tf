output "resource_group_name" {
  value = azurerm_resource_group.main.name
  description = "The name of the resource group"
}

output "app_service_plan_id" {
  value = azurerm_service_plan.main.id
  description = "The ID of the App Service Plan"
}

output "web_app_id" {
  value = azurerm_windows_web_app.main.id
  description = "The ID of the Web App"
}
