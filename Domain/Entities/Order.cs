public class Order
{
  public int OrderId { get; set; }
  public string OrderNumber { get; set; }
  public DateTime OrderDate { get; set; } 
  public decimal TotalAmount { get; set; }
  public int CustomerId { get; set; }
  public Customer Customers { get; set; }
}