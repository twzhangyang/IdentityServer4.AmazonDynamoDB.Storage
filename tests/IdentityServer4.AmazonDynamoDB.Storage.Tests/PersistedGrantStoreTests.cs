using System;
using System.Linq;
using Amazon.Util;
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
    [TestCaseOrderer("IdentityServer4.AmazonDynamoDB.Storage.Tests.TestOrder.PriorityOrderer",
        "IdentityServer4.AmazonDynamoDB.Storage.Tests")]
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
            for (int i = 0; i < 10; i++)
            {
                var token = new PersistedGrant
                {
                    Key = "key" + i,
                    ClientId = "clientId" + i,
                    CreationTime = DateTime.Now,
                    Expiration = DateTime.Now.AddDays(1),
                    SubjectId = "subjectId" + i,
                    Data = "data" + i,
                    Type = "type" + i,
                };

                await PersistedGrantStore.StoreAsync(token);
            }
        }

        [Fact, TestPriority(3)]
        public async void ShouldGetStoreToken()
        {
            //Arrange
            var key = "key";

            //Act
            var result = await PersistedGrantStore.GetAsync(key);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact, TestPriority(4)]
        public async void GetAllBySubjectId()
        {
            //Arrange
            var subjectId = "subjectId5";

            //Act
            var result = await PersistedGrantStore.GetAllAsync(subjectId);

            //Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact, TestPriority(5)]
        public async void ShouldRemoveByKey()
        {
            //Arrange
            var key = "key";

            //Act
            await PersistedGrantStore.RemoveAsync(key);
        }

        [Fact, TestPriority(6)]
        public async void ShouldRemoveBySubjectId()
        {
            //Arrange
            var subjectId = "subjectId6";

            //Act
            await PersistedGrantStore.RemoveAllAsync(subjectId, string.Empty);
        }
        
        [Fact]
        public void Test()
        {
            var ttl = "1574822917";
            var date = AWSSDKUtils.ConvertFromUnixEpochSeconds(int.Parse(ttl)).ToUniversalTime();

            date.Should().Equals(DateTime.Parse("2019-11-27T02:48:37.172Z"));
        }
    }
}