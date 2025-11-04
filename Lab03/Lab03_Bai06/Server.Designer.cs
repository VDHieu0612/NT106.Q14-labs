namespace Lab03_Bai06
{
    partial class Server
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
            this.dataBox = new System.Windows.Forms.TextBox();
            this.serverListen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dataBox
            // 
            this.dataBox.Location = new System.Drawing.Point(44, 69);
            this.dataBox.Multiline = true;
            this.dataBox.Name = "dataBox";
            this.dataBox.Size = new System.Drawing.Size(697, 301);
            this.dataBox.TabIndex = 0;
            // 
            // serverListen
            // 
            this.serverListen.Location = new System.Drawing.Point(616, 18);
            this.serverListen.Name = "serverListen";
            this.serverListen.Size = new System.Drawing.Size(124, 37);
            this.serverListen.TabIndex = 1;
            this.serverListen.Text = "Listen";
            this.serverListen.UseVisualStyleBackColor = true;
            this.serverListen.Click += new System.EventHandler(this.serverListen_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.serverListen);
            this.Controls.Add(this.dataBox);
            this.Name = "Server";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dataBox;
        private System.Windows.Forms.Button serverListen;
    }
}

