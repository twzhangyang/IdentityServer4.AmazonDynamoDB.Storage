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

### Describe table 
```
aws2 dynamodb describe-table --table-name local-PersistedGrant --endpoint-url http://localhost:8000
```

```
{
  "Table": {
    "AttributeDefinitions": [
      {
        "AttributeName": "Key",
        "AttributeType": "S"
      },
      {
        "AttributeName": "SubjectId",
        "AttributeType": "S"
      },
      {
        "AttributeName": "CreationTime",
        "AttributeType": "S"
      }
    ],
    "TableName": "local-PersistedGrant",
    "KeySchema": [
      {
        "AttributeName": "Key",
        "KeyType": "HASH"
      }
    ],
    "TableStatus": "ACTIVE",
    "CreationDateTime": "2019-11-25T18:53:39.536000+07:00",
    "ProvisionedThroughput": {
      "LastIncreaseDateTime": "1970-01-01T07:00:00+07:00",
      "LastDecreaseDateTime": "1970-01-01T07:00:00+07:00",
      "NumberOfDecreasesToday": 0,
      "ReadCapacityUnits": 10,
      "WriteCapacityUnits": 10
    },
    "TableSizeBytes": 1400,
    "ItemCount": 10,
    "TableArn": "arn:aws:dynamodb:ddblocal:000000000000:table/local-PersistedGrant",
    "BillingModeSummary": {
      "BillingMode": "PROVISIONED",
      "LastUpdateToPayPerRequestDateTime": "1970-01-01T07:00:00+07:00"
    },
    "GlobalSecondaryIndexes": [
      {
        "IndexName": "SubjectIdAndCreationTimeIndex",
        "KeySchema": [
          {
            "AttributeName": "SubjectId",
            "KeyType": "HASH"
          },
          {
            "AttributeName": "CreationTime",
            "KeyType": "RANGE"
          }
        ],
        "Projection": {
          "ProjectionType": "KEYS_ONLY"
        },
        "IndexStatus": "ACTIVE",
        "ProvisionedThroughput": {
          "ReadCapacityUnits": 5,
          "WriteCapacityUnits": 5
        },
        "IndexSizeBytes": 1400,
        "ItemCount": 10,
        "IndexArn": "arn:aws:dynamodb:ddblocal:000000000000:table/local-PersistedGrant/index/SubjectIdAndCreationTimeIndex"
      }
    ]
  }
}
```


### Query table in aws cli
```
aws2 dynamodb list-tables --endpoint-url http://localhost:8000
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
