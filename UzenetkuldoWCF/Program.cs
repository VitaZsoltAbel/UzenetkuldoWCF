using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using UzenetkuldoConsole.Services;

namespace UzenetkuldoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8080/");

            using (WebServiceHost host = new WebServiceHost(typeof(UzenetService), baseAddress))
            {
                try
                {
                    ServiceEndpoint endpoint = host.AddServiceEndpoint(
                        typeof(IUzenetService),
                        new WebHttpBinding(),
                        ""
                    );

                    host.Open();

                    Console.WriteLine("=============================================");
                    Console.WriteLine(" WCF REST Szolgáltatás fut...");
                    Console.WriteLine(" Alapértelmezett URL: http://localhost:8080/Uzenetek");
                    Console.WriteLine(" A leállításhoz nyomj ENTER-t.");
                    Console.WriteLine("=============================================");

                    Console.ReadLine();
                    host.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba a szerver indításakor: {ex.Message}");
                    Console.ReadLine();
                }
            }
        }
    }
}