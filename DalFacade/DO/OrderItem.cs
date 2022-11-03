namespace DO;

/// <summary>
/// Structure for conecction between a product and an order
/// </summary>
public struct OrderItem
{
    /// <summary>
    /// Unique ID of the Ordered Item
    /// </summary>
    public int OrderItemID { get; set; }
    /// <summary>
    /// Unique Product's ID of the Ordered Item
    /// </summary>
    public int ProductID { get; set; }
    /// <summary>
    /// Unique Order's ID of the Ordered Item
    /// </summary>
    public int? OrderID { get; set; }
    /// <summary>
    /// Unique Price of the Ordered Item
    /// </summary>
    public double? Price { get; set; }
    /// <summary>
    /// Unique Amount of the Ordered Item
    /// </summary>
    public int? Amount { get; set; }

    /// <summary>
    /// returns a string of the ordered item's details
    /// </summary>
    /// <returns>string</returns>
    public override string ToString() => $@"
       OrderItemID -{OrderItemID}
       Order's ID - {OrderID}
       Product's ID - {ProductID}
       price - {Price}
       amount - {Amount}
";
}
