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
            NLPprocess nlp = new NLPprocess();
            nlp.Processing();
            Console.WriteLine("Finished.");
            Console.ReadKey();
        }
    }

    class NLPprocess
    {
        public void Processing()
        {
            NLP nlp = new NLP();
            StreamWriter swt = new StreamWriter("./svm-s_test.txt");
            StreamWriter uwt = new StreamWriter("./svm-u_test.txt");
            StreamReader sr = new StreamReader("./shifted_sent.txt");
            StreamReader ur = new StreamReader("./unshifted_sent.txt");
            StreamReader sf = new StreamReader("./shifted_file.txt");
            StreamReader uf = new StreamReader("./unshifted_file.txt");
            StreamReader srt = new StreamReader("./shiftedsent_test.txt");
            StreamReader urt = new StreamReader("./unshiftedsent_test.txt");
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Dictionary<string, string> dicc = new Dictionary<string, string>();
            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            Dictionary<string, int> dict2 = new Dictionary<string, int>();
            Dictionary<string, int> dicts = new Dictionary<string, int>();
            int number = 1;
            int count1 = 0, count2 = 0;
            string line, file;
            string pol = "-1";

            while (((line = sr.ReadLine()) != null) && ((file = sf.ReadLine()) != null))
            {
                string[] tokenss;
                if (file.Contains("29590"))
                    pol = "+1";
                if (!dicc.ContainsKey(line))
                    dicc.Add(line, pol);
                tokenss = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(nlp.Tokenize(line))));
                foreach (string token in tokenss)
                {
                    if (pol == "-1")
                    {
                        count1++;
                        if (dict1.ContainsKey(token))
                            dict1[token]++;
                        else
                            dict1.Add(token, 1);
                        if (dic.ContainsKey(token))
                            continue;
                        else
                            dic.Add(token, number++);
                    }
                    else
                    {
                        count2++;
                        if (dict2.ContainsKey(token))
                            dict2[token]++;
                        else
                            dict2.Add(token, 1);
                        if (dic.ContainsKey(token))
                            continue;
                        else
                            dic.Add(token, number++);
                    }
                }
            }
            while (((line = srt.ReadLine()) != null))
            {
                string[] tokenss;
                if (!dicc.ContainsKey(line))
                    dicc.Add(line, pol);
                tokenss = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(nlp.Tokenize(line))));
                foreach (string token in tokenss)
                {
                    if (dic.ContainsKey(token))
                        continue;
                    else
                        dic.Add(token, number++);
                }
            }
            int tcount;
            srt = new StreamReader("./shiftedsent_test.txt");
            while ((line = srt.ReadLine()) != null)
            {
                swt.Write("+1 ");
                swt.Flush();
                tcount = 0;
                string[] tokens;
                tokens = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(nlp.Tokenize(line))));
                foreach (string token in tokens)
                {
                    tcount++;
                    if (!dicts.ContainsKey(token))
                        dicts.Add(token, 1);
                    else
                        dicts[token]++;
                }
                foreach (KeyValuePair<string, int> kvp in dicts)
                {
                    if (!dict1.ContainsKey(kvp.Key) || !dict2.ContainsKey(kvp.Key) || (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2)))) < 0))
                        continue;
                    else
                        swt.Write(dic[kvp.Key] + ":" + (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2))))) + " ");
                    swt.Flush();
                }
                swt.WriteLine();
                swt.Flush();
                dicts.Clear();
            }
            srt.Close();
            swt.Close();
            sr.Close();
            sf.Close();
            dic.Clear();
            dict1.Clear();
            dict2.Clear();
            pol = "-1";
            number = 1;
            count1 = 0;
            count2 = 0;
            while (((line = ur.ReadLine()) != null) && ((file = uf.ReadLine()) != null))
            {
                string[] tokenfs;
                if (file.Contains("29590"))
                    pol = "+1";
                if (!dicc.ContainsKey(line))
                    dicc.Add(line, pol);
                tokenfs = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(nlp.Tokenize(line))));
                foreach (string token in tokenfs)
                {
                    if (pol == "-1")
                    {
                        count1++;
                        if (dict1.ContainsKey(token))
                            dict1[token]++;
                        else
                            dict1.Add(token, 1);
                        if (dic.ContainsKey(token))
                            continue;
                        else
                            dic.Add(token, number++);
                    }
                    else
                    {
                        count2++;
                        if (dict2.ContainsKey(token))
                            dict2[token]++;
                        else
                            dict2.Add(token, 1);
                        if (dic.ContainsKey(token))
                            continue;
                        else
                            dic.Add(token, number++);
                    }
                }
            }
            while (((line = urt.ReadLine()) != null))
            {
                string[] tokenss;
                if (!dicc.ContainsKey(line))
                    dicc.Add(line, pol);
                tokenss = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(nlp.Tokenize(line))));
                foreach (string token in tokenss)
                {
                    if (dic.ContainsKey(token))
                        continue;
                    else
                        dic.Add(token, number++);
                }
            }
            urt = new StreamReader("./unshiftedsent_test.txt");
            while ((line = urt.ReadLine()) != null)
            {
                tcount = 0;
                uwt.Write("+1 ");
                uwt.Flush();
                string[] tokens;
                tokens = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(nlp.Tokenize(line))));
                foreach (string token in tokens)
                {
                    tcount++;
                    if (!dicts.ContainsKey(token))
                        dicts.Add(token, 1);
                    else
                        dicts[token]++;
                }
                foreach (KeyValuePair<string, int> kvp in dicts)
                {
                    if (!dict2.ContainsKey(kvp.Key) || !dict1.ContainsKey(kvp.Key) || (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2)))) < 0))
                        continue;
                    else
                        uwt.Write(dic[kvp.Key] + ":" + (Math.Log((double)(((double)(kvp.Value) / (double)(tcount)) / ((double)(dict1[kvp.Key] + dict2[kvp.Key]) / (double)(count1 + count2))))) + " ");
                    uwt.Flush();
                }
                uwt.WriteLine();
                uwt.Flush();
                dicts.Clear();
            }
            urt.Close();
            uwt.Close();
            ur.Close();
            uf.Close();
        }
    }
}
