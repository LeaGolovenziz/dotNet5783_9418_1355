using DO;
using System.Security.Cryptography.X509Certificates;

namespace Dal;

public class DalProduct
{
    /// <summary>
    /// adds the product to the produts list and return it's id if doesn't already exist
    /// </summary>
    /// <param name="product"></param>
    /// <returns>int</returns>
    /// <exception cref="Exception"></exception>
    public static int Add(Product product)
    {
        if (!DataSource._lstPruducts.Exists(x => x.ID == product.ID))
        {
            DataSource._lstPruducts.Add(product);
            return product.ID;
        }
        throw new Exception("the product is already exist");
    }
    /// <summary>
    /// gets an id and return the product with this id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Product</returns>
    /// <exception cref="Exception"></exception>
    public static Product Get(int id)
    {
        if (DataSource._lstPruducts.Exists(x => x.ID == id))
            return DataSource._lstPruducts.Find(x => x.ID == id);
        throw new Exception("the product doesn't exist");
    }

    /// <summary>
    /// gets an ID and deletes the product with this ID
    /// </summary>
    /// <param name="id"></param>
    public static void Delete(int id)
    {
        DataSource._lstPruducts.Remove(Get(id));
    }

    /// <summary>
    /// gets an product and updetes it
    /// </summary>
    /// <param name="product"></param>
    public static void Update(Product product)
    {
        Delete(product.ID);
        Add(product);
    }

    /// <summary>
    /// return the list of products
    /// </summary>
    /// <returns>List<Order></returns>
    public static List<Product> Get()
    {
        return DataSource._lstPruducts.ToList();
    }
}
