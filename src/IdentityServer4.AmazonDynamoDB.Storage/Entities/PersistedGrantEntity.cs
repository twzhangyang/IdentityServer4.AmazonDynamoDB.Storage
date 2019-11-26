using System;
using Amazon.DynamoDBv2.DataModel;

namespace IdentityServer4.AmazonDynamoDB.Storage.Entities
{
    [DynamoDBTable("PersistedGrant")]
    public class PersistedGrantEntity
    {
        [DynamoDBHashKey]
        public string Key { get; set; }

        [DynamoDBProperty]
        public string Type { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string SubjectId { get; set; }

        [DynamoDBProperty]
        public string ClientId { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey]
        public DateTime CreationTime { get; set; }
     
        [DynamoDBProperty]
        public DateTime? Expiration { get; set; }

        [DynamoDBProperty]
        public string Data { get; set; }
        
        [DynamoDBProperty]
        public long TTL { get; set; }
    }
}