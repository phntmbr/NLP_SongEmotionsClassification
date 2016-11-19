using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTranslation
{
    public partial class Form1 : Form
    {
        public static string InputFolder = @"C:\Users\Ian Hsieh\Downloads\Project Testsite\WordLyrics_test\";
        public static string OutputFolder = @"C:\Users\Ian Hsieh\Downloads\Project Testsite\TransLyrics_test\";
        public static List<string> FilepathList;
        public Form1()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = webBrowser1.Url.ToString();
            if (url == "https://translate.google.com.tw/?tr=f&hl=zh-TW&tab=wT#ja/en/")
            {
                if (FilepathList == null)
                {
                    FilepathList = new List<string>(Directory.GetFiles(InputFolder));
                }
                Populate().ContinueWith((_) =>
                {
                }, TaskScheduler.FromCurrentSynchronizationContext());
                webBrowser1.Document.GetElementById("gt-submit").Focus();
                SendKeys.Send("{Enter}");
            }
            else if (url == "https://translate.googleusercontent.com/translate_f")
            {
                string content = webBrowser1.DocumentText;
                content = content.Replace("</pre>", "").Replace("<pre>", "");
                if (!Directory.Exists(OutputFolder))
                {
                    Directory.CreateDirectory(OutputFolder);
                }
                File.WriteAllText(OutputFolder + Path.GetFileName(FilepathList[0]), content);
                FilepathList.RemoveAt(0);
                if (FilepathList.Count == 0)
                {
                    MessageBox.Show("Finish!");
                }
                
            }
        }

        async Task PopulateInputFile(HtmlElement element)
        {
            element.Focus();

            // delay the execution of SendKey to let the Choose File dialog show up
            var sendKeyTask = Task.Delay(500).ContinueWith((_) =>
            {
                // this gets executed when the dialog is visible
                SendKeys.Send(FilepathList[0] + "{ENTER}");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            element.InvokeMember("Click"); // this shows up the dialog

            await sendKeyTask;

            // delay continuation to let the Choose File dialog hide
            await Task.Delay(500);
        }

        async Task Populate()
        {
            var elements = webBrowser1.Document.GetElementsByTagName("input");
            foreach (HtmlElement element in elements)
            {
                if (element.GetAttribute("name") == "file")
                {
                    element.Focus();
                    await PopulateInputFile(element);
                }
            }
        }
    }
}
