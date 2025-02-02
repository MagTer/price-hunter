# price-hunter
Web and functions to track prices for items in online stores


flowchart TD
    A[Git Repository<br>(price-hunter)]
    B[Azure Pipelines<br>CI/CD Pipeline]
    C[Build Stage<br>(.NET Build, Test, Publish Artifacts)]
    D[Deploy Stage<br>(Terraform & Azure CLI)]
    E[Azure Resource Group]
    F[App Service Plan]
    G[Azure Web App]
    H[Federated Identity<br>(Managed Identity via Entra)]
    
    A --> B
    B --> C
    C --> D
    D --> E
    E --> F
    F --> G
    D --> H
