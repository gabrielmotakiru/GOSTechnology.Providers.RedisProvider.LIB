# GOSTechnology.Providers.RedisProvider.LIB
### Redis connection management provider.
###### *Version Note: 1.0.0-preview5.*

[![N|Solid](https://cdn.iconscout.com/icon/free/png-256/redis-83994.png)](https://github.com/gabrielmotakiru/GOSTechnology.Providers.RedisProvider.LIB)

---

#### 1 - CONFIGURING LIBRARY:
- **Add system environment variable (linux or windows):**
> RedisConnectionString = host=x.x.x.x;port=6379;password=mypassword;timeout=500;timecache=300;

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

- **PersistAsync json (string value in Redis validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
> 
> *var obj = new { Message = "ShouldSuccessPersist" };*
> 
> *await this._redisProvider.PersistAsync(key, obj);*

- **Remove json (string value validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
>
> *this._redisProvider.Remove(key);*

- **RemoveAsync json (string value validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
>
> *await this._redisProvider.RemoveAsync(key);*

- **Get json (string value in Redis validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
> 
> *Object result = this._redisProvider.Get\<Object\>(key);*

- **GetAsync json (string value in Redis validated as json) in Redis server:**
> *var key = Guid.NewGuid().ToString();*
> 
> *Object result = await this._redisProvider.GetAsync\<Object\>(key);*

---

###### **Note**: Environment variable **RedisConnectionString** require **timeout** in **miliseconds** (obviously by response time level).
###### **Note**: Environment variable **RedisConnectionString** require **timecache** in **secods** (obviously by the level of persistence time).
###### **Note**: **Persists** (synchronous or asynchronous) and **Gets** (synchronous or asynchronous) methods uses conversion **Newtonsoft.Json** for **transform object** in string json, **validated as json** in Redis server.
###### **Note**: **Persists** (synchronous or asynchronous) and **Removes** (synchronous or asynchronous) methods is of type command flag **FireAndForget** (obviously using Redis without response prioritization).
###### **Note**: The methods **Persist**, **PersistAsync**, **Remove** and **RemoveAsync** provide the **FireAndForget** command flag of Redis configured by **default**, but it is possible to customize it according to **your needs**, passing it as a parameter in each method usage (**last parameter**).
###### **Note**: Object can be any template class passed as a type for object.