using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Dal
{
    internal class Product : IProduct
    {
        XElement ProductRoot;
        string path = @"Product.xml";

        public Product() => ProductRoot = new XElement("products");


        /// <summary>
        /// gets a product and create a XElement of this product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>XElement</returns>
        private XElement create(DO.Product product) =>
            new XElement("Product",
                new XElement("ID", product.ID),
                new XElement("Name", product.Name),
                new XElement("Category", product.Category),
                new XElement("Price", product.Price),
                new XElement("InStock", product.InStock),
                new XElement("Image", product.Image));

        /// <summary>
        /// helper function
        /// </summary>
        /// <param name="product"></param>
        private void add(DO.Product product) => ProductRoot.Add(create(product));

        /// <summary>
        /// gets a product and adds it to the products' file
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int Add(DO.Product product)
        {
            XmlTools.LoadListFromXMLElement(path, ProductRoot);

            //try to find the product in the file - if doesnt exist add it
            try
            {
                Get(product.ID);
                throw new AlreadyExist();
            }
            catch (NotFound)
            {
                add(product);

                XmlTools.SaveListToXMLElement(ProductRoot, path);
            }
            return product.ID;
        }

        /// <summary>
        /// helper function
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotFound"></exception>
        private void delete(int id)
        {
            try
            {
                (from product in ProductRoot.Elements()
                 where (int)product.Element("ID")! == id
                 select product).FirstOrDefault()?.Remove();
            }
            catch (NotFound)
            {
                throw new NotFound();
            }
        }

        /// <summary>
        /// Deletes a product from the file of products
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="FileLoadingError"></exception>
        /// <exception cref="FileSavingError"></exception>
        public void Delete(int id)
        {
            XmlTools.LoadListFromXMLElement(path, ProductRoot);

            delete(id);

            XmlTools.SaveListToXMLElement(ProductRoot, path);
        }

        /// <summary>
        /// Gets a product and updates it in the file of the products
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="FileLoadingError"></exception>
        /// <exception cref="FileSavingError"></exception>
        public void Update(DO.Product product)
        {

            XmlTools.LoadListFromXMLElement(path, ProductRoot);

            delete(product.ID);
            add(product);

            XmlTools.SaveListToXMLElement(ProductRoot, path);
        }

        /// <summary>
        /// Returns list of all the products, if gets a condition - by it
        /// </summary>
        /// <param name="func"></param>
        /// <returns>IEnumerable<DO.Product?></returns>
        /// <exception cref="FileLoadingError"></exception>
        /// <exception cref="XmlFormatError"></exception>
        public IEnumerable<DO.Product?> Get(Func<DO.Product?, bool>? func = null)
        {
            ProductRoot = XmlTools.LoadListFromXMLElement(path, ProductRoot);

            var products = from product in ProductRoot.Elements()
                           select new DO.Product()
                           {
                               ID = Convert.ToInt32(product.Element("ID")!.Value),
                               Name = product.Element("Name")!.Value,
                               Category = (DO.Enums.Category)Enum.Parse(typeof(DO.Enums.Category), product.Element("Category")!.Value),
                               Price = Convert.ToDouble(product.Element("Price")!.Value),
                               InStock = Convert.ToInt32(product.Element("InStock")!.Value),
                               Image = product.Element("Image")!.Value
                           };


            return func != null ? products.Cast<DO.Product?>().Where(func) : products.Cast<DO.Product?>();
        }


        /// <summary>
        /// gets an ID and returns the product with this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DO.Product</returns>
        /// <exception cref="nullvalue"></exception>
        public DO.Product Get(int id)
        {
            return GetIf(product => (product ?? throw new Nullvalue()).ID == id);
        }

        /// <summary>
        /// Gets a condition and returns an product with this condition
        /// </summary>
        /// <param name="func"></param>
        /// <returns>DO.Product</returns>
        /// <exception cref="NotFound"></exception>
        public DO.Product GetIf(Func<DO.Product?, bool>? func)
        {
            return Get(func).FirstOrDefault() ?? throw new NotFound();
        }
    }
}
