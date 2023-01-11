using DO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    static class XmlTools
    {
        // xml files' folders name
        static string dir = @"..\xml\";
        public static string configPath = dir + @"Config.xml";

        static XmlTools()
        {
            // if the xml folderdoesn't exists create the folder
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static int? ToIntNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;

        /// <summary>
        /// loads the products file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadingError"></exception>
        public static XElement LoadListFromXMLElement(string filePath, XElement root)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(root.Name);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception)
            {
                throw new FileLoadingError();
            }
        }

        /// <summary>
        /// saves the products file
        /// </summary>
        /// <param name="rootElem"></param>
        /// <param name="filePath"></param>
        /// <exception cref="FileSavingError"></exception>
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception)
            {
                throw new FileSavingError();
            }
        }

        public static List<T?> LoadListFromXMLSerializer<T>(string filePath,string rootName)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T?> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T?>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T?>)x.Deserialize(file)!;
                    file.Close();
                    return list;
                }
                else
                {
                    XElement rootElem = new XElement(rootName);
                    rootElem.Save(dir + filePath);
                    return new List<T?>()!;
                }
            }
            catch (Exception)
            {
                throw new FileLoadingError();
            }
        }


        public static void SaveListToXMLSerializer<T>(List<T?> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception)
            {
                throw new FileSavingError();
            }
        }
    }
}
