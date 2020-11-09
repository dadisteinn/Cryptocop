namespace Cryptocop.Software.API.Models.Entities
{
  public class OrderItem
  {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProducrIdentifier { get; set; } //TODO: misspelled, should be ProductIdentifier
    public float Quantity { get; set; }
    public float UnitPrice { get; set; }
    public float TotalPrice { get; set; }
  }
}