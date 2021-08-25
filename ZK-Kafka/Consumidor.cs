using Confluent.Kafka;
using System;

using System.Threading;
using ZK_Kafka.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using ZK_Kafka.DBContext.Modelos;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ZK_Kafka
{
    class Consumidor
    {
        private static ConsumerConfig config;
        private static String topic;
        private static bool cancelar;

        public Consumidor()
        {

        }
        public static void IniciarConfig()
        {
            cancelar = false;
            topic = "ZK-BioData";
            config = new ConsumerConfig
            {

                //BootstrapServers = "192.168.1.128:9092",
                BootstrapServers = "localhost:9092",
                GroupId = "biodata-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                EnablePartitionEof = true,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000
               
            };

        }

        public static void LeerTopic(int i)
        {



            IniciarConfig();
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topic);
                CancellationTokenSource cts = new CancellationTokenSource();

                
                while (!cancelar)
                {
              
                     var consumeResult = consumer.Consume(cts.Token);

                    //BioData bioData = JsonSerializer.Deserialize<BioData>(consumeResult.Message.Value.ToString());
                    if (!String.IsNullOrEmpty(consumeResult.Message?.Value))
                    {
                        Console.WriteLine("Consumidor {0}: Leyendo offset {1} de la particion {2}, {3}", i, consumeResult.Offset, consumeResult.Partition, consumeResult.Message?.Value);
                        consumer.Commit(consumeResult);
                    }

                    if (consumeResult.IsPartitionEOF)
                    {

                        Console.WriteLine("Fin de algo");
                        continue;
                    }

                  
                  

    
                }

  

                //var status = await InsertarBioDataGenHora(bioData);
                //if (status)
                //{
                //    consumer.Commit(consumeResult);

                //    Console.WriteLine("BioData insertada en {0}", bioData.EmpresaNombreBD);
                //}



                consumer.Close();
            }
        }

        private static async Task<bool> InsertarBioDataGenHora(BioData bioData)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            String cadenaConexion = String.Format("Data Source=192.168.110.46;Initial Catalog={0};user id=sa;password=Pqgen.gen.19;Trust Server Certificate = true", bioData.EmpresaNombreBD);
            GenHorasEntitidades conexion = new GenHorasEntitidades(cadenaConexion);


            SqlParameter[] parametros = new[]
            {
                        new SqlParameter("Pin", bioData.Pin),
                        new SqlParameter("No", bioData.No),
                        new SqlParameter("Index", bioData.Index),
                        new SqlParameter("Valid", bioData.Valid),
                        new SqlParameter("Duress", bioData.Duress),
                        new SqlParameter("Type", bioData.Type),
                        new SqlParameter("Majorver", bioData.Majorver),
                        new SqlParameter("Minorver", bioData.Minorver),
                        new SqlParameter("Format", bioData.Format),
                        new SqlParameter("Tmp", bioData.Tmp),
                        new SqlParameter("equ_serie", bioData.Equipo),
            };

            var resultado = await conexion.SPBioData.FromSqlRaw("sp_acceso_i_zk_to_biodata @Pin, @No, @Index, @Valid, @Duress, @Type, @Majorver, @Minorver, @Format, @Tmp, @equ_serie", parametros).ToListAsync();
            conexion.Dispose();

            return (bool)resultado.FirstOrDefault()?.Status;
        }
    }
}
