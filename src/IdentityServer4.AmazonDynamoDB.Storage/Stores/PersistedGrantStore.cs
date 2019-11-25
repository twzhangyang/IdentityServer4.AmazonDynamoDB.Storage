using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using IdentityServer4.AmazonDynamoDB.Storage.Entities;
using IdentityServer4.AmazonDynamoDB.Storage.Mappers;
using IdentityServer4.Extensions;
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
            var persistedGrant = await _dynamoDbContext.LoadAsync<PersistedGrantEntity>(key);

            return persistedGrant.ToModel();
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
           var  result = await _dynamoDbContext.QueryAsync<PersistedGrant>(subjectId, 
                   new DynamoDBOperationConfig {IndexName = "SubjectIdAndCreationTimeIndex"})
                .GetRemainingAsync();

           return result;
        }

        public async Task RemoveAsync(string key)
        {
            var persistedGrant = await _dynamoDbContext.LoadAsync<PersistedGrantEntity>(key);
            await _dynamoDbContext.DeleteAsync(persistedGrant);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var results = await GetAllAsync(subjectId);

            if (results.IsNullOrEmpty())
            {
                throw new NoneGrantTypeRecordFoundException();
            }

            foreach (var result in results)
            {
                await RemoveAsync(result.Key);
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            await RemoveAllAsync(subjectId, clientId);
        }
    }
}