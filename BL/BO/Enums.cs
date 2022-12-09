namespace BO;

/// <summary>
/// Structure for enums
/// </summary>
public struct Enums
{
    /// <summary>
    /// unique categories of products
    /// </summary>
    public enum Category { Trees, Flowers, Pots, Tools, Soils};
    /// <summary>
    /// unique statuses of order
    /// </summary>
    public enum OrderStatus { Confirmed, Sent, Delivered };
}
