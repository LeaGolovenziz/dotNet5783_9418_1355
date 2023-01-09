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
            // Try to load the file of the products
            try
            {
                XmlTools.LoadListFromXMLElement(path, ProductRoot);
            }
            catch
            {
                throw new FileLoadingError();
            }

            // try to find the product in the file - if doesnt exist add it
            //try
            //{
            //    Get(product.ID);
            //    throw new AlreadyExist();
            //}
            //catch (DO.NotFound)
            {
                add(product);

                // Try to save the file with the additional product
                try
                {
                    XmlTools.SaveListToXMLElement(ProductRoot, path);
                }
                catch
                {
                    throw new FileSavingError();
                }
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
                 where (int)product.Element("id")! == id
                 select product).FirstOrDefault()?.Remove();
            }
            catch
            {
                throw new NotFound();
            }
        }

        public void Delete(int id)
        {
            try
            {
                XmlTools.LoadListFromXMLElement(path, ProductRoot);
            }
            catch
            {
                throw new FileLoadingError();
            }

            delete(id);

            try
            {
                XmlTools.SaveListToXMLElement(ProductRoot, path);
            }
            catch
            {
                throw new FileSavingError();
            }
        }

        /// <summary>
        /// Gets a product and updates it in the file of the products
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="FileLoadingError"></exception>
        /// <exception cref="FileSavingError"></exception>
        public void Update(DO.Product product)
        {
            try
            {
                XmlTools.LoadListFromXMLElement(path, ProductRoot);
            }
            catch
            {
                throw new FileLoadingError();
            }

            delete(product.ID);
            add(product);

            try
            {
                XmlTools.SaveListToXMLElement(ProductRoot, path);
            }
            catch
            {
                throw new FileSavingError();
            }
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
            List<DO.Product?> products = XmlTools.LoadListFromXMLSerializer<DO.Product?>(path, ProductRoot.Name.ToString());


            return func == null ? products.AsEnumerable() : products.Where(func);

            //try
            //{
            //    XmlTools.LoadListFromXMLElement(path, ProductRoot);
            //}
            //catch
            //{
            //    throw new FileLoadingError();
            //}
            //var products = from product in ProductRoot.Elements()
            //               select new DO.Product()
            //               {
            //                   ID = (int)product.Element("ID")!,
            //                   Name = (string?)product.Element("Name"),
            //                   Category = (DO.Enums.Category?)(int?)product.Element("Category"),
            //                   Price = (double?)product.Element("Price"),
            //                   InStock = (int?)product.Element("InStock"),
            //                   Image = (string)(product.Element("Image"))!
            //               };


            //return func != null ? products.Cast<DO.Product?>().Where(x => func(x)) : products.Cast<DO.Product?>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="nullvalue"></exception>
        public DO.Product Get(int id)
        {
            return GetIf(product => (product ?? throw new Nullvalue()).ID == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="NotFound"></exception>
        public DO.Product GetIf(Func<DO.Product?, bool>? func)
        {
            return Get(func).FirstOrDefault() ?? throw new NotFound();
        }

    }
}
