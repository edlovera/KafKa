using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZK_Kafka
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var arg1 = args.Length == 0 ? "C" : args[0];

            var arg2 = args.Length <= 1 ? "5" : args[1];

            Console.WriteLine("Creando {0} consumidores", arg1);

            if (arg1 != "C" && arg1 != "P")
            {
                Console.WriteLine("El primer argumento '{0}' indica la debe ser C para Consumidor o P para productor", arg1);
            }
            if (!Int32.TryParse(arg2, out int cantConsumidores))
            {
                Console.WriteLine("El segundo argumento '{0}' indica la cantidad de consumidores, por lo que debe ser númerico", arg2);
            }
 

            switch (arg1)
            {
                case "C":
                    Task[] t = new Task[cantConsumidores];
                    for (int i = 0; i < cantConsumidores; i++)
                    {
                        t[i] = Task.Run(() => Consumidor.LeerTopic(i));
                    }
                    Task.WaitAll(t);
                    break;
                case "P":
                    Productor.EscribirEnTopic();
                    break;
            }

         

        
 
        }
    }
}
