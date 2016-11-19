using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP_project_combine
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            string firstResult = "svm-s_test-sc.out"; // svm-s_test-sc.out
            string firstNames = "shifted_file_test"; // shifted_file_test

            string secondResult = "svm-u_test-sc.out";
            string secondNames = "unshifted_file_test";

            StreamReader freReader = new StreamReader(@".\" + firstResult);
            StreamReader fnaReader = new StreamReader(@".\" + firstNames + ".txt");
            StreamReader sreReader = new StreamReader(@".\" + secondResult);
            StreamReader snaReader = new StreamReader(@".\" + secondNames + ".txt");
            StreamWriter sw = new StreamWriter(@".\" + firstResult + "_result.txt");

            int[] result = new int[400];
            bool[] last = new bool[400]; // true is postive, false is negative
            
            string[] files = Directory.GetFiles(@".\test_set", "*.txt");
            Console.WriteLine(files.Length);
            Dictionary<string, int> list = new Dictionary<string, int>();
            int count = 0;
            for (int i = 0; i < files.Length; i++)
            {
                if (!list.ContainsKey(files[i].Substring(16)))
                {
                    list.Add(files[i].Substring(16), count);
                    count++;
                }
            }
            Console.WriteLine("dict total=" + count);

            count = 0;
            while (!fnaReader.EndOfStream)
            {
                string line = fnaReader.ReadLine();
                int temp = Convert.ToInt32(freReader.ReadLine());
                count++;

                result[list[line]] += temp;
                if(temp >= 0)
                {
                    last[list[line]] = true;
                }
                else
                {
                    last[list[line]] = false;
                }
            }
            Console.WriteLine("total lines=" + count);

            count = 0;
            while (!snaReader.EndOfStream)
            {
                string line = snaReader.ReadLine();
                int temp = Convert.ToInt32(sreReader.ReadLine());
                count++;

                result[list[line]] += temp;
            }
            Console.WriteLine("total lines=" + count);

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i].Substring(16);
                string re;
                if (result[list[fileName]] > 0)
                {
                    re = "pos";
                }
                else if (result[list[fileName]] < 0)
                {
                    re = "neg";
                }
                else
                {
                    if (last[list[fileName]])
                    {
                        re = "pos";
                    }
                    else
                    {
                        re = "neg";
                    }
                }

                sw.WriteLine(fileName + "\t" + re);
            }

            //Console.ReadKey();

            sw.Close();
            freReader.Close();
            fnaReader.Close();
            sreReader.Close();
            snaReader.Close();
        }
        */


        static void Main(string[] args)
        {
            string firstResult = "svm_B_test-sc.out";
            StreamReader freReader = new StreamReader(@".\" + firstResult);
            StreamWriter sw = new StreamWriter(@".\" + firstResult + "_result.txt");

            string[] files = Directory.GetFiles(@".\test_set", "*.txt");
            for(int i = 0; i < files.Length; i++)
            {
                string re = "";
                int temp = Convert.ToInt32(freReader.ReadLine());
                if (temp >= 0)
                {
                    re = "pos";
                }
                else if (temp < 0)
                {
                    re = "neg";
                }
                sw.WriteLine(files[i].Substring(16) + "\t" + re);
            }
            freReader.Close();
            sw.Close();
        }
    }
}
