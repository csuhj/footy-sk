# footy-sk
A Semantic Kernel project with football data

### Initial Setup

In order to set up the Azure resources needed to run the project, set up and deploy an appropriate OpenAI model in Azure:

* Create a Azure OpenAI resource in Azure via the Azure Portal
* Go to the Azure AI Foundary, linked to from the portal
* Deploy your chosen model (for example `gpt-4o-mini`).

### Configuration and running

To then run the project:

* With the details from above, set up a `credentials.json` file in the same directory as the `FootySk.sln` file for this project.  For example:
```
{
    "deploymentName": "gpt-4o-mini",
    "endpoint": "https://<your_resource_name>.openai.azure.com/",
    "apiKey": "<your_api_key>"
}
```
* Run the project from the same directory:
```
dotnet run --project ./src/FootySk.Console/FootySk.Console.csproj
```