﻿using EI_OpgaveApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Services
{
    public class SalesPersonService
    {
        public async Task<SalesPerson[]> GetSalesPersonsAsync()
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync("api/salesperson/");

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<SalesPerson[]>(statsJson);

                return rootObject;
            }
            catch
            {
                SalesPerson[] sl = null;
                return sl;
            }
        }

        public async Task<SalesPerson> GetSalesPersonAsync(string id)
        {
            try
            {
                HttpClient client = ClientGateway.GetHttpClient;

                var response = await client.GetAsync("api/salesperson/" + id);

                var statsJson = response.Content.ReadAsStringAsync().Result;

                var rootObject = JsonConvert.DeserializeObject<SalesPerson>(statsJson);

                return rootObject;
            }
            catch
            {
                SalesPerson sp = null;
                return sp;
            }
        }
    }
}
