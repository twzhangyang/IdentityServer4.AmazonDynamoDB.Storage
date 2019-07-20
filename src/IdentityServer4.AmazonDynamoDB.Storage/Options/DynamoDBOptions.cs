using System;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer4.AmazonDynamoDB.Storage.Options
{
    public class DynamoDBOptions
    {
        public string ServiceURL { get; set; }

        public string Environment { get; set; }

        public string TablePrefix => $"{Environment}-";
        
        public string PersistedGrantTableName => $"{Environment}-PersistedGrant";
    }
}