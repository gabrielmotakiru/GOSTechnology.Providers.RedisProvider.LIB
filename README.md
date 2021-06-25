# GOSTechnology.Providers.RedisProvider.LIB
### Redis connection management provider.
###### *Version Note: 1.0.0-preview3.*

[![N|Solid](https://cdn.iconscout.com/icon/free/png-256/redis-83994.png)](https://github.com/gabrielmotakiru/GOSTechnology.Providers.RedisProvider.LIB)

---

#### 1 - CONFIGURING LIBRARY:
- **Add system environment variable (linux or windows):**
> RedisConnectionString = host;port;password;timeout;timecache;

- **Add using namespace for library:**
> *using GOSTechnology.Providers.RedisProvider.LIB;*

- **Add dependency injection (ConfigureServices - Startup.cs):**
> *services.AddRedisProvider();*

- **Add dependency injection with scope (ConfigureServices - Startup.cs):**
> *// SINGLETON or SCOPED or TRANSIENT.*
> 
> *services.AddRedisProvider(TypeInjection.SCOPED);*

---

#### 2 - USING LIBRARY:
- **Persist json (string value in Redis validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
> 
> *var obj = new { Message = "ShouldSuccessPersist" };*
> 
> *this._redisProvider.Persist(key, obj);*

- **Get json (string value in Redis validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
> 
> *Object result = this._redisProvider.Get\<Object\>(key);*

- **Remove json (string value validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
>
> *this._redisProvider.Remove(key);*

---

###### *Note: **Persist** and **Get** methods uses conversion **Newtonsoft.Json** for **transform object** in string json, **validated as json** in Redis server.*
###### *Note: **Persist** and **Remove** methods is of type command flag **FireAndForget** (obviously using Redis without response prioritization).*
###### *Note: Environment variable **RedisConnectionString** require **timeout** in **miliseconds** (obviously by response time level).*
###### *Note: Environment variable **RedisConnectionString** require **timecache** in **secods** (obviously by the level of persistence time).*