using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
  public class ExchangeService : IExchangeService
  {
    public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
    {
      // Make http request
      var apiQuery = $"?page={pageNumber}";
      HttpResponseMessage response;
      try
      {
        response = await MessariHelper.makeHttpRequest("/v1/markets" + apiQuery);
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
      var data = await MessariHelper.GetExchangesFromReponse(response);//GetDataFromResponse(response);
      var env = new Envelope<ExchangeDto>
      {
        Items = data,
        PageNumber = pageNumber
      };
      return env;
    }
  }
}