output "web_app_default_host_name" {
  description = "The default hostname of the deployed web app"
  value       = azurerm_app_service.webapp.default_site_hostname
}
