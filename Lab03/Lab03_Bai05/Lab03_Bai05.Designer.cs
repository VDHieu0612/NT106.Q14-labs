namespace Lab03_Bai05
{
    partial class Lab03_Bai05
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.btnTaoServer = new System.Windows.Forms.Button();
            this.btnTaoClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTaoServer
            // 
            this.btnTaoServer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoServer.Location = new System.Drawing.Point(54, 38);
            this.btnTaoServer.Name = "btnTaoServer";
            this.btnTaoServer.Size = new System.Drawing.Size(277, 65);
            this.btnTaoServer.TabIndex = 0;
            this.btnTaoServer.Text = "Khởi Động Server";
            this.btnTaoServer.UseVisualStyleBackColor = true;
            this.btnTaoServer.Click += new System.EventHandler(this.btnTaoServer_Click);
            // 
            // btnTaoClient
            // 
            this.btnTaoClient.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoClient.Location = new System.Drawing.Point(54, 128);
            this.btnTaoClient.Name = "btnTaoClient";
            this.btnTaoClient.Size = new System.Drawing.Size(277, 65);
            this.btnTaoClient.TabIndex = 1;
            this.btnTaoClient.Text = "Tạo Client Mới";
            this.btnTaoClient.UseVisualStyleBackColor = true;
            this.btnTaoClient.Click += new System.EventHandler(this.btnTaoClient_Click);
            // 
            // Lab03_Bai05
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 233);
            this.Controls.Add(this.btnTaoClient);
            this.Controls.Add(this.btnTaoServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Lab03_Bai05";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lab03_Bai05";
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Button btnTaoServer;
        private System.Windows.Forms.Button btnTaoClient;
    }
}