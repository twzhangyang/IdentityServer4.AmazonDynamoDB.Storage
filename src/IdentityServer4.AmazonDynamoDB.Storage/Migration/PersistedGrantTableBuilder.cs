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
            var tables = await _amazonDynamoDbClient.ListTablesAsync();

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
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "CreationTime",
                            AttributeType = "S"
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "TTL",
                            AttributeType = "N"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Key",
                            KeyType = "HASH"
                        }
                    },
                    GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>()
                    {
                        new GlobalSecondaryIndex()
                        {
                            IndexName = "SubjectIdAndCreationTimeIndex",
                            KeySchema = new List<KeySchemaElement>
                            {
                                new KeySchemaElement
                                {
                                    AttributeName = "SubjectId",
                                    KeyType = "HASH"
                                },
                                new KeySchemaElement
                                {
                                    AttributeName = "CreationTime",
                                    KeyType = "RANGE"
                                }
                            },
                            Projection = new Projection
                            {
                                ProjectionType = ProjectionType.KEYS_ONLY
                            },
                            ProvisionedThroughput = new ProvisionedThroughput
                            {
                                ReadCapacityUnits = 5,
                                WriteCapacityUnits = 5
                            }
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 10,
                        WriteCapacityUnits = 10
                    },
                };

                await _amazonDynamoDbClient.CreateTableAsync(createRequest);
            }
        }
    }
}