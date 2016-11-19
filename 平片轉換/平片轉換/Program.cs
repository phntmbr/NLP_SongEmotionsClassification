using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace 平片轉換
{
    class Program
    {
        private static string answer;
        static void Main(string[] args)
        {
            string[] context = File.ReadAllLines("./石井竜也_RIVER.txt", Encoding.UTF8);
            StreamWriter sw = new StreamWriter("./石井竜也_RIVER_after.txt");
            foreach(string s in context)
            {
                string temp = tran(s);
                sw.WriteLine(temp);
            }
            sw.Close();
        }
        static string tran(string line)
        {
            answer = "";
            foreach(char c in line)
            {
                if (c >= 'ぁ' && c <= 'ゖ')
                {
                    answer += (char)(c - 'ぁ' + 'ァ');
                    Console.WriteLine((char) (c - 'ぁ' + 'ァ'));
                }
                else if (c >= 'ァ' && c <= 'ヶ')
                {
                    answer += (char)(c - 'ァ' + 'ぁ');
                    Console.WriteLine((char)(c - 'ァ' + 'ぁ'));
                }
                else
                    answer += c;
            }
            return answer;
        }
    }
}
