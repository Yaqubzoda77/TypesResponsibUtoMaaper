public class Address
{
  public int AddressId { get; set; } 
  public string Address1 { get; set; }
  public string Address2 { get; set; }
  public string City { get; set; }
  public int PostalCode { get; set; }
  public int CustomerId { get; set; }
  public Customer Customers { get; set; }
}
