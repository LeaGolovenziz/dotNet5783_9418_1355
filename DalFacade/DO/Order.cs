﻿namespace DO;

/// <summary>
/// Structure for orders
/// </summary>
public struct Order
{
    /// <summary>
    /// Unique ID of order
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Unique custumer's name of order
    /// </summary>
    public string? CustumerName { get; set; }
    /// <summary>
    /// Unique austumer's email of order
    /// </summary>
    public string? CustumerEmail{ get; set; }
    /// <summary>
    /// Unique custumer's adress of order
    /// </summary>
    public string? CustumerAdress { get; set; }
    /// <summary>
    /// Unique date of order
    /// </summary>
    public DateTime? OrderDate { get; set; }
    /// <summary>
    /// Unique ship date of order
    /// </summary>
    public DateTime? ShipDate { get; set; }
    /// <summary>
    /// Unique delivery date of order
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// returns a string of the order's details
    /// </summary>
    /// <returns>string</returns>
    public override string ToString() => $@"
       Order's ID - {ID}: 
       Custumer's name - {CustumerName}
       Custumer's Email - {CustumerEmail}
       Custumer's adress - {CustumerAdress}
       Order's date - {OrderDate}
       Order's ship date - {ShipDate}
       Order's delivery date - {DeliveryDate}
";
}