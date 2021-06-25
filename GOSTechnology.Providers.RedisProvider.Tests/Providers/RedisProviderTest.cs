using FluentAssertions;
using GOSTechnology.Providers.RedisProvider.LIB;
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
        #region "CONFIGURATIONS"

        /// <summary>
        /// _redisProvider.
        /// </summary>
        private IRedisProvider _redisProvider;

        /// <summary>
        /// _architectureProvider.
        /// </summary>
        private IArchitectureProvider _architectureProvider;

        /// <summary>
        /// _loggerRedisProvider.
        /// </summary>
        private Mock<ILogger<LIB.RedisProvider>> _loggerRedisProvider;

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
            this._loggerRedisProvider = new Mock<ILogger<LIB.RedisProvider>>();
            this._loggerArchitectureProvider = new Mock<ILogger<ArchitectureProvider>>();
            this._architectureProvider = new ArchitectureProvider(this._loggerArchitectureProvider.Object);
            this._redisProvider = new LIB.RedisProvider(this._loggerRedisProvider.Object, this._architectureProvider);
        }

        /// <summary>
        /// ValidateLogger.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        private void ValidateLogger(String message, LogLevel level)
        {
            this._loggerRedisProvider.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == level),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        #endregion

        #region "PERSIST OBJECT TESTS"

        /// <summary>
        /// ShouldFailPersistObjectByNullKeyObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_NULL)]
        public void ShouldFailPersistObjectByNullKeyObject(String key)
        {
            this._redisProvider.Persist(key, null);
            Object result = this._redisProvider.Get<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_REQUIRED_KEY_OBJECT, LogLevel.Warning);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldSuccessPersistObjectByKeyObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_1)]
        [TestCase(ConstantsRedisProvider.KEY_2)]
        [TestCase(ConstantsRedisProvider.KEY_3)]
        public void ShouldSuccessPersistObjectByKeyObject(String key)
        {
            this._redisProvider.Persist(key, new { Message = ConstantsRedisProvider.MESSAGE_SUCCESS_PERSIST });
            Object result = this._redisProvider.Get<Object>(key);
            this._redisProvider.Remove(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_SUCCESS_PERSIST_OBJECT, LogLevel.Information);
            result.Should().NotBeNull();
        }

        #endregion

        #region "PERSIST ASYNC OBJECT TESTS"

        /// <summary>
        /// ShouldFailPersistAsyncObjectByNullKeyObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_NULL)]
        public async Task ShouldFailPersistAsyncObjectByNullKeyObject(String key)
        {
            await this._redisProvider.PersistAsync(key, null);
            Object result = await this._redisProvider.GetAsync<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_REQUIRED_KEY_OBJECT, LogLevel.Warning);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldSuccessPersistAsyncObjectByKeyObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_1)]
        [TestCase(ConstantsRedisProvider.KEY_2)]
        [TestCase(ConstantsRedisProvider.KEY_3)]
        public async Task ShouldSuccessPersistAsyncObjectByKeyObject(String key)
        {
            await this._redisProvider.PersistAsync(key, new { Message = ConstantsRedisProvider.MESSAGE_SUCCESS_PERSIST });
            Object result = await this._redisProvider.GetAsync<Object>(key);
            await this._redisProvider.RemoveAsync(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_SUCCESS_PERSIST_OBJECT, LogLevel.Information);
            result.Should().NotBeNull();
        }

        #endregion

        #region "REMOVE OBJECT TESTS"

        /// <summary>
        /// ShouldFailRemoveObjectByNullKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_NULL)]
        public void ShouldFailRemoveObjectByNullKey(String key)
        {
            this._redisProvider.Remove(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_REQUIRED_KEY, LogLevel.Warning);
        }

        /// <summary>
        /// ShouldFSuccessRemoveObjectByKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_1)]
        [TestCase(ConstantsRedisProvider.KEY_2)]
        [TestCase(ConstantsRedisProvider.KEY_3)]
        public void ShouldFSuccessRemoveObjectByKey(String key)
        {
            this._redisProvider.Persist(key, new { Message = ConstantsRedisProvider.MESSAGE_SUCCESS_REMOVE });
            Object result = this._redisProvider.Get<Object>(key);
            result.Should().NotBeNull();
            this._redisProvider.Remove(key);
            Object result2 = this._redisProvider.Get<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_SUCCESS_PREMOVE_OBJECT, LogLevel.Information);
            result2.Should().BeNull();
        }

        #endregion

        #region "REMOVE ASYNC OBJECT TESTS"

        /// <summary>
        /// ShouldFailRemoveAsyncObjectByNullKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_NULL)]
        public async Task ShouldFailRemoveAsyncObjectByNullKey(String key)
        {
            await this._redisProvider.RemoveAsync(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_REQUIRED_KEY, LogLevel.Warning);
        }

        /// <summary>
        /// ShouldFSuccessRemoveAsyncObjectByKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_1)]
        [TestCase(ConstantsRedisProvider.KEY_2)]
        [TestCase(ConstantsRedisProvider.KEY_3)]
        public async Task ShouldFSuccessRemoveAsyncObjectByKey(String key)
        {
            await this._redisProvider.PersistAsync(key, new { Message = ConstantsRedisProvider.MESSAGE_SUCCESS_REMOVE });
            Object result = await this._redisProvider.GetAsync<Object>(key);
            result.Should().NotBeNull();
            await this._redisProvider.RemoveAsync(key);
            Object result2 = await this._redisProvider.GetAsync<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_SUCCESS_PREMOVE_OBJECT, LogLevel.Information);
            result2.Should().BeNull();
        }

        #endregion

        #region "GET OBJECT TESTS"

        /// <summary>
        /// ShouldFailGetObjectByNullKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_NULL)]
        public void ShouldFailGetObjectByNullKey(String key)
        {
            Object result = this._redisProvider.Get<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_REQUIRED_KEY, LogLevel.Warning);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldFailGetObjectByKeyNotFound.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_EMPTY)]
        public void ShouldFailGetObjectByKeyNotFound(String key)
        {
            Object result = this._redisProvider.Get<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_NOT_FOUND_OBJECT, LogLevel.Information);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldSuccessGetObjectbyKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_1)]
        [TestCase(ConstantsRedisProvider.KEY_2)]
        [TestCase(ConstantsRedisProvider.KEY_3)]
        public void ShouldSuccessGetObjectbyKey(String key)
        {
            this._redisProvider.Persist(key, new { Message = ConstantsRedisProvider.MESSAGE_SUCCESS_GET });
            Object result = this._redisProvider.Get<Object>(key);
            this._redisProvider.Remove(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_SUCCESS_GET_OBJECT, LogLevel.Information);
            result.Should().NotBeNull();
        }

        #endregion

        #region "GET ASYNC OBJECT TESTS"

        /// <summary>
        /// ShouldFailGetAsyncObjectByNullKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_NULL)]
        public async Task ShouldFailGetAsyncObjectByNullKey(String key)
        {
            Object result = await this._redisProvider.GetAsync<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_REQUIRED_KEY, LogLevel.Warning);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldFailGetAsyncObjectByKeyNotFound.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_EMPTY)]
        public async Task ShouldFailGetAsyncObjectByKeyNotFound(String key)
        {
            Object result = await this._redisProvider.GetAsync<Object>(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_NOT_FOUND_OBJECT, LogLevel.Information);
            result.Should().BeNull();
        }

        /// <summary>
        /// ShouldSuccessGetAsyncObjectbyKey.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Test]
        [TestCase(ConstantsRedisProvider.KEY_1)]
        [TestCase(ConstantsRedisProvider.KEY_2)]
        [TestCase(ConstantsRedisProvider.KEY_3)]
        public async Task ShouldSuccessGetAsyncObjectbyKey(String key)
        {
            await this._redisProvider.PersistAsync(key, new { Message = ConstantsRedisProvider.MESSAGE_SUCCESS_GET });
            Object result = await this._redisProvider.GetAsync<Object>(key);
            await this._redisProvider.RemoveAsync(key);
            this.ValidateLogger(ConstantsRedisProvider.MSG_SUCCESS_GET_OBJECT, LogLevel.Information);
            result.Should().NotBeNull();
        }

        #endregion
    }
}
