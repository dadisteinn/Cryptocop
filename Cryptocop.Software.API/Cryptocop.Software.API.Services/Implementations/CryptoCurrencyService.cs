using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
  public class CryptoCurrencyService : ICryptoCurrencyService
  {
    readonly private static List<string> availvableCryptoCurrs = new List<string> { "BTC", "ETH", "USDT", "XMR" };

    public async Task<IEnumerable<CryptocurrencyDto>> GetAvailableCryptocurrencies()
    {
      // Make http request
      var apiQuery = "?fields=id,symbol,name,slug,metrics/market_data/price_usd,profile/general/overview/project_details";
      HttpResponseMessage response;
      try
      {
        response = await MessariHelper.makeHttpRequest("/v2/assets" + apiQuery);
      }
      catch (MessariException e)
      {
        throw e;
      }

      // Throw error if response was not successful
      if (!response.IsSuccessStatusCode)
      {
        throw new MessariException("Failed to get successful response from Messari api.");
      }

      // Normalize data end return
      var data = await MessariHelper.GetCryptoListFromResponse(response, availvableCryptoCurrs);
      return data;
    }

    public async Task<float> GetCryptocurrencyPrice(string productIdentifier)
    {
      // Make http request
      var apiQuery = "?fields=market_data/price_usd";
      HttpResponseMessage response;
      try
      {
        response = await MessariHelper.makeHttpRequest($"/v1/assets/{productIdentifier}/metrics/market-data" + apiQuery);
      }
      catch (MessariException e)
      {
        throw e;
      }

      // Throw error if response was not successful
      if (!response.IsSuccessStatusCode)
      {
        throw new MessariException("Failed to get successful response from Messari api.");
      }

      // Normalize data end return
      return await MessariHelper.GetCryptoPriceFromResponse(response);
    }
  }
}