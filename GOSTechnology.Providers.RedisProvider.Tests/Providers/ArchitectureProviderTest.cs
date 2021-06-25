using FluentAssertions;
using GOSTechnology.Providers.RedisProvider.LIB;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;
using System;

namespace GOSTechnology.Providers.RedisProvider.Tests
{
    /// <summary>
    /// ArchitectureProviderTest.
    /// </summary>
    [TestFixture]
    public class ArchitectureProviderTest
    {
        #region "CONFIGURATIONS"

        /// <summary>
        /// _architectureProvider.
        /// </summary>
        private IArchitectureProvider _architectureProvider;

        /// <summary>
        /// _loggerArchitectureProvider.
        /// </summary>
        private Mock<ILogger<ArchitectureProvider>> _loggerArchitectureProvider;

        /// <summary>
        /// SetUp.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this._loggerArchitectureProvider = new Mock<ILogger<ArchitectureProvider>>();
            this._architectureProvider = new ArchitectureProvider(this._loggerArchitectureProvider.Object);
        }

        #endregion

        #region "ARCHITECTURE TESTS"

        /// <summary>
        /// ShouldSuccessGetTimeCache.
        /// </summary>
        /// <returns></returns>
        [Test]
        public void ShouldSuccessGetTimeCache()
        {
            Int32 result = this._architectureProvider.GetTimeCache();
            result.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// ShouldSuccessGetConfigurationOptions.
        /// </summary>
        [Test]
        public void ShouldSuccessGetConfigurationOptions()
        {
            ConfigurationOptions result = this._architectureProvider.GetConfigurationOptions();
            result.Should().NotBeNull();
        }

        #endregion
    }
}
