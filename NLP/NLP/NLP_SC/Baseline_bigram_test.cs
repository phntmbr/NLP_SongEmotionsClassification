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
            StreamWriter sww = new StreamWriter("./svm_B.txt");
            StreamWriter swt = new StreamWriter("./svm_B_test.txt");
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
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader(fname);
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] sents = nlp.SentDetect(line.Trim());
                        string[] tokens, tokens_reviews;
                        Tuple<string, string>[] bigram;
                        foreach (string sent in sents)
                        {
                            tokens = nlp.Tokenize(sent);//tokenize sentences
                            bigram = nlp.Bigrams(nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(tokens))));
                            tokens_reviews = new string[bigram.Length];
                            int k = 0;
                            foreach (Tuple<string, string> tpl in bigram)
                                tokens_reviews[k++] = tpl.Item1 + tpl.Item2;
                            foreach (string token in tokens_reviews)
                            {
                                if (i == 0)
                                {
                                    count1++;
                                    if (dict1.ContainsKey(token))
                                        dict1[token]++;
                                    else
                                        dict1.Add(token, 1);
                                }
                                else if (i == 1)
                                {
                                    count2++;
                                    if (dict2.ContainsKey(token))
                                        dict2[token]++;
                                    else
                                        dict2.Add(token, 1);
                                }
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
                    /*if(System.IO.Directory.GetFiles(folderName).Last() == fname)
                        file.Close();*/
                }
                if (i == 0)
                    folderName = System.Windows.Forms.Application.StartupPath + @folder2;
                else if (i == 1)
                    folderName = System.Windows.Forms.Application.StartupPath + @folder3;
                foreach (KeyValuePair<string, int> item in dict)
                {
                    Console.WriteLine(item.Key + " " + item.Value);
                }
            }
            folderName = System.Windows.Forms.Application.StartupPath + @folder1;
            for (int i = 0; i < 3; i++)
            {
                foreach (string fname in System.IO.Directory.GetFiles(folderName))
                {
                    int tcount = 0;
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader(fname);
                    if (i == 0)
                        sww.Write("-1 ");
                    else if (i == 1)
                        sww.Write("+1 ");
                    else
                        swt.Write("+1 ");
                    while ((line = file.ReadLine()) != null)
                    {
                        if (i < 2)
                            sww.Flush();
                        else
                            swt.Flush();
                        string[] sents = nlp.SentDetect(line.Trim());
                        string[] tokens, tokens_reviews;
                        Tuple<string, string>[] bigram;
                        foreach (string sent in sents)
                        {
                            tokens = nlp.Tokenize(sent);//tokenize sentences
                            bigram = nlp.Bigrams(nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(tokens))));
                            tokens_reviews = new string[bigram.Length];
                            int k = 0;
                            foreach (Tuple<string, string> tpl in bigram)
                                tokens_reviews[k++] = tpl.Item1 + tpl.Item2;
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
                        if (i == 0)
                        {
                            if (!dict2.ContainsKey(kvp.Key) || (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict2[kvp.Key]) / (double)(count2)))) < 0))
                                continue;
                            else
                                sww.Write(dic[kvp.Key] + ":" + (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict2[kvp.Key]) / (double)(count2))))));
                        }
                        else if (i == 1)
                        {
                            if (!dict1.ContainsKey(kvp.Key) || (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key]) / (double)(count1)))) < 0))
                                continue;
                            else
                                sww.Write(dic[kvp.Key] + ":" + (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key]) / (double)(count1))))));
                        }
                        else
                        {
                            if (!dict1.ContainsKey(kvp.Key) || !dict2.ContainsKey(kvp.Key) || (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2)))) < 0))
                                continue;
                            else
                                swt.Write(dic[kvp.Key] + ":" + (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2))))));
                        }
                        if (i < 2)
                        {
                            sww.Flush();
                            sww.Write(" ");
                            sww.Flush();
                        }
                        else
                        {
                            swt.Flush();
                            swt.Write(" ");
                            swt.Flush();
                        }
                    }
                    if (i < 2)
                    {
                        sww.WriteLine("");
                        sww.Flush();
                    }
                    else
                    {
                        swt.WriteLine("");
                        swt.Flush();
                    }
                    if (System.IO.Directory.GetFiles(folderName).Last() == fname && i == 2)
                        file.Close();
                    dicts.Clear();
                }
                if (i == 0)
                    folderName = System.Windows.Forms.Application.StartupPath + @folder2;
                else if (i == 1)
                {
                    folderName = System.Windows.Forms.Application.StartupPath + @folder3;
                    sww.Close();
                }
                if (i == 2)
                    swt.Close();
            }
        }
    }
}
