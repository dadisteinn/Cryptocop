namespace Cryptocop.Software.API.Models.DTOs
{
  public class CryptocurrencyDto
  {
    public string Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public float PriceInUsd { get; set; } // metrics -> market_data -> price_data
    public string ProjectDetails { get; set; } // profile -> general -> overview -> project_details

    public override string ToString()
    {
      return $"\n id: {Id}\n symbol: {Symbol}\n Name: {Name}\n Slug: {Slug}\n PriceInUsd: {PriceInUsd}\n ProjectDetails: {ProjectDetails}\n";
    }
  }
}