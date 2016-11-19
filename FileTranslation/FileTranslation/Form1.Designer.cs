using System;
using System.Threading;
namespace FileTranslation
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(1145, 526);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("https://translate.google.com.tw/?tr=f&hl=zh-TW&tab=wT#ja/en/", System.UriKind.Absolute);
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 526);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
             tcb = this.seturl;
            stateTimer = new Timer(tcb,null,5000,60000);
            

        }
        public void seturl(Object stateInfo)
        {
            //webBrowser1.Url = new System.Uri("https://translate.google.com.tw/?tr=f&hl=zh-TW&tab=wT#ja/en/", System.UriKind.Absolute);

            webBrowser1.Navigate("https://translate.google.com.tw/?tr=f&hl=zh-TW&tab=wT#ja/en/");
        }
        #endregion
        TimerCallback tcb = null;
        Timer stateTimer = null;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}

