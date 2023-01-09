# Asset Admin Shell Repository

IEC 63278 Asset Admin Shell Repository reference implementation using C# and .Net6.0.

# Features

1. Loading of Asset Admin Shells V1, V2 and V3 (but always saving in V3 format, leveraging XML)
1. Standardized AAS REST interface (as specified in part 2 of the AAS spec)
1. Swagger endpoint at /swagger
1. IDTA AASX Package Explorer client interface support
1. Joint CESMII/Plattform Industrie 4.0/IDTA/DTC Carbon Reporting Demonstrator integration
1. WattTime and CarbonIntensity.co.uk integration
1. Azure Data Explorer data source integration
1. Integrated AAS browser
1. Integrated OPC UA Nodeset file browser
1. Integrated AutomationML file browser
1. Basic authentication header support (use "admin" and ServicePassword environment variable to set password)
1. Local storage of AASX package files (in the server's root directory)
1. Cloud storage of AASX package files

# Usage

Docker containers are automatically built. Simply run the app on a Docker-enabled PC via:

docker run -p 80:80 ghcr.io/digitaltwinconsortium/aas-repository:masterv3

And then point your browser to http://localhost.

# Required Environment variables

1. ServicePassword: Password to access the service via basic authentication header

# Optional Environment variables

1. HostingPlatform: The hosting platoform of the repository. Current options are `Azure` (the default is to run it locally)
1. UACLUsername: OPC Foundation UA Cloud Library username
1. UACLPassword: OPC Foundation UA Cloud Library password
1. ADX_HOST: Azure Data Explorer host endpoint
1. ADX_DB: Azure Data Explorer database name
1. AAD_TENANT: Azure Active Directory tenant ID of your Azure subscription (GUID)
1. AAD_APPLICATION_ID: Azure Active Directory application ID (GUID, set this up via an app registration in the Azure Portal)
1. AAD_APPLICATION_KEY: Azure Active Directory application key (with data owner access to ADX cluster and database, create a secret within your AAD app registration)
1. ADX_QUERY_INTERVAL: The query interval for the ADX database
1. WATTTIME_USER: WattTime service username
1. WATTTIME_PASSWORD: WattTime service password
1. WATTTIME_LATITUDE: WattTime location to query
1. WATTTIME_LONGITUDE: WattTime location to query
