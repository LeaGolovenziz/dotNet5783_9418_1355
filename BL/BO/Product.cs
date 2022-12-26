using static BO.Enums;

namespace BO
{
    public class Product
    {
        /// <summary>
        /// Unique ID of product
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Unique name of product
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Unique price of product
        /// </summary>
        public double? Price { get; set; }
        /// <summary>
        /// Unique catgory of product
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Unique inStock of product
        /// </summary>
        public int? InStock { get; set; }
        /// <summary>
        /// Unique image of product
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// returns a string of the product's details
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.ToStringProperty();
    }
}
