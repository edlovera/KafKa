using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK_Kafka
{
    class Productor
    {
        private static string topic;
        private static ProducerConfig config;

        public static void IniciarConfig()
        {
            topic = "ZK-BioData";
            config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "999"
                
            };
        }

        public static void EscribirEnTopic()
        {
            IniciarConfig();
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                for(int i = 0; i <= 10000; i++)
                {
                    producer.Produce(topic, new Message<Null, string> { Value = "mensaje " + i.ToString() });

                    producer.Flush();
                    
                }
            }
        }
    }
}
