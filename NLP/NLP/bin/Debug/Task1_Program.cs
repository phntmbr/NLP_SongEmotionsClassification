using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using opennlp.tools.parser;
using LemmaSharp;

namespace NLP_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            NLP nlp = new NLP();
            XmlDocument doc = new XmlDocument();
            doc.Load("training.xml");

            XmlNodeList nodes = doc.SelectNodes("RDF/Text");
            Dictionary<string, int> dict = new Dictionary<string, int>();
            char[] separator = {' ', ',', '.', '?', '!'};
            int tokno = 1;
            StreamWriter sw = new StreamWriter("./Task1op.txt");
            foreach (XmlNode n in nodes)
            {
                string text = n.InnerText ;
                string[] sents = nlp.SentDetect(text);
                string p = n.Attributes["category"].Value;
                int polar = 0;
                if (p == "book")
                    polar = 1;
                else if (p == "dvd")
                    polar = 2;
                else if (p == "health")
                    polar = 3;
                else if (p == "music")
                    polar = 4;
                else if (p == "toys_games")
                    polar = 5;
                sw.Write("+" + polar.ToString() + " ");
                sw.Flush();
                foreach (string sent in sents)
                {
                    string s = sent.ToLower();
                    string[] token = nlp.Tokenize(s);   
                    foreach (string t in token)
                    {
                        string lemma = nlp.Lemmatization(t);
                        if (nlp.IsStopWord(lemma))
                            continue;
                        else
                        {
                            if (!dict.ContainsKey(lemma))
                            {
                                dict.Add(lemma, tokno);
                                tokno++;
                            }
                            sw.Write(dict[lemma] + ":1 ");
                            sw.Flush();
                        }
                    }
                }
                sw.WriteLine("");
                sw.Flush();
            }
            Console.WriteLine("Finished.");
            Console.ReadKey();
        }
    }
}
