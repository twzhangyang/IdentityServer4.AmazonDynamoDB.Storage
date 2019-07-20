using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using IdentityServer4.AmazonDynamoDB.Storage.Options;

namespace IdentityServer4.AmazonDynamoDB.Storage.Migration
{
    public class PersistedGrantTableBuilder
    {
        private readonly IAmazonDynamoDB _amazonDynamoDbClient;

        public PersistedGrantTableBuilder(IAmazonDynamoDB amazonDynamoDbClient)
        {
            _amazonDynamoDbClient = amazonDynamoDbClient;
        }
        
        public async Task CreateTable(DynamoDBOptions options)
        {
            var tableName = options.PersistedGrantTableName;
            var tables = await _amazonDynamoDbClient.ListTablesAsync(tableName);

            if (!tables.TableNames.Contains(tableName))
            {
                var createRequest = new CreateTableRequest
                {
                    TableName = tableName,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Key",
                            AttributeType = "S"
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "SubjectId",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Key",
                            KeyType = "HASH"
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "SubjectId",
                            KeyType = "RANGE"
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 2,
                        WriteCapacityUnits = 2
                    }
                };

                await _amazonDynamoDbClient.CreateTableAsync(createRequest);
            }
        }
    }
}