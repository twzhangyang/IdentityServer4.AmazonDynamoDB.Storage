using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using IdentityServer4.AmazonDynamoDB.Storage.Entities;
using IdentityServer4.AmazonDynamoDB.Storage.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.AmazonDynamoDB.Storage.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly DynamoDBContext _dynamoDbContext;

        public PersistedGrantStore(DynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public Task StoreAsync(PersistedGrant token)
        {
            return _dynamoDbContext.SaveAsync(token.ToEntity());
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = await _dynamoDbContext.QueryAsync<PersistedGrantEntity>(key)
                .GetRemainingAsync();

            return persistedGrant.FirstOrDefault()?.ToModel();
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var result = new List<PersistedGrant>();
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("SubjectId", ScanOperator.Equal, subjectId),
            };

            var batch = _dynamoDbContext.ScanAsync<PersistedGrantEntity>(conditions);
            while (!batch.IsDone)
            {
                var dataset = await batch.GetNextSetAsync();

                if (dataset.Any())
                {
                    result.AddRange(dataset.Select(x => x.ToModel()));
                }
            }

            return result;
        }

        public async Task RemoveAsync(string key)
        {
            var persistedGrant = await _dynamoDbContext.QueryAsync<PersistedGrantEntity>(key)
                .GetRemainingAsync();

            if (persistedGrant.Any())
            {
                await _dynamoDbContext.DeleteAsync(persistedGrant.First());
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("SubjectId", ScanOperator.Equal, subjectId),
                new ScanCondition("ClientId", ScanOperator.Equal, clientId)
            };

            var batch = _dynamoDbContext.ScanAsync<PersistedGrantEntity>(conditions);
            while (!batch.IsDone)
            {
                var dataset = await batch.GetNextSetAsync();

                if (dataset.Any())
                {
                    dataset.ForEach(async item => await _dynamoDbContext.DeleteAsync(item));
                }
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("SubjectId", ScanOperator.Equal, subjectId),
                new ScanCondition("ClientId", ScanOperator.Equal, clientId),
                new ScanCondition("Type", ScanOperator.Equal, type)
            };

            var batch = _dynamoDbContext.ScanAsync<PersistedGrantEntity>(conditions);
            while (!batch.IsDone)
            {
                var dataset = await batch.GetNextSetAsync();

                if (dataset.Any())
                {
                    dataset.ForEach(async item => await _dynamoDbContext.DeleteAsync(item));
                }
            }
        }
    }
}