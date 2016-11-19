#define DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLP_Project;

namespace ClassNLPproj
{
    class Program
    {
        static void Main(string[] args)
        {
            NLPprocess nlpp = new NLPprocess();
            nlpp.Processing("./neg", "./pos", "./test_set");
            Console.WriteLine("Finished.");
            Console.ReadKey();
        }
    }

    class NLPprocess
    {
        public void Processing(string folder1, string folder2, string folder3)
        {
            StreamWriter swt = new StreamWriter("./svm-negation_test.txt");
            string folderName = System.Windows.Forms.Application.StartupPath + @folder1;
            int number = 1;
            NLP nlp = new NLP();
            int count1 = 0, count2 = 0;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            Dictionary<string, int> dict2 = new Dictionary<string, int>();
            Dictionary<string, int> dicts = new Dictionary<string, int>();

            for (int i = 0; i < 3; i++)
            {
                foreach (string fname in System.IO.Directory.GetFiles(folderName))
                {
                    string line, neggram = "";
                    System.IO.StreamReader file = new System.IO.StreamReader(fname);
                    while ((line = file.ReadLine()) != null)
                    {
                        bool neg = false;
                        string[] sents = nlp.SentDetect(line.Trim());
                        string[] tokens, tokens_reviews;
                        foreach (string sent in sents)
                        {
                            string pretok = "";
                            tokens = nlp.Tokenize(sent);//tokenize sentences
                            tokens_reviews = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(tokens)));
                            foreach (string token in tokens_reviews)
                            {
                                if (nlp.IsNegation(token))
                                {
                                    neggram = pretok + token;
                                    neg = true;
                                    if (i == 0)
                                    {
                                        count1++;
                                        if (dict1.ContainsKey(neggram))
                                            dict1[neggram]++;
                                        else
                                            dict1.Add(neggram, 1);
                                    }
                                    else if (i == 1)
                                    {
                                        count2++;
                                        if (dict2.ContainsKey(neggram))
                                            dict2[neggram]++;
                                        else
                                            dict2.Add(neggram, 1);
                                    }
                                    if (dict.ContainsKey(neggram))
                                        dict[neggram]++;
                                    else
                                        dict.Add(neggram, 1);
                                    if (dic.ContainsKey(neggram))
                                        continue;
                                    else
                                        dic.Add(neggram, number++);
                                    neggram = token;
                                    continue;
                                }
                                pretok = token;
                                if (i == 0)
                                {
                                    if (neg)
                                    {
                                        neggram = neggram + token;
                                        count1++;
                                        if (dict1.ContainsKey(neggram))
                                            dict1[neggram]++;
                                        else
                                            dict1.Add(neggram, 1);
                                        if (dict.ContainsKey(neggram))
                                            dict[neggram]++;
                                        else
                                            dict.Add(neggram, 1);
                                        if (dic.ContainsKey(neggram))
                                            continue;
                                        else
                                            dic.Add(neggram, number++);
                                        neg = false;
                                    }
                                    else
                                    {
                                        count1++;
                                        if (dict1.ContainsKey(token))
                                            dict1[token]++;
                                        else
                                            dict1.Add(token, 1);
                                        if (dict.ContainsKey(token))
                                            dict[token]++;
                                        else
                                            dict.Add(token, 1);
                                        if (dic.ContainsKey(token))
                                            continue;
                                        else
                                            dic.Add(token, number++);
                                    }
                                }
                                else if (i == 1)
                                {
                                    if (neg)
                                    {
                                        neggram = neggram + token;
                                        count2++;
                                        if (dict2.ContainsKey(neggram))
                                            dict2[neggram]++;
                                        else
                                            dict2.Add(neggram, 1);
                                        if (dict.ContainsKey(neggram))
                                            dict[neggram]++;
                                        else
                                            dict.Add(neggram, 1);
                                        if (dic.ContainsKey(neggram))
                                            continue;
                                        else
                                            dic.Add(neggram, number++);
                                        neg = false;
                                    }
                                    else
                                    {
                                        count2++;
                                        if (dict2.ContainsKey(token))
                                            dict2[token]++;
                                        else
                                            dict2.Add(token, 1);
                                        if (dict.ContainsKey(token))
                                            dict[token]++;
                                        else
                                            dict.Add(token, 1);
                                        if (dic.ContainsKey(token))
                                            continue;
                                        else
                                            dic.Add(token, number++);
                                    }
                                }
                                else
                                {
                                    if (neg)
                                    {
                                        neggram = neggram + token;
                                        if (dict.ContainsKey(neggram))
                                            dict[neggram]++;
                                        else
                                            dict.Add(neggram, 1);
                                        if (dic.ContainsKey(neggram))
                                            continue;
                                        else
                                            dic.Add(neggram, number++);
                                        neg = false;
                                    }
                                    else
                                    {
                                        if (dict.ContainsKey(token))
                                            dict[token]++;
                                        else
                                            dict.Add(token, 1);
                                        if (dic.ContainsKey(token))
                                            continue;
                                        else
                                            dic.Add(token, number++);
                                    }
                                }
                            }
                        }
                    }
                    /*if(System.IO.Directory.GetFiles(folderName).Last() == fname)
                        file.Close();*/
                }
                if (i == 0)
                    folderName = System.Windows.Forms.Application.StartupPath + @folder2;
                else if (i == 1)
                    folderName = System.Windows.Forms.Application.StartupPath + @folder3;
            }
            foreach (string fname in System.IO.Directory.GetFiles(folderName))
            {
                int tcount = 0;
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(fname);
                swt.Write("+1 ");
                while ((line = file.ReadLine()) != null)
                {
                    swt.Flush();
                    string[] sents = nlp.SentDetect(line.Trim());
                    string[] tokens, tokens_reviews;
                    foreach (string sent in sents)
                    {
                        tokens = nlp.Tokenize(sent);//tokenize sentences
                        tokens_reviews = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(tokens)));
                        foreach (string token in tokens_reviews)
                        {
                            tcount++;
                            if (!dicts.ContainsKey(token))
                                dicts.Add(token, 1);
                            else
                                dicts[token]++;
                        }
                    }
                }
                foreach (KeyValuePair<string, int> kvp in dicts)
                {
                    if (!dict1.ContainsKey(kvp.Key) || !dict2.ContainsKey(kvp.Key) || (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2)))) < 0))
                        continue;
                    else
                        swt.Write(dic[kvp.Key] + ":" + (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2))))));
                    swt.Flush();
                    swt.Write(" ");
                    swt.Flush();
                }
                swt.WriteLine("");
                swt.Flush();
                if (System.IO.Directory.GetFiles(folderName).Last() == fname)
                    file.Close();
                dicts.Clear();
            }
            swt.Close();
        }
    }
}
