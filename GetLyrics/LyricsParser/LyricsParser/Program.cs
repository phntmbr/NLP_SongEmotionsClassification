using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace LyricsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            ParsingKasiHtml();
            //RemoveEmptyFile();
            Console.WriteLine("Finished.");
            Console.ReadKey();
        }

        static void RemoveEmptyFile()
        {
            string[] filepaths = Directory.GetFiles("C:\\Users\\Ian Hsieh\\Downloads\\Project Testsite\\Lyrics_test\\");
            foreach (string path in filepaths)
            {
                string lyrics = File.ReadAllText(path);
                if (lyrics == "" || lyrics == " " || lyrics == "\n")
                {
                    File.Delete(path);
                }
            }
        }

        static void ParsingKasiHtml()
        {
            int count_no_lyrics = 0;
            int count_all_songs = 0;
            FileInfo f1 = new FileInfo("C:\\Users\\Ian Hsieh\\Downloads\\Project Testsite\\Singers_test.txt");
            StreamReader sr1 = f1.OpenText();
            FileInfo f2 = new FileInfo("C:\\Users\\Ian Hsieh\\Downloads\\Project Testsite\\Songnames_test.txt");
            StreamReader sr2 = f2.OpenText();
            FileInfo f4 = new FileInfo("C:\\Users\\Ian Hsieh\\Downloads\\Project Testsite\\No._of_Songs_test.txt");
            StreamWriter sw2 = f4.CreateText();

            if (File.Exists(@".\Error Log.txt"))
            {
                File.Delete(@".\Error Log.txt");
            }

            while (sr1.Peek() >= 0)
            {
                //Console.WriteLine("{0}\t{1}", sr1.ReadLine(), sr2.ReadLine());
                string singer = sr1.ReadLine();
                string songname = sr2.ReadLine();

                string datapath = "C:\\Users\\Ian Hsieh\\Downloads\\Project Testsite\\Lyrics_test\\" + singer.Replace(':', ' ').Replace('?', ' ') + "_" + songname.Replace(':', ' ').Replace('?', ' ') + ".txt";
                count_all_songs++;

                if (!File.Exists(datapath)) //  功能:不重複抓歌詞
                {
                    string link = GetSongLink(singer, songname);

                    if (link == "")
                    {
                        StreamWriter sw = new StreamWriter(@".\Error Log.txt", true); //錯誤記錄在Bin/Debug/裡面
                        sw.WriteLine(singer + "\t" + songname + "\t" + "沒有符合的結果");
                        Console.WriteLine(singer + "\t" + songname + "\t" + "沒有符合的結果");
                        sw.Close();
                        count_no_lyrics++;
                        continue;
                    }

                    string Lyrics = GetLyrics(link);

                    if (Lyrics == "")
                    {
                        StreamWriter sw = new StreamWriter(@".\Error Log.txt", true); //錯誤記錄在Bin/Debug/裡面
                        sw.WriteLine(singer + "\t" + songname + "\t" + "沒有歌詞");
                        Console.WriteLine(singer + "\t" + songname + "\t" + "沒有歌詞");
                        sw.Close();
                        continue;
                    }
                    FileInfo f3 = new FileInfo(datapath);
                    StreamWriter sw1 = f3.CreateText();
                    //Console.WriteLine(Lyrics, Encoding.GetEncoding("UTF-8"));
                    sw1.Write(Lyrics + "\n");//將歌詞內容開檔存下來
                    sw1.Close();
                }
            }
            sw2.Write("沒有歌詞的歌曲數量:{0}\r\n所有歌曲的數量:{1}\r\n", count_no_lyrics, count_all_songs);
            sw2.Close();
        }

        static string GetSongLink(string singer, string songname)
        {
            string sn = Regex.Replace(songname.Replace(@"-", " ").Trim().ToLower(), @"[\(].+?[\)]", "");
            WebClient client = new WebClient();
            MemoryStream ms = new MemoryStream(client.DownloadData(@"http://mojim.com/" + sn + ".html?t3f"));

            // 使用預設編碼讀入 HTML 
            HtmlDocument doc = new HtmlDocument();
            doc.Load(ms, Encoding.Default);

            // 裝載第一層查詢結果 
            HtmlDocument docStockContext = new HtmlDocument();

            docStockContext.LoadHtml(doc.DocumentNode.InnerHtml);
            docStockContext.LoadHtml(doc.DocumentNode.SelectSingleNode("/html/body/table/tbody/tr/td/table[4]/tbody/tr[1]/td[1]/table[@class='iB']").InnerHtml);

            // 輸出資料
            string linkUrl = "";
            string date = "";
            SortedList<string, string> list = new SortedList<string, string>();
            foreach (HtmlNode node in docStockContext.DocumentNode.SelectNodes("/tr"))
            {
                if (node.Attributes.Count != 0)
                {
                    if (node.Attributes.Count != 0 && (node.Attributes["bgcolor"].Value == "#FFFFFF" || node.Attributes["bgcolor"].Value == "#EFEFEF"))
                    {
                        if (singer.ToLower() == Regex.Replace(node.ChildNodes[3].InnerText, @"[\(（].+?[\)）]", "").ToLower() && songname.ToLower() == Regex.Replace(node.ChildNodes[7].InnerText.Split('.')[1], @"[\(（].+?[\)）]", "").ToLower())
                        {
                            linkUrl = node.ChildNodes[7].ChildNodes[0].Attributes["href"].Value;
                            date = node.ChildNodes[9].InnerText;
                            if (!list.ContainsKey(date))
                                list.Add(date, linkUrl);
                        }
                    }
                }
            }
            if (list.Count == 0 || singer == null)
            {
                foreach (HtmlNode node in docStockContext.DocumentNode.SelectNodes("/tr"))
                {
                    if (node.Attributes.Count != 0)
                    {
                        if (node.Attributes.Count != 0 && (node.Attributes["bgcolor"].Value == "#FFFFFF" || node.Attributes["bgcolor"].Value == "#EFEFEF"))
                        {
                            if (sn == Regex.Replace(node.ChildNodes[7].InnerText.Split('.')[1], @"[\(（].+?[\)）]", "").ToLower())
                            {
                                linkUrl = node.ChildNodes[7].ChildNodes[0].Attributes["href"].Value;
                                date = node.ChildNodes[9].InnerText;
                                if (!list.ContainsKey(date))
                                    list.Add(date, linkUrl);
                            }
                        }
                    }
                }
            }

            doc = null;
            docStockContext = null;
            client = null;
            ms.Close();

            if (list.Count > 0)
                return list.First().Value;
            else
                return "";
        }

        static string GetLyrics(string link)
        {
            WebClient client = new WebClient();
            MemoryStream ms = new MemoryStream(client.DownloadData(@"http://mojim.com" + link));

            // 使用預設編碼讀入 HTML 
            HtmlDocument doc = new HtmlDocument();
            doc.Load(ms, Encoding.Default);

            // 裝載第一層查詢結果 
            HtmlDocument docStockContext = new HtmlDocument();

            docStockContext.LoadHtml(doc.DocumentNode.SelectSingleNode("//div[@id='fsZ']//dd").InnerHtml);

            string text = docStockContext.DocumentNode.InnerHtml.Replace("<br>", "\n");
            text = text.Replace(@"更多更詳盡歌詞 在 <a href=""http://mojim.com"">※ Mojim.com　魔鏡歌詞網 </a>", "");
            //string lyrics = Regex.Replace(text, "\n[^\\[\n]+", "");

            return text;
        }

    }
}

