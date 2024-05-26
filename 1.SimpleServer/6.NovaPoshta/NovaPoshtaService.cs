﻿using _6.NovaPoshta.Data;
using _6.NovaPoshta.Models;
using _6.NovaPoshta.Models.City;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta
{
    public class NovaPoshtaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly MyDataContext _dataContext;

        public NovaPoshtaService()
        {
            _httpClient = new HttpClient();
            _url = "https://api.novaposhta.ua/v2.0/json/";
            _dataContext = new MyDataContext();
            _dataContext.Database.Migrate();
        }
        public void GetAreas() 
        {
            string key = "63aa362a44e812e38243bd8fb803b606";

            AreaRequestDTO areaRequestDTO = new AreaRequestDTO
            {
                ApiKey = key,
                ModelName = "Address",
                CalledMethod = "getSettlementAreas",
                MethodProperties = new AreaRequesPropertyDTO
                {
                    Ref="",
                    Page=1
                }
            };

            string json = JsonConvert.SerializeObject(areaRequestDTO);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<AreaResponseDTO>(responseData);
                if (result.Data.Any())
                {
                    foreach (var area in result.Data)
                    {
                        var entity = _dataContext.Areas.SingleOrDefault(x => x.Ref == area.Ref);
                        if (entity is null)
                        {
                            entity = new Data.Entities.AreaEntity
                            {
                                Name = area.Description,
                                Ref = area.Ref
                            };
                            _dataContext.Areas.Add(entity);
                            _dataContext.SaveChanges(); 
                        }
                    }
                }

                //Console.WriteLine("Response {0}", responseData);
                //var result = JsonConvert.DeserializeObject<>
            }
            else
            {
                Console.WriteLine($"Помилка запиту: {response.StatusCode}");
            }
        }

        public void GetSettlements()
        {
            string key = "63aa362a44e812e38243bd8fb803b606";
            int page = 1;
            while(true)
            {
                SettlementRequestDTO areaRequestDTO = new SettlementRequestDTO
                {
                    ApiKey = key,
                    ModelName = "AddressGeneral",
                    CalledMethod = "getSettlements",
                    MethodProperties = new SettlementRequestPropertyDTO
                    {
                        Page = page
                    }
                };

                string json = JsonConvert.SerializeObject(areaRequestDTO);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<SettlementResponseDTO>(responseData);
                    if (result.Data.Any())
                    {
                        foreach (var settlement in result.Data)
                        {
                            var entity = _dataContext.Settlements.SingleOrDefault(x => x.Ref == settlement.Ref);
                            if (entity is null)
                            {
                                Console.WriteLine("{0} {1}", page, settlement.Description);
                                entity = new Data.Entities.SettlementEntity
                                {
                                    Description = settlement.Description,
                                    Ref = settlement.Ref,
                                    AreaId = _dataContext.Areas.Where(x => x.Ref == settlement.Area).Select(x => x.Id).SingleOrDefault()

                                };
                                _dataContext.Settlements.Add(entity);
                            }
                        }
                        _dataContext.SaveChanges();
                        page++;
                    }
                    else
                        return;
                }
                else
                {
                    Console.WriteLine($"Помилка запиту: {response.StatusCode}");
                }
            }
           
        }
    }
}
