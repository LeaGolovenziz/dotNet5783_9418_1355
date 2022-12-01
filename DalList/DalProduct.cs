using DO;
using System.Security.Cryptography.X509Certificates;
using DalApi;
namespace Dal;

internal class DalProduct : IProduct
{
    /// <summary>
    /// adds the product to the produts list and return it's id if doesn't already exist
    /// </summary>
    /// <param name="product"></param>
    /// <returns>int</returns>
    /// <exception cref="Exception"></exception>
    public int Add(Product product)
    {
        if (DataSource._lstPruducts.Exists(x => x.ID == product.ID))
            throw new AlreadyExist();
        DataSource._lstPruducts.Add(product);
        return product.ID;
       
    }
    /// <summary>
    /// gets an id and return the product with this id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Product</returns>
    /// <exception cref="Exception"></exception>
    public Product Get(int id)
    {
        if (!DataSource._lstPruducts.Exists(x => x.ID == id))
            throw new NotFound();
        return DataSource._lstPruducts.Find(x => x.ID == id);
    }

    /// <summary>
    /// gets an ID and deletes the product with this ID
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        DataSource._lstPruducts.Remove(Get(id));
    }

    /// <summary>
    /// gets an product and updetes it
    /// </summary>
    /// <param name="product"></param>
    public void Update(Product product)
    {
        int index = DataSource._lstPruducts.FindIndex(x => x.ID == product.ID);
        if (index == -1)
            throw new NotFound();
        DataSource._lstPruducts[index] = product;
    }

    /// <summary>
    /// return the list of products
    /// </summary>
    /// <returns>List<Order></returns>
    public IEnumerable<Product?> Get(Func<Product?, bool>? func)
    {
        if(func != null)
            return (IEnumerable<Product?>)DataSource._lstPruducts.Where(x => func(x)).ToList();
        return (IEnumerable<Product?>)DataSource._lstPruducts;
    }
}
