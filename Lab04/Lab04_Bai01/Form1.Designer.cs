namespace Lab04_Bai01
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblURL = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(20, 23);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(34, 13);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "URL:";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(70, 20);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(600, 20);
            this.txtURL.TabIndex = 1;
            this.txtURL.Text = "https://example.com";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(680, 18);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(100, 25);
            this.btnGet.TabIndex = 2;
            this.btnGet.Text = "GET";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // rtbContent
            // 
            this.rtbContent.Location = new System.Drawing.Point(20, 60);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.ReadOnly = true;
            this.rtbContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.rtbContent.Size = new System.Drawing.Size(840, 480);
            this.rtbContent.TabIndex = 3;
            this.rtbContent.Text = "";
            this.rtbContent.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.rtbContent);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.lblURL);
            this.Name = "Form1";
            this.Text = "Lab04_Bai01 - Hiển thị nội dung HTML";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.RichTextBox rtbContent;
    }
}