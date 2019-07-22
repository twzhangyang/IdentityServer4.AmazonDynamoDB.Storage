using System;
using System.Linq;
using FluentAssertions;
using IdentityServer4.AmazonDynamoDB.Storage.Migration;
using IdentityServer4.AmazonDynamoDB.Storage.Options;
using IdentityServer4.AmazonDynamoDB.Storage.Tests.TestOrder;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace IdentityServer4.AmazonDynamoDB.Storage.Tests
{
    [Collection("persisted grant")]
    public class PersistedGrantStoreTests : TestBase
    {
        [Fact, TestPriority(1)]
        public async void ShouldCreateTable()
        {
            var tableBuilder = ServiceProvider.GetService<PersistedGrantTableBuilder>();

            var options = ServiceProvider.GetService<IOptions<DynamoDBOptions>>();
            await tableBuilder.CreateTable(options.Value);
        }

        [Fact, TestPriority(2)]

        public async void ShouldStoreToken()
        {
            //Arrange
            var token = new PersistedGrant
            {
                Key = "key",
                ClientId = "clientId",
                CreationTime = DateTime.Now,
                Expiration = DateTime.Now,
                SubjectId = "subjectId",
                Data = "data",
                Type = "type"
            };

            //Act
            await PersistedGrantStore.StoreAsync(token);
        }
        
        [Fact, TestPriority(3)]
        public async void ShouldGetStoreToken()
        {
            //Arrange
            var key = "key";
            
            //Act
            var token = await PersistedGrantStore.GetAsync(key);
            
            //Assert
            token.Should().NotBeNull();
        }
        
        [Fact, TestPriority(4)]
        public async void GetAllBySubjectId()
        {
            //Arrange
            var subjectId = "subjectId";
            
            //Act
            var tokens = await PersistedGrantStore.GetAllAsync(subjectId);
            
            //Assert
            tokens.Should().HaveCountGreaterThan(0);
        }
        
        [Fact]
        public async void ShouldRemoveByKey()
        {
            //Arrange
            var key = "key";
            
            //Act
            await PersistedGrantStore.RemoveAsync(key);
            
        }
    }
}