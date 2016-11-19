using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XML_processing
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("input_all.xml");

            XmlNodeList nodes = doc.SelectNodes("RDF/Description/sentence");

            foreach (XmlNode n in nodes)
            {
                Console.WriteLine(n["text"].InnerText);
                Console.WriteLine(n["polarity"].InnerText);

                Console.ReadKey();
            }

        }
    }
}
