namespace Lab03_Bai06
{
    partial class ChatManager
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
            this.startServer = new System.Windows.Forms.Button();
            this.startClient1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startServer
            // 
            this.startServer.Location = new System.Drawing.Point(134, 179);
            this.startServer.Name = "startServer";
            this.startServer.Size = new System.Drawing.Size(150, 85);
            this.startServer.TabIndex = 0;
            this.startServer.Text = "Server";
            this.startServer.UseVisualStyleBackColor = true;
            this.startServer.Click += new System.EventHandler(this.startServer_Click);
            // 
            // startClient1
            // 
            this.startClient1.Location = new System.Drawing.Point(477, 179);
            this.startClient1.Name = "startClient1";
            this.startClient1.Size = new System.Drawing.Size(150, 85);
            this.startClient1.TabIndex = 2;
            this.startClient1.Text = "New Client";
            this.startClient1.UseVisualStyleBackColor = true;
            this.startClient1.Click += new System.EventHandler(this.startClient1_Click);
            // 
            // ChatManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.startClient1);
            this.Controls.Add(this.startServer);
            this.Name = "ChatManager";
            this.Text = "ChatManager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startServer;
        private System.Windows.Forms.Button startClient1;
    }
}