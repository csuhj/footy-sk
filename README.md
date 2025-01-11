# footy-sk
A [Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/overview/) project with football data

### Initial Setup

In order to set up the Azure resources needed to run the project, set up and deploy an appropriate OpenAI model in Azure:

* Create a Azure OpenAI resource in Azure via the Azure Portal
* Go to the Azure AI Foundary, linked to from the portal
* Deploy your chosen model for chat completions (for example `gpt-4o-mini`) and text embeddings (for example `text-embedding-ada-002`)

You will also need to use the `vec0` SQlite extension to support Vector Stores held in Sqlite.  To do this:
* Go to https://github.com/asg017/sqlite-vec/releases (for appropriate architecture) and copy the vec0 DLL/dylib into the FootySk.Console project's executable dir
* On MacOS, this will fail first time with a message about not being trusted by Apple.  Go to System Settings -> Privacy & Security, scroll to bottom and allow vec0.dylib anyway.  This should then work again.  See https://support.apple.com/en-gb/guide/mac-help/mh40616/mac


### Configuration and running

To then run the project:

* With the details from above, set up a `credentials.json` file in the same directory as the `FootySk.sln` file for this project.  For example:
```
{
    "chatDeploymentName": "gpt-4o-mini",
    "embeddingDeploymentName": "text-embedding-ada-002",
    "endpoint": "https://<your_resource_name>.openai.azure.com/",
    "apiKey": "<your_api_key>"
}
```
* Run the project from the same directory:
```
dotnet run --project ./src/FootySk.Console/FootySk.Console.csproj
```