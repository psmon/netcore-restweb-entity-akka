using Akka.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Config
{

    public class AkkaSetting
    {
        Akka.Configuration.Config hConfig;

        public AkkaSetting(IConfiguration configuration)
        {
            string akkaip = configuration.GetSection("akkaip").Value;
            string akkaport = configuration.GetSection("akkaport").Value;
            string akkaseed = configuration.GetSection("akkaseed").Value;

            string hconString = @"
            akka {
                remote {
                    dot-netty.tcp {
                        port = $akkaport
                        hostname = ""$akkaip""
                    }
                }

                actor{
                    provider = cluster
                    deployment {
                      /scatter {
                        router = scatter-gather-group # routing strategy
                        routees.paths = [""/user/accountpool""]
                        nr -of-instances = 10 # max number of total routees
                        cluster {
                            enabled = on
                            allow-local-routees = off
                            use-role = account
                            max-nr-of-instances-per-node = 1
                        }
                      }
                    }
                }

                actor {
                   serializers {
                      json = ""Akka.Serialization.NewtonSoftJsonSerializer""
                      bytes = ""Akka.Serialization.ByteArraySerializer""
                   }
                    serialization-bindings {
                      ""System.Byte[]"" = bytes
                      ""System.Object"" = json
                    }
                }

                cluster {
                   seed-nodes = [""akka.tcp://AccountClusterSystem@$akkaseed""] # address of seed node
                   roles = [""account""] # roles this member is in
                }
            }
            "
            .Replace("$akkaport", akkaport)
            .Replace("$akkaip", akkaip)
            .Replace("$akkaseed", akkaseed);

            hConfig = ConfigurationFactory.ParseString(hconString);
        }

        public Akka.Configuration.Config GetConfig()
        {
            return hConfig;
        }
    }
}
