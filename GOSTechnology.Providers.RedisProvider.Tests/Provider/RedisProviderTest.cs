using FluentAssertions;
using GOSTechnology.Providers.RedisProvider.LIB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace GOSTechnology.Providers.RedisProvider.Tests
{
    /// <summary>
    /// RedisProviderTest.
    /// </summary>
    [TestFixture]
    public class RedisProviderTest
    {
        /// <summary>
        /// _redisProvider.
        /// </summary>
        private IRedisProvider _redisProvider;

        /// <summary>
        /// SetUp.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            IConfiguration configuration = InfrastructureExtension.GetConfiguration();
            Mock<ILogger<LIB.RedisProvider>> logger = new Mock<ILogger<LIB.RedisProvider>>();
            _redisProvider = new LIB.RedisProvider(configuration, logger.Object);
        }

        /// <summary>
        /// ShouldFailGetKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase("00000000-0000-0000-0000-000000000000")]
        public void ShouldFailGetKey(String key)
        {
            Object result = this._redisProvider.Get<Object>(key);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldSuccessGetKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase("861708da-924e-4d07-ac83-f6c6d3ff28b2")]
        [TestCase("feb0ffee-75e4-4ecd-9861-bdd92371939a")]
        [TestCase("b02ca313-3afe-4321-b12d-076d586b1404")]
        public void ShouldSuccessGetKey(String key)
        {
            this._redisProvider.Persist(key, new { Message = "ShouldSuccessGetKey" });
            Object result = this._redisProvider.Get<Object>(key);
            result.Should().NotBeNull();
        }

        /// <summary>
        /// ShouldFailPersist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase("ee155bba-13f0-4a1f-8820-1a5a8f0c3bae")]
        [TestCase("26d98e0e-1e35-4e7f-a14c-33a11252eb52")]
        [TestCase("daa40537-ddc3-4d7a-9366-292e2a19b157")]
        public void ShouldFailPersist(String key)
        {
            this._redisProvider.Persist(key, null);
            Object result = this._redisProvider.Get<Object>(key);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldSuccessPersist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase("6b553d49-4704-4218-b792-923176a39269")]
        [TestCase("392f665f-a026-459e-ba9b-cb85e20fbe5b")]
        [TestCase("b952305e-1565-443b-929c-1f22e2cf04e8")]
        public void ShouldSuccessPersist(String key)
        {
            this._redisProvider.Persist(key, new { Message = "ShouldSuccessPersist" });
            Object result = this._redisProvider.Get<Object>(key);
            result.Should().NotBeNull();
        }

        /// <summary>
        /// ShouldSuccessRemove.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase("23109437-2924-433f-91cc-a11a6a774d00")]
        [TestCase("c9790a73-2cbf-427b-850e-3a6d5beec54b")]
        [TestCase("9b1f14b9-3409-40e4-a428-bb6cbee52f9c")]
        public void ShouldSuccessRemove(String key)
        {
            this._redisProvider.Persist(key, new { Message = "ShouldSuccessRemove" });
            Object result = this._redisProvider.Get<Object>(key);
            result.Should().NotBeNull();
            this._redisProvider.Remove(key);
            Object result2 = this._redisProvider.Get<Object>(key);
            result2.Should().BeNull();
        }
    }
}
