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
            StreamReader sr = new StreamReader("./svm-B_test-sc_result.txt");
            StreamReader sr1 = new StreamReader("./svm-neg_test-sc_result.txt");
            StreamReader sr2 = new StreamReader("./svm-us_test-sc_result.txt");
            StreamWriter sw = new StreamWriter("./svm-vote_test-sc.txt");
            string line, line1, line2;
            while ((line = sr.ReadLine()) != null && (line1 = sr1.ReadLine()) != null && (line2 = sr2.ReadLine()) != null)
            {
                int vote = 0;
                if (line.Contains("pos"))
                    vote++;
                if (line1.Contains("pos"))
                    vote++;
                if (line2.Contains("pos"))
                    vote++;
                if (vote >= 2)
                    sw.WriteLine(line.Remove(line.Length - 3) + "pos");
                else
                    sw.WriteLine(line.Remove(line.Length - 3) + "neg");
                sw.Flush();
            }
            sw.Close();
            Console.Write("Finished.");
            Console.ReadKey();
        }
    }
}
