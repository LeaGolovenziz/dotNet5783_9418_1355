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
            new XElement("product",
                new XElement("id", product.ID),
                new XElement("name", product.Name),
                new XElement("category", product.Category),
                new XElement("price", product.Price),
                new XElement("amount", product.InStock),
                new XElement("image", product.Image));

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
                XmlTools.LoadListFromXMLElement(path,ProductRoot);
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

        static DO.Product? GetProduct(XElement product) =>
        product.ToIntNullable("ID") is null ? null : new DO.Product()
        {
            ID = Convert.ToInt32(product.Element("id") ?? throw new XmlFormatError("id")),
            Name = (string?)product.Element("name") ?? throw new XmlFormatError("name"),
            Category = (DO.Enums.Category?)(int?)product.Element("category") ?? throw new XmlFormatError("category"),
            Price = (double?)product.Element("price") ?? throw new XmlFormatError("price"),
            InStock = (int?)product.Element("amount") ?? throw new XmlFormatError("amount"),
            Image = Convert.ToString(product.Element("image") ?? throw new XmlFormatError("image"))!
        
        };

        /// <summary>
        /// Returns list of all the products, if gets a condition - by it
        /// </summary>
        /// <param name="func"></param>
        /// <returns>IEnumerable<DO.Product?></returns>
        /// <exception cref="FileLoadingError"></exception>
        /// <exception cref="XmlFormatError"></exception>
        public IEnumerable<DO.Product?> Get(Func<DO.Product?, bool>? func = null)
        {
            if (func == null)
                return XmlTools.LoadListFromXMLElement(path, ProductRoot).Elements().Select(product => GetProduct(product));
            else
                return XmlTools.LoadListFromXMLElement(path, ProductRoot).Elements().Select(product => GetProduct(product)).Where(product => func(product));
            //try
            //{
            //    XmlTools.LoadListFromXMLElement(path);
            //}
            //catch
            //{
            //    throw new FileLoadingError();
            //}
            //IEnumerable<DO.Product?> products = (IEnumerable<DO.Product?>)(from product in ProductRoot.Elements()
            //                                   select new DO.Product()
            //                                   {
            //                                       ID = Convert.ToInt32(product.Element("id") ?? throw new XmlFormatError("id")),
            //                                       Name = (string?)product.Element("name") ?? throw new XmlFormatError("name"),
            //                                       Category = (DO.Enums.Category?)(int?)product.Element("category") ?? throw new XmlFormatError("category"),
            //                                       Price = (double?)product.Element("price") ?? throw new XmlFormatError("price"),
            //                                       InStock = (int?)product.Element("amount") ?? throw new XmlFormatError("amount"),
            //                                       Image = Convert.ToString(product.Element("image") ?? throw new XmlFormatError("image"))!
            //                                   });


            //return func != null ? products.Where(x => func(x)) : products;
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
