
# Introduction

This repository shows how to use Thor to enable Tracing on a HotChocolate Api.

In our example, we will generate resolver errors in a HotChocolate Api. 
HotChocolate will automatically send events about these errors to an EventHub. 

We will read these events with a HostedService, which will forward them to a HotChocolate subscription.
This is just for convenience, you could instead write them into the console or make another App that's listening to the EventHub and stores the received events in a database.

# Getting Started

The Host app is `Host.csproj`.

You'll need an Azure Blob Storage and an EventHub.

For the blob Storage, you can use Azurite (see the [official Azurite doc](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) as well as the [Azure Cloud Storage connectionString override mechanism](https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string#connect-to-the-emulator-account-using-a-shortcut))
```
docker pull mcr.microsoft.com/azure-storage/azurite
docker run -p 10000:10000 -p 10001:10001 mcr.microsoft.com/azure-storage/azurite
```
Or you can use an azure resource. 

Both resources' connectionStrings are to be stored in the appsettings.json File.

# Testing the tracing feature

Our HotChocolate api automatically forwards all exceptions and resolver issues to the configured EventBus. 
This is done with just two lines in the Startup class (`AddTracing()` and `AddThorLogging()`).

We will use a HostedService to listen to new events from the EventHub. 
This hosted service will forward the events to a HotChocolate subscription `onEvent` that we can consume via a compatible GraphQL Client.

## The subscription

A BananaCakepop GraphQL Client is available on the endpoint (http://localhost:XXXX).

Open your BananaCakepop GraphQL client and run the following request: 
```
subscription sub {
  onEvent(eventType: "trace"){
    id
    activityId
    applicationId
    level
    message
    payloads{
      statusCode
      statusText
    }
    providerName
    taskId
  }
}
```
Each time a new event is received from the EventHub, you'll see it here.


Now open another GraphQL Client and run the following Query:
```
query f1 {
  nonNullableFieldWithNullResult
}
```

You'll see events appear in the subscription:
![requests and traces](https://github.com/gmiserez/HotChocolate_ThorLogging/blob/master/images/traces.gif?raw=true)


