# IdentityServer4.AmazonDynamoDB.Storage
An AmazonDynamoDB-based implementation is provided for the operational data extensibility points in IdentityServer.
Configuration data store is in progress.

### Setup DanymoDB on local
```
docker run -p 8000:8000 dwmkerr/dynamodb -sharedDb
```

### Create table in shell
```
http://localhost:8000/shell
```

### Add config in appsettings.json
```
  "AWS": {
    "Region": "[region]"
  },
  "DynamoDB": {
    "Environment": "local",
    "ServiceURL": "http://localhost:8000"
  }

```

Please update region and set ServiceURL to null when using DynamoDB on AWS

### Configure
```
 var builder = services.AddIdentityServer()
                .AddOperationalDynamoDBStore(Configuration,
                    options =>
                    {
                        var section = Configuration.GetSection("DynamoDB");
                        section.Bind(options);
                    });
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

```
