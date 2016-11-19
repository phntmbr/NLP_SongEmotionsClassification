using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LIBSVM_processing
{
    class Program
    {
        static void Main(string[] args)
        {

            StreamReader sr;
            /*
            StreamWriter sw = new StreamWriter("./books_total.txt");
            string pol;
            int num = 0;
            for (int i = 1; i <= 2; i++)
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();
                if (i == 2)
                {
                    sr = new StreamReader("./negative.review");
                    pol = "-1";
                }
                else
                {
                    sr = new StreamReader("./positive.review");
                    pol = "+1";
                }
                while (!sr.EndOfStream)
                {
                    string text = sr.ReadLine();
                    string[] c = text.Split(' ');
                    sw.Write(pol + " ");
                    sw.Flush();
                    foreach (string s in c)
                    {
                        if (s.Contains('#'))
                            continue;
                        int idx = s.IndexOf(":");
                        string word = s.Remove(idx);
                        if (!dict.ContainsKey(word))
                        {
                            dict.Add(word, ++num);
                            sw.Write(num + ":" + s.Substring(idx + 1) + " ");
                        }
                        else
                            sw.Write(dict[word] + ":" + s.Substring(idx + 1) + " ");
                        sw.Flush();
                    }
                    sw.WriteLine();
                    sw.Flush();
                }
                sw.Flush();
            }
            sw.Close();
			*/
            sr = new StreamReader(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\MIR_Baseline.txt");
            StreamWriter sw1 = new StreamWriter(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\MIR_Baseline_SVM.txt");
            List<string> genre1 = new List<string>();
            List<string> genre2 = new List<string>();
            List<string> genre3 = new List<string>();
            List<string> genre4 = new List<string>();
            while (!sr.EndOfStream)
            {
                string text = sr.ReadLine().TrimEnd();
                string[] c = text.Split(' ');
                if (c[0] == "+1")
                    genre1.Add(text);
                else if (c[0] == "+2")
                    genre2.Add(text);
                else if (c[0] == "+3")
                    genre3.Add(text);
                else
                    genre4.Add(text);
            }
            genre1.AddRange(genre2);
            genre1.AddRange(genre3);
            genre1.AddRange(genre4);
            foreach(string str in genre1)
            {
                string[] c = str.Split(' ');
                string[] r = new string[65536];
                int i = 0;
                int idx;
                int[] no = new int[65536];
                foreach (string s in c)
                {
                    r[i] = s + " ";
                    if (s.Contains('+') || s.Contains('-'))
                    {
                        i++;
                        continue;
                    }
                    idx = s.IndexOf(":");
                    string a;
                    if (idx == 1)
                        a = s.Substring(0, 1);
                    else
                        a = s.Substring(0, idx);
                    no[i] = Convert.ToInt32(a);
                    int z = i;
                    while ((z >= 1) && (no[z] < no[z - 1]))
                    {
                        int t;
                        string w;
                        t = no[z];
                        w = r[z];
                        no[z] = no[z - 1];
                        r[z] = r[z - 1];
                        no[z - 1] = t;
                        r[z - 1] = w;
                        z--;
                    }
                    i++;
                }
                foreach (string s in r)
                {
                    sw1.Write(s);
                    sw1.Flush();
                }
                sw1.WriteLine();
                sw1.Flush();
            }
            Console.Write("Finished.");
            Console.ReadKey();
        }
    }
}
