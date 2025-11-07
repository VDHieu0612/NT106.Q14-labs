namespace Lab03_Bai05
{
    partial class ClientForm
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
            this.grThemMonAn = new System.Windows.Forms.GroupBox();
            this.btnChonAnh = new System.Windows.Forms.Button();
            this.IntxtFileAnh = new System.Windows.Forms.TextBox();
            this.btnThemMon = new System.Windows.Forms.Button();
            this.IntxtTenNguoiDongGop = new System.Windows.Forms.TextBox();
            this.IntxtTenMonAn = new System.Windows.Forms.TextBox();
            this.lbFileAnh = new System.Windows.Forms.Label();
            this.lbTenNguoiDongGop = new System.Windows.Forms.Label();
            this.lbTenMonAn = new System.Windows.Forms.Label();
            this.lsVCacMonAn = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grHomNayAnGi = new System.Windows.Forms.GroupBox();
            this.OutxtTenMonAn = new System.Windows.Forms.TextBox();
            this.OutxtTenNguoiDongGop = new System.Windows.Forms.TextBox();
            this.OutLbTenMonAN = new System.Windows.Forms.Label();
            this.OutLbTenNguoiDongGop = new System.Windows.Forms.Label();
            this.btnChonMon = new System.Windows.Forms.Button();
            this.picAnhMonAn = new System.Windows.Forms.PictureBox();
            this.grThemMonAn.SuspendLayout();
            this.grHomNayAnGi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAnhMonAn)).BeginInit();
            this.SuspendLayout();
            // 
            // grThemMonAn
            // 
            this.grThemMonAn.Controls.Add(this.btnChonAnh);
            this.grThemMonAn.Controls.Add(this.IntxtFileAnh);
            this.grThemMonAn.Controls.Add(this.btnThemMon);
            this.grThemMonAn.Controls.Add(this.IntxtTenNguoiDongGop);
            this.grThemMonAn.Controls.Add(this.IntxtTenMonAn);
            this.grThemMonAn.Controls.Add(this.lbFileAnh);
            this.grThemMonAn.Controls.Add(this.lbTenNguoiDongGop);
            this.grThemMonAn.Controls.Add(this.lbTenMonAn);
            this.grThemMonAn.Location = new System.Drawing.Point(18, 19);
            this.grThemMonAn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grThemMonAn.Name = "grThemMonAn";
            this.grThemMonAn.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grThemMonAn.Size = new System.Drawing.Size(651, 377);
            this.grThemMonAn.TabIndex = 0;
            this.grThemMonAn.TabStop = false;
            this.grThemMonAn.Text = "Thêm món ăn";
            // 
            // btnChonAnh
            // 
            this.btnChonAnh.Location = new System.Drawing.Point(516, 205);
            this.btnChonAnh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChonAnh.Name = "btnChonAnh";
            this.btnChonAnh.Size = new System.Drawing.Size(112, 36);
            this.btnChonAnh.TabIndex = 7;
            this.btnChonAnh.Text = "Chọn...";
            this.btnChonAnh.UseVisualStyleBackColor = true;
            this.btnChonAnh.Click += new System.EventHandler(this.btnChonAnh_Click);
            // 
            // IntxtFileAnh
            // 
            this.IntxtFileAnh.Location = new System.Drawing.Point(244, 206);
            this.IntxtFileAnh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IntxtFileAnh.Name = "IntxtFileAnh";
            this.IntxtFileAnh.ReadOnly = true;
            this.IntxtFileAnh.Size = new System.Drawing.Size(260, 31);
            this.IntxtFileAnh.TabIndex = 7;
            // 
            // btnThemMon
            // 
            this.btnThemMon.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemMon.Location = new System.Drawing.Point(244, 278);
            this.btnThemMon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnThemMon.Name = "btnThemMon";
            this.btnThemMon.Size = new System.Drawing.Size(384, 70);
            this.btnThemMon.TabIndex = 6;
            this.btnThemMon.Text = "Thêm Món";
            this.btnThemMon.UseVisualStyleBackColor = true;
            this.btnThemMon.Click += new System.EventHandler(this.btnThemMon_Click);
            // 
            // IntxtTenNguoiDongGop
            // 
            this.IntxtTenNguoiDongGop.Location = new System.Drawing.Point(244, 134);
            this.IntxtTenNguoiDongGop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IntxtTenNguoiDongGop.Name = "IntxtTenNguoiDongGop";
            this.IntxtTenNguoiDongGop.Size = new System.Drawing.Size(382, 31);
            this.IntxtTenNguoiDongGop.TabIndex = 4;
            // 
            // IntxtTenMonAn
            // 
            this.IntxtTenMonAn.Location = new System.Drawing.Point(244, 58);
            this.IntxtTenMonAn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IntxtTenMonAn.Name = "IntxtTenMonAn";
            this.IntxtTenMonAn.Size = new System.Drawing.Size(382, 31);
            this.IntxtTenMonAn.TabIndex = 3;
            // 
            // lbFileAnh
            // 
            this.lbFileAnh.AutoSize = true;
            this.lbFileAnh.Location = new System.Drawing.Point(26, 214);
            this.lbFileAnh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFileAnh.Name = "lbFileAnh";
            this.lbFileAnh.Size = new System.Drawing.Size(91, 25);
            this.lbFileAnh.TabIndex = 2;
            this.lbFileAnh.Text = "File Ảnh";
            // 
            // lbTenNguoiDongGop
            // 
            this.lbTenNguoiDongGop.AutoSize = true;
            this.lbTenNguoiDongGop.Location = new System.Drawing.Point(26, 139);
            this.lbTenNguoiDongGop.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTenNguoiDongGop.Name = "lbTenNguoiDongGop";
            this.lbTenNguoiDongGop.Size = new System.Drawing.Size(204, 25);
            this.lbTenNguoiDongGop.TabIndex = 1;
            this.lbTenNguoiDongGop.Text = "Tên người đóng góp";
            // 
            // lbTenMonAn
            // 
            this.lbTenMonAn.AutoSize = true;
            this.lbTenMonAn.Location = new System.Drawing.Point(26, 62);
            this.lbTenMonAn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTenMonAn.Name = "lbTenMonAn";
            this.lbTenMonAn.Size = new System.Drawing.Size(126, 25);
            this.lbTenMonAn.TabIndex = 0;
            this.lbTenMonAn.Text = "Tên món ăn";
            // 
            // lsVCacMonAn
            // 
            this.lsVCacMonAn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsVCacMonAn.HideSelection = false;
            this.lsVCacMonAn.Location = new System.Drawing.Point(678, 33);
            this.lsVCacMonAn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lsVCacMonAn.Name = "lsVCacMonAn";
            this.lsVCacMonAn.Size = new System.Drawing.Size(796, 360);
            this.lsVCacMonAn.TabIndex = 1;
            this.lsVCacMonAn.UseCompatibleStateImageBehavior = false;
            this.lsVCacMonAn.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên Món Ăn";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Người đóng góp";
            this.columnHeader2.Width = 150;
            // 
            // grHomNayAnGi
            // 
            this.grHomNayAnGi.Controls.Add(this.OutxtTenMonAn);
            this.grHomNayAnGi.Controls.Add(this.OutxtTenNguoiDongGop);
            this.grHomNayAnGi.Controls.Add(this.OutLbTenMonAN);
            this.grHomNayAnGi.Controls.Add(this.OutLbTenNguoiDongGop);
            this.grHomNayAnGi.Location = new System.Drawing.Point(18, 489);
            this.grHomNayAnGi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grHomNayAnGi.Name = "grHomNayAnGi";
            this.grHomNayAnGi.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grHomNayAnGi.Size = new System.Drawing.Size(651, 309);
            this.grHomNayAnGi.TabIndex = 2;
            this.grHomNayAnGi.TabStop = false;
            this.grHomNayAnGi.Text = "Hôm nay ăn gì?";
            // 
            // OutxtTenMonAn
            // 
            this.OutxtTenMonAn.Location = new System.Drawing.Point(244, 178);
            this.OutxtTenMonAn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutxtTenMonAn.Name = "OutxtTenMonAn";
            this.OutxtTenMonAn.ReadOnly = true;
            this.OutxtTenMonAn.Size = new System.Drawing.Size(382, 31);
            this.OutxtTenMonAn.TabIndex = 5;
            // 
            // OutxtTenNguoiDongGop
            // 
            this.OutxtTenNguoiDongGop.Location = new System.Drawing.Point(244, 100);
            this.OutxtTenNguoiDongGop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutxtTenNguoiDongGop.Name = "OutxtTenNguoiDongGop";
            this.OutxtTenNguoiDongGop.ReadOnly = true;
            this.OutxtTenNguoiDongGop.Size = new System.Drawing.Size(382, 31);
            this.OutxtTenNguoiDongGop.TabIndex = 3;
            // 
            // OutLbTenMonAN
            // 
            this.OutLbTenMonAN.AutoSize = true;
            this.OutLbTenMonAN.Location = new System.Drawing.Point(22, 183);
            this.OutLbTenMonAN.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OutLbTenMonAN.Name = "OutLbTenMonAN";
            this.OutLbTenMonAN.Size = new System.Drawing.Size(126, 25);
            this.OutLbTenMonAN.TabIndex = 2;
            this.OutLbTenMonAN.Text = "Tên món ăn";
            // 
            // OutLbTenNguoiDongGop
            // 
            this.OutLbTenNguoiDongGop.AutoSize = true;
            this.OutLbTenNguoiDongGop.Location = new System.Drawing.Point(22, 105);
            this.OutLbTenNguoiDongGop.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OutLbTenNguoiDongGop.Name = "OutLbTenNguoiDongGop";
            this.OutLbTenNguoiDongGop.Size = new System.Drawing.Size(204, 25);
            this.OutLbTenNguoiDongGop.TabIndex = 0;
            this.OutLbTenNguoiDongGop.Text = "Tên người đóng góp";
            // 
            // btnChonMon
            // 
            this.btnChonMon.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChonMon.Location = new System.Drawing.Point(18, 406);
            this.btnChonMon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChonMon.Name = "btnChonMon";
            this.btnChonMon.Size = new System.Drawing.Size(651, 73);
            this.btnChonMon.TabIndex = 6;
            this.btnChonMon.Text = "Tìm Món Ăn Ngẫu Nhiên";
            this.btnChonMon.UseVisualStyleBackColor = true;
            this.btnChonMon.Click += new System.EventHandler(this.btnChonMon_Click);
            // 
            // picAnhMonAn
            // 
            this.picAnhMonAn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAnhMonAn.Location = new System.Drawing.Point(678, 406);
            this.picAnhMonAn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picAnhMonAn.Name = "picAnhMonAn";
            this.picAnhMonAn.Size = new System.Drawing.Size(796, 391);
            this.picAnhMonAn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAnhMonAn.TabIndex = 7;
            this.picAnhMonAn.TabStop = false;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1506, 817);
            this.Controls.Add(this.picAnhMonAn);
            this.Controls.Add(this.btnChonMon);
            this.Controls.Add(this.grHomNayAnGi);
            this.Controls.Add(this.lsVCacMonAn);
            this.Controls.Add(this.grThemMonAn);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.grThemMonAn.ResumeLayout(false);
            this.grThemMonAn.PerformLayout();
            this.grHomNayAnGi.ResumeLayout(false);
            this.grHomNayAnGi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAnhMonAn)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.GroupBox grThemMonAn;
        private System.Windows.Forms.Button btnChonAnh;
        private System.Windows.Forms.TextBox IntxtFileAnh;
        private System.Windows.Forms.Button btnThemMon;
        private System.Windows.Forms.TextBox IntxtTenNguoiDongGop;
        private System.Windows.Forms.TextBox IntxtTenMonAn;
        private System.Windows.Forms.Label lbFileAnh;
        private System.Windows.Forms.Label lbTenNguoiDongGop;
        private System.Windows.Forms.Label lbTenMonAn;
        private System.Windows.Forms.ListView lsVCacMonAn;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox grHomNayAnGi;
        private System.Windows.Forms.TextBox OutxtTenMonAn;
        private System.Windows.Forms.TextBox OutxtTenNguoiDongGop;
        private System.Windows.Forms.Label OutLbTenMonAN;
        private System.Windows.Forms.Label OutLbTenNguoiDongGop;
        private System.Windows.Forms.Button btnChonMon;
        private System.Windows.Forms.PictureBox picAnhMonAn;
    }
}