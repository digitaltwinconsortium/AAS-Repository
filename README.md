# Asset Admin Shell Repository

IEC 63278 Asset Admin Shell Repository reference implementation using C# and .Net6.0.

# Features

1. Loading of Asset Admin Shells V1, V2 and V3 (both XML and JSON encoding), but always saving in V3 format, leveraging both JSON and XML)
1. Loading of OPC UA nodeset2 XML files as AAS models
1. Standardized AAS REST interface (as specified in part 2 of the AAS spec)
1. Swagger endpoint at /swagger
1. IDTA AASX Package Explorer client interface support
1. Joint CESMII/Plattform Industrie 4.0/IDTA/DTC Carbon Reporting Demonstrator integration
1. Product Carbon Footprint (PCF) calculation (based on GHG Protocol specification) for the production line simulation which is part of the [DTC's Manufacturing Ontologies reference solution](https://github.com/digitaltwinconsortium/ManufacturingOntologies).
1. WattTime integration
1. OPC UA PubSub operational data integration
1. UA Cloud Library integration
1. Azure Data Explorer data source integration
1. Integrated AAS browser
1. Integrated OPC UA Nodeset file browser
1. Integrated AutomationML file browser
1. Basic authentication header support (use "admin" and ServicePassword environment variable to set password)
1. Local storage of AASX package files (in the server's root directory)
1. Cloud storage of AASX package files
1. CESMII Smart Manufacturing Innovation Plattform (SMIP) imtegration


# Calculating the Product Carbon Footprint (PCF)

One of the most popular use cases for the Asset Administration Shell (AAS) is to make the Product Carbon Footprint (PCF) of manufactured products available to customers of those products. In fact, the AAS will most likely become the underlying technology in the upcoming [Digital Product Passport (DPP)](https://circulareconomy.europa.eu/platform/en/news-and-events/all-events/eu-digital-product-passport-learning-frontrunners) initiative from the European Union. To calculate the PCF, all three scopes (1, 2 & 3) of emissions need to be taken into account. 

### Scope 1 Emissions

These emissions come from all sources the manufacturer uses to burn fossil fuels, either during production (for example when the manufacturer has a natural gas-powered production process) or before (for example picking up parts by truck) or afterwards (for example the cars of sales people or the delivery trucks with the produced products). They are relatively easy to calculate as the emissions from fossil fuel-powered engines are a well-understood quantity. This reference solution simply adds a fixed value for scope 1 emissions to the total product carbon footprint.

### Scope 2 Emissions

These emissions come from the electricity used during production. If the manufacturer uses a 100% renewable energy provider, the scope 2 emissions are zero. However, most manufacturers have long-term contracts with energy providers and need to ask their energy provider for the carbon intensity per KWh of energy delivered. If this data is not available, an average for the electricity grid region the manufacturing site is in should be used. This data is available through services like [WattTime](https://www.watttime.org) and this is what this reference solution uses. The PCF calculation first checks if a new product was successfully produced by the production line, retrieves the produced product's serial number, followed by the energy consumption of each machine of the production line while the new product was produced by the machine and then applies the carbon intensity to the sum of all machines' energy consumption.

### Scope 3 Emissions

These emissions come from the parts and raw materials used within the product being manufactured as well as from using the product by the end customer (and getting it into the customer's hands in the first place!) and are the hardest to calculate simply due to a lack of data from the worldwide suppliers manufacturer uses today. Unfortunately, scope 3 emissions make up almost 90% of the emissions in manufacturing. However, this is where the AAS can help create a standardized interface and data model to provide and retrieve scope 3 emissions. This reference solution does just that by making an AAS available for each manufactured product built by the simulated production line and also reads PCF data from another AAS simulating a manufacturing supply chain.


# Usage

Docker containers are automatically built. Simply run the app on a Docker-enabled PC via:

docker run -p 80:80 ghcr.io/digitaltwinconsortium/aas-repository:masterv3

And then point your browser to http://localhost.

Note: For a quickstart, the AAS Repository is integrated in the [Manufacturing Ontologies](https://github.com/digitaltwinconsortium/ManufacturingOntologies) reference solution.


# Required Environment variables

1. ServicePassword: Password to access the service via basic authentication header


# Optional Environment variables

1. HostingPlatform: The hosting platform of the repository. Current options are `Azure` (the default is to run it locally)
1. BlobStorageConnectionString: The connection string to the Azure Blob storage when the hosting platform is set to Azure
1. UACLUsername: OPC Foundation UA Cloud Library username
1. UACLPassword: OPC Foundation UA Cloud Library password
1. ADX_HOST: Azure Data Explorer host name
1. ADX_DB: Azure Data Explorer database name
1. AAD_TENANT: Azure Active Directory tenant ID of your Azure subscription (GUID)
1. AAD_APPLICATION_ID: Azure Active Directory application ID (GUID, set this up via an app registration in the Azure Portal)
1. AAD_APPLICATION_KEY: Azure Active Directory application key (with data owner access to ADX cluster and database, create a secret within your AAD app registration)
1. DATA_QUERY_INTERVAL: The query interval for the connected database
1. WATTTIME_USER: WattTime service username
1. WATTTIME_PASSWORD: WattTime service password
1. WATTTIME_LATITUDE: WattTime location to query
1. WATTTIME_LONGITUDE: WattTime location to query
1. CALCULATE_PCF: Set to "1" to enable PCF calculation for the DTC's Manufacturing Ontologies Reference Solution
1. CALCULATE_PCF_SMIP: Set to "1" to enable PCF calculation for CESMII's Smart Manufacturing Innovation Platform
1. CARBON_REPORTING: Set to "1" to enable carbon reporting
1. OPCUA_REPORTING: Set to "1" to enable OPC UA PubSub operational data reporting
1. USE_JSON_SERIALIZATION: Set "1" to enable saving AAS spec in JSON format (within AASX file)
1. SMIP_GRAPHQL_ENDPOINT_URL: The CESMII SMIP GarphQL Endpoint URL, e.g. "https://demo.cesmii.net/graphql"
1. SMIP_USERNAME: Your CESMII SMIP username
1. SMIP_CLIENT_ID: The CESMII SMIP client ID of this AAS Repo
1. SMIP_CLIENT_PASSWORD: The CESMII SMIP client password of this AAS Repo
1. SMIP_CLIENT_ROLE: The CESMII SMIP client role of this AAS Repo
1. SMIP_BEARER_TOKEN: The current CESMII SMIP bearer token. If blank, the other SMIP env variables are used to request a new token
