using System;
using System.Xml.Linq;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using System.IO;

namespace YMLParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int shopId = Convert.ToInt32(args[1]);
                switch(args[0])
                {
                    case "save":
                        string ymlUrl = args[2];
                        SaveData(shopId, ymlUrl);
                        break;
                    case "print":
                        PrintData(shopId);
                        break;
                    default:
                        Console.WriteLine("Please use commands like 'run' or 'save'");
                        break;
                }
            }
            catch (IndexOutOfRangeException indexException)
            {
                Console.WriteLine(indexException.Message + "\nTry adding more arguments.");
            }
            catch (FormatException formatException){
                Console.WriteLine(formatException.Message);
            }
            
        }
        /// <summary>
        /// Parses yml/xml given in url, saves parsed information in database
        /// </summary>
        /// <param name="shopId">Shop id</param>
        /// <param name="url">Link for yml/xml file</param>
        private static void SaveData(int shopId, string url)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            XElement shopYMLData = XElement.Load(url);
            IEnumerable<ShopDto> shopData = from item in shopYMLData.Descendants("offer") select new ShopDto{
                                                                                                            id = (int)item.Attribute("id"), 
                                                                                                            name = item.Element("name").Value, 
                                                                                                            shopId = shopId
                                                                                                            };
            CrudController controller = new CrudController();
            foreach(var item in shopData)
            {
                controller.Create(item);
            }
            Console.WriteLine("Saved {0} entries in db", shopData.Count());
        }
        /// <summary>
        /// Prints database entries given shopId (up to 50 entries)
        /// </summary>
        /// <param name="shopId">Shop id</param>
        private static void PrintData(int shopId)
        {
           CrudController controller = new CrudController();
           List<ShopDto> items = controller.GetMany(shopId);
           using(var memory = new MemoryStream())
           using(var writer = new StreamWriter(memory))
           using(var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
           {
               csvWriter.Configuration.Delimiter = "; ";
               csvWriter.Configuration.HasHeaderRecord = true;
               csvWriter.Configuration.AutoMap<ShopDto>();
               csvWriter.WriteHeader<ShopDto>();
               csvWriter.WriteRecords(items);
               writer.Flush();
               var result = Encoding.UTF8.GetString(memory.ToArray());
               Console.WriteLine(result);
           }

        }
    }
}
