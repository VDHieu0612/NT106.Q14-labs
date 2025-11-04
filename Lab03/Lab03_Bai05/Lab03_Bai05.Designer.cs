namespace Lab03_Bai05
{
    partial class Lab03_Bai05
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.btnTaoServer = new System.Windows.Forms.Button();
            this.btnTaoClient = new System.Windows.Forms.Button();
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTaoServer
            // 
            this.btnTaoServer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoServer.Location = new System.Drawing.Point(54, 38);
            this.btnTaoServer.Name = "btnTaoServer";
            this.btnTaoServer.Size = new System.Drawing.Size(375, 65);
            this.btnTaoServer.TabIndex = 0;
            this.btnTaoServer.Text = "Khởi Động Server";
            this.btnTaoServer.UseVisualStyleBackColor = true;
            this.btnTaoServer.Click += new System.EventHandler(this.btnTaoServer_Click);
            // 
            // btnTaoClient
            // 
            this.btnTaoClient.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoClient.Location = new System.Drawing.Point(54, 185);
            this.btnTaoClient.Name = "btnTaoClient";
            this.btnTaoClient.Size = new System.Drawing.Size(375, 65);
            this.btnTaoClient.TabIndex = 1;
            this.btnTaoClient.Text = "Tạo Client Mới";
            this.btnTaoClient.UseVisualStyleBackColor = true;
            this.btnTaoClient.Click += new System.EventHandler(this.btnTaoClient_Click);
            // 
            // txtServerIp
            // 
            this.txtServerIp.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerIp.Location = new System.Drawing.Point(149, 129);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(280, 34);
            this.txtServerIp.TabIndex = 2;
            this.txtServerIp.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(49, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 28);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP Server:";
            // 
            // Lab03_Bai05
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 283);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtServerIp);
            this.Controls.Add(this.btnTaoClient);
            this.Controls.Add(this.btnTaoServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Lab03_Bai05";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bảng Điều Khiển";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Button btnTaoServer;
        private System.Windows.Forms.Button btnTaoClient;
        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.Label label1;
    }
}