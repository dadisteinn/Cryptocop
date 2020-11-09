
using System;

namespace Cryptocop.Software.API.Models.DTOs
{
  public class ExchangeDto
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string AssetSymbol { get; set; }
    public float? PriceInUsd { get; set; }
    public DateTime? LastTrade { get; set; }

    public override string ToString()
    {
      return $"\n id: {Id}\n Name: {Name}\n Slug: {Slug}\n AssetSymbol: {AssetSymbol}\n PriceInUsd: {PriceInUsd}\n LastTrade: {LastTrade}\n";
    }
  }
}