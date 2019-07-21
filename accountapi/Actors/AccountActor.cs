using accountapi.Repository;
using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace accountapi.Actors
{
    public delegate IActorRef AccountActorProvider();

    public class ActorMessage
    {
    }

    public class AskToken : ActorMessage
    {
        public AskToken(string _authtoken)
        {
            authtoken = _authtoken;
        }
        public string authtoken { get; set; }
    }

    public class AccountActor : UntypedActor
    {

        private readonly ILoggingAdapter Log = Context.GetLogger();
        private readonly IMemoryCache _memoryCache;


        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);


        public AccountActor(IMemoryCache memoryCache)
        {
            Console.WriteLine("========= Created AccountActor...");
            _memoryCache = memoryCache;
        }

        protected override void PreStart()
        {
            // subscribe to IMemberEvent and UnreachableMember events
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents,
                new[] { typeof(ClusterEvent.IMemberEvent), typeof(ClusterEvent.UnreachableMember) });
        }

        protected override void PostStop()
        {
            //정상적으로 Actor를 정지하면 멤버에서 탈퇴가됩니다.
            Cluster.Unsubscribe(Self);
        }


        protected void OnReceiveDataByAskToken(AskToken message)
        {
            string loginInfo;
            if (_memoryCache.TryGetValue(CachePrefix.prefix_auth + message.authtoken, out loginInfo))
            {
                Log.Info($"Found:{message.ToString()}");
                Sender.Tell(loginInfo);
            }
            else
            {
                Log.Info("Not Found:" + message.ToString());
            }
        }

        protected override void OnReceive(object message)
        {
            var up = message as ClusterEvent.MemberUp;
            if (up != null)
            {
                var mem = up;
                Log.Info("Member is Up: {0}", mem.Member);
            }
            else if (message is ClusterEvent.UnreachableMember)
            {
                var unreachable = (ClusterEvent.UnreachableMember)message;
                Log.Info("Member detected as unreachable: {0}", unreachable.Member);
                //멤버와 갑자기 연락 두절이 되었기때문에, 멈버를 제거합니다.
                Cluster.Down(unreachable.Member.Address);
            }
            else if (message is ClusterEvent.MemberRemoved)
            {
                var removed = (ClusterEvent.MemberRemoved)message;
                Log.Info("Member is Removed: {0}", removed.Member);
            }
            else if (message is ClusterEvent.IMemberEvent)
            {
                //IGNORE
            }
            else if (message is AskToken)
            {
                OnReceiveDataByAskToken(message as AskToken); //실제 처리할 DataProcess                   
            }
            else if (message is string)
            {
                Log.Info(message as string);
            }
            else
            {
                Unhandled(message);
            }
        }
    }
}
