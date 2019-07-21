using accountapi.Actors;
using Akka.Actor;
using Akka.Routing;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Repository
{
    public static class CachePrefix
    {
        public static string prefix_auth = "auth_";
    }

    public class LocalCacheRepository
    {
        private readonly MemoryCacheEntryOptions cachePolycy_AuthToken;
        private readonly IMemoryCache _memoryCache;
        private readonly ActorSystem _actorSystem;
        private readonly IActorRef _clusterActor;


        public LocalCacheRepository(IMemoryCache memoryCache, ActorSystem actorSystem)
        {
            _memoryCache = memoryCache;
            _actorSystem = actorSystem;
            cachePolycy_AuthToken = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));


            var acountActor = actorSystem.ActorOf(Props.Create(() => new AccountActor(memoryCache)), "accountpool");

            _clusterActor = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "scatter");

        }

        public void SetAuthToken(string authtoken,string someInfo )
        {
            _memoryCache.Set(CachePrefix.prefix_auth + authtoken, someInfo, cachePolycy_AuthToken);
        }

        public string GetLoginInfoByToken(string authtoken)
        {
            string loginInfo;
            if (_memoryCache.TryGetValue(CachePrefix.prefix_auth + authtoken, out loginInfo))
            {
                return loginInfo;
            }
            else
            {
                try
                {
                    loginInfo = _clusterActor.Ask(new AskToken(authtoken), TimeSpan.FromSeconds(3)).Result as string;
                    SetAuthToken(authtoken, loginInfo);
                    return loginInfo;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed - Access Token");
                }
                throw new Exception("Failed - Access Token");
            }
        }        
    }
}
