using DalApi;
using DO;
namespace Dal;

public class DalProduct : IProduct
{
    /// <summary>
    /// adds the product to the produts list and return it's id if doesn't already exist
    /// </summary>
    /// <param name="product"></param>
    /// <returns>int</returns>
    /// <exception cref="Exception"></exception>
    public int Add(Product product)
    {
        if (DataSource.lstPruducts.Exists(x => (x?? throw new nullvalue()).ID == product.ID))
            throw new AlreadyExist();
        DataSource.lstPruducts.Add(product);
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
        return GetIf(product => (product ?? throw new nullvalue()).ID == id);
    }

    /// <summary>
    /// gets an ID and deletes the product with this ID
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        DataSource.lstPruducts.Remove(Get(id));
    }

    /// <summary>
    /// gets an product and updetes it
    /// </summary>
    /// <param name="product"></param>
    public void Update(Product product)
    {
        int index = DataSource.lstPruducts.FindIndex(x => (x ?? throw new nullvalue()).ID == product.ID);
        if (index == -1)
            throw new NotFound();
        DataSource.lstPruducts[index] = product;
    }

    /// <summary>
    /// return the list of products
    /// </summary>
    /// <returns>List<Order></returns>
    public IEnumerable<Product?> Get(Func<Product?, bool>? func)
    {
        if (func != null)
            return DataSource.lstPruducts.Where(x => func(x)).ToList();
        return DataSource.lstPruducts;
    }
    /// <summary>
    ///  returns product who meets the condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns>Product</returns>
    /// <exception cref="NotFound"></exception>
    public Product GetIf(Func<Product?, bool>? func)
    {
        if (DataSource.lstPruducts.Exists(x => func!(x)))
            return (Product)DataSource.lstPruducts.Find(x => func!(x))!;
        throw new NotFound();
    }
}
