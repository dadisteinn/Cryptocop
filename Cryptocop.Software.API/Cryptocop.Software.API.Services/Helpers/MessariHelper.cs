using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Newtonsoft.Json.Linq;

namespace Cryptocop.Software.API.Services.Helpers
{
  public static class MessariHelper
  {
    public static async Task<HttpResponseMessage> makeHttpRequest(string directoryString)
    {
      var client = new HttpClient();
      client.BaseAddress = new System.Uri("https://data.messari.io/api");
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      return await client.GetAsync(client.BaseAddress + directoryString);
    }

    public static async Task<List<CryptocurrencyDto>> GetCryptoListFromResponse(HttpResponseMessage response, List<string> toFindList)
    {
      var json = await response.Content.ReadAsStringAsync();
      var jsonObject = JObject.Parse(json);
      var results = new List<CryptocurrencyDto>();

      foreach (var data in jsonObject["data"].ToList())
      {
        var m = data.ToObject<CryptocurrencyDto>();
        if (toFindList.Contains(m.Symbol))
        {
          results.Add(m);

          if (float.TryParse(data["metrics"]["market_data"]["price_usd"].ToString(), out var priceAmt))
            m.PriceInUsd = priceAmt;

          m.ProjectDetails = data["profile"]["general"]["overview"]["project_details"].ToString();
        }
      }
      return results;
    }

    public static async Task<float> GetCryptoPriceFromResponse(HttpResponseMessage response)
    {
      var json = await response.Content.ReadAsStringAsync();
      var jsonObject = JObject.Parse(json);
      float.TryParse(jsonObject["data"]["market_data"]["price_usd"].ToString(), out var priceInUsd);
      return priceInUsd;
    }

    public static async Task<List<ExchangeDto>> GetExchangesFromReponse(HttpResponseMessage response)
    {
      var json = await response.Content.ReadAsStringAsync();
      var jsonObject = JObject.Parse(json);
      var results = new List<ExchangeDto>();

      foreach (var data in jsonObject["data"].ToList())
      {
        var timeString = data["last_trade_at"].ToString();
        DateTime? time = null;

        if (timeString != "" && timeString != null)
        {
          time = DateTime.Parse(timeString);
        }
        float.TryParse(data["price_usd"].ToString(), out var priceInUsd);

        var dto = new ExchangeDto
        {
          Id = data["id"].ToString(),
          Name = data["exchange_name"].ToString(),
          Slug = data["exchange_slug"].ToString(),
          AssetSymbol = data["base_asset_symbol"].ToString(),
          PriceInUsd = priceInUsd,
          LastTrade = time
        };
        results.Add(dto);
      }
      return results;
    }
  }
}