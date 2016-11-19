using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLP_Project;

namespace NLP_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            process p = new process();
            p.Nlp(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\TransLyricsTest");
            Console.WriteLine("Finished.");
            Console.ReadKey();
        }
    }

    class process
    {
        public void Nlp(string folderName)
        {
            NLP nlp = new NLP();
            StreamReader srP = new StreamReader(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\SenticDic\positive-words.txt");
            StreamReader srN = new StreamReader(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\SenticDic\negative-words.txt");
            StreamReader srGA = new StreamReader(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\Gold_answer_test.txt");
            StreamReader srSN = new StreamReader(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\Songnames_test.txt");
            string[] pw = new string[2048];
            string[] nw = new string[5000];
            Dictionary<string, string> answer = new Dictionary<string, string>();
            int i = 0;
            string line, line1;
            while ((line = srP.ReadLine()) != null){
                pw[i++] = nlp.Lemmatization(nlp.Stem(line));
            }
            i = 0;
            while ((line = srN.ReadLine()) != null)
            {
                nw[i++] = nlp.Lemmatization(nlp.Stem(line));
            }
            i = 0;
            while((line = srGA.ReadLine()) != null && (line1 = srSN.ReadLine()) != null)
            {
                //if (line == "1" || line == "4")
                    //answer.Add(line1.Replace('?', ' ').Replace(':', ' '), "1");
                //else if (line == "3" || line == "2")
                    answer.Add(line1.Replace('?', ' ').Replace(':', ' '), line);
            }
            srP.Close();
            srN.Close();
            srGA.Close();
            srSN.Close();

            StreamWriter sw = new StreamWriter(@"C:\Users\Ian Hsieh\Downloads\Project Testsite\MIR_TF_test.txt");
            int number = 1, tv, no;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            List<string> avg = new List<string>();

            foreach (string fileName in System.IO.Directory.GetFiles(folderName))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                while ((line = file.ReadLine()) != null)
                {
                    string[] sents = nlp.SentDetect(line.Trim());
                    string[] tokens, tokens_reviews;
                    foreach (string sent in sents)
                    {
                        if (sent.Contains('('))
                            sent.Replace('(', ' ');
                        if (sent.Contains(')'))
                            sent.Replace(')', ' ');
                        if (sent.Contains('!'))
                            sent.Replace('!', ' ');
                        if (sent.Contains('#'))
                            sent.Replace('#', ' ');
                        if (sent.Contains('&'))
                            sent.Replace('&', ' ');
                        if (sent.Contains('*'))
                            sent.Replace('*', ' ');
                        if (sent.Contains(','))
                            sent.Replace(',', ' ');
                        if (sent.Contains('.'))
                            sent.Replace('.', ' ');
                        if (sent.Contains(':'))
                            sent.Replace(':', ' ');
                        if (sent.Contains(';'))
                            sent.Replace(';', ' ');
                        if (sent.Contains('?'))
                            sent.Replace('?', ' ');
                        if (sent.Contains('"'))
                            sent.Replace('"', ' ');
                        if (sent.Contains('\\'))
                            sent.Replace('\\', ' ');
                        if (sent.Contains('/'))
                            sent.Replace('/', ' ');
                        if (sent.Contains('-'))
                            sent.Replace('-', ' ');
                        if (sent == " ")
                            continue;
                        tokens = nlp.Tokenize(sent);//tokenize sentences
                        tokens_reviews = nlp.Lemmatization(nlp.Stem(nlp.FilterOutStopWords(tokens)));
                        foreach (string token in tokens_reviews)
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
                /*if(System.IO.Directory.GetFiles(folderName).Last() == fname)
                    file.Close();*/
                sw.Write("+" + answer[fileName.Substring(fileName.IndexOf('_') + 1, (fileName.IndexOf(".txt") - fileName.IndexOf('_') - 1))] + " ");
                tv = 0;
                no = 0;
                foreach (KeyValuePair<string, int> item in dict)
                {
                    int value = 1;//0;
                    /*foreach (string word in pw)
                    {
                        if (word == item.Key)
                        {
                            value = 1;
                            break;
                        }
                    }
                    foreach(string word in nw)
                    {
                        if (word == item.Key)
                        {
                            value = -1;
                            break;
                        }
                    }
                    if (value == 0 && !avg.Contains(item.Key))
                    {
                        avg.Add(item.Key);
                        continue;
                    }*/
                    Console.WriteLine(dic[item.Key] + ":" + (item.Value * value) + "\n");
                    sw.Write(dic[item.Key] + ":" + (item.Value * value) + " ");
                    tv += item.Value*value;
                    no += item.Value;
                }
                /*foreach (string str in avg)
                {
                    Console.WriteLine(dic[str] + ":" + ((double)dict[str] * (double)tv / (double)no) + "\n");
                    sw.Write(dic[str] + ":" + ((double)dict[str] * (double)tv / (double)no) + " ");
                }*/
                sw.WriteLine();
                dict.Clear();
                avg.Clear();  
            }
            sw.Close();
        }
    }
}
