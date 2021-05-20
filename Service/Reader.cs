using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace TestTaskWeb.Service
{
    public class Reader
    {

        private readonly ILogger<Reader> logger;
        public Reader(ILogger<Reader> logger)
        {
            this.logger = logger;
        }
        public async Task<string> GetJSON(string path)
        {




            string result = string.Empty;

           
                     
                        if (File.Exists(path))
                        {
                            if (Path.GetExtension(path) == ".xml")
                            {
                                XDocument _xDocument = XDocument.Load(@path);

                                var _tmp = _xDocument.Element("CATALOG").Elements("CD");
                                CD_Analys cDAnalys = new CD_Analys();
                                foreach (var rs in _tmp)
                                {
                                    //increment every cd
                                    cDAnalys.CdsCount++;
                                    //get counntries
                                    cDAnalys.Countries.Add(rs.Element("COUNTRY").Value);
                                    //get price
                                    var t = rs.Element("PRICE").Value;
                                    var q = t.Replace('.', ',');
                                    decimal val;
                                    if (decimal.TryParse(q, out val))
                                    {
                                        cDAnalys.PricesSum += val;
                                    }


                                    int curYear;
                                    if (int.TryParse(rs.Element("YEAR").Value, out curYear))
                                    {

                                    }

                                    //get years and compare with saved
                                    if ((curYear < cDAnalys.MinYear) || (cDAnalys.MinYear == 0))
                                    {
                                        cDAnalys.MinYear = int.Parse(rs.Element("YEAR").Value);
                                    }

                                    if ((curYear > cDAnalys.MaxYear) || (cDAnalys.MaxYear == 0))
                                    {
                                        cDAnalys.MaxYear = int.Parse(rs.Element("YEAR").Value);
                                    }



                                }
                                //replace repeated countries
                                cDAnalys.Countries = cDAnalys.Countries.Distinct().ToList();
                                string _result = JsonConvert.SerializeObject(cDAnalys, Formatting.Indented);
                    result = _result;

                    logger.LogInformation($"JSON result is {_result}");
                            
                            }
                        }
                        else
                        {

                logger.LogWarning("Incorrect path");
                        }

            return await Task.FromResult(result);

                      
                   


                    

                }
            
        
    }
    public class CD_Analys
    {
        public int CdsCount { get; set; } = 0;
        public decimal PricesSum { get; set; } = 0;
        public List<string> Countries { get; set; } = new List<string>();
        public int MinYear
        {
            get; set;
        } = 0;
        public int MaxYear { get; set; } = 0;
    }
}
