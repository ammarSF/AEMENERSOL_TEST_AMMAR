using System;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;

namespace AEMENERSOL
{
    class Program
    {
        private static void Main(string[] args)
        {
            RestController controller = new RestController();

            IList<Platform> _platforms = new List<Platform>();
            IList<Well> _wells = new List<Well>();

            
            try
            {
                controller.setClient();
                var token = controller.GetToken();
                Console.WriteLine("Logged in...");
                Console.WriteLine("");

                var dataPlatform = controller.GetPlatformWell(token);
                
                IList<Platform>? obj = JsonConvert.DeserializeObject<IList<Platform>>(dataPlatform);

                
                for (int i = 0; i < obj?.Count; i++)
                {
                    _platforms.Add(obj[i]);

                    for (int j = 0; j < obj[i]?.well?.Count; j++)
                    {
                        _wells.Add(obj[i]?.well[j]);
                    }
                }

                foreach(var p in _platforms) 
                {
                    int count_well = 0;
                    foreach(var w in _wells)
                    {
                        count_well++;
                    }
                    Console.WriteLine("Platform " + p.uniqueName + " has " + count_well + " wells.");
                }

                using (var ctx = new PlatformWellContext()) 
               {
                    Console.WriteLine("START DB TRANSACTION");
                    foreach (var _p in _platforms)
                    {
                        ctx.PlatformTable.AddOrUpdate(_p);
                    }
                    foreach (var _w in _wells)
                    {
                        ctx.WellTable.AddOrUpdate(_w);
                    }

                    try
                    {
                        int num = ctx.SaveChanges();
                    }catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                    IList<PlatformTable> p = ctx.PlatformTable.ToList();
                }
                Console.WriteLine("DB TRANSACTION FINISH");

            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.ReadLine();
        }
    }
}