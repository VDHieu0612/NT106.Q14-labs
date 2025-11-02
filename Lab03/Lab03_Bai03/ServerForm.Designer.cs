namespace Lab03_Bai03
{
    partial class ServerForm
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
            this.btnListen = new System.Windows.Forms.Button();
            this.rtbListen = new System.Windows.Forms.RichTextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(604, 27);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(172, 45);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // rtbListen
            // 
            this.rtbListen.Location = new System.Drawing.Point(30, 96);
            this.rtbListen.Name = "rtbListen";
            this.rtbListen.ReadOnly = true;
            this.rtbListen.Size = new System.Drawing.Size(745, 339);
            this.rtbListen.TabIndex = 1;
            this.rtbListen.Text = "";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(30, 27);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(172, 45);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 460);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.rtbListen);
            this.Controls.Add(this.btnListen);
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.RichTextBox rtbListen;
        private System.Windows.Forms.Button btnStop;
    }
}