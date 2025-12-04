using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab04_Bai04
{
    partial class Datvephim
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.GroupBox gbThongTin;
        private System.Windows.Forms.ComboBox cbPhongChieu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbChonPhim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbChonGhe;
        private System.Windows.Forms.Button btnA5;
        private System.Windows.Forms.Button btnA4;
        private System.Windows.Forms.Button btnA3;
        private System.Windows.Forms.Button btnA2;
        private System.Windows.Forms.Button btnA1;
        private System.Windows.Forms.Label lblScreen;
        private System.Windows.Forms.Button btnC5;
        private System.Windows.Forms.Button btnC4;
        private System.Windows.Forms.Button btnC3;
        private System.Windows.Forms.Button btnC2;
        private System.Windows.Forms.Button btnC1;
        private System.Windows.Forms.Button btnB5;
        private System.Windows.Forms.Button btnB4;
        private System.Windows.Forms.Button btnB3;
        private System.Windows.Forms.Button btnB2;
        private System.Windows.Forms.Button btnB1;
        private System.Windows.Forms.GroupBox gbThanhToan;
        private System.Windows.Forms.TextBox txtThongTin;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.Button btnStat;
        private System.Windows.Forms.ProgressBar progressBar1;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.gbThongTin = new System.Windows.Forms.GroupBox();
            this.cbPhongChieu = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbChonPhim = new System.Windows.Forms.ComboBox();
            this.gbChonGhe = new System.Windows.Forms.GroupBox();
            this.lblScreen = new System.Windows.Forms.Label();
            this.btnC5 = new System.Windows.Forms.Button();
            this.btnC4 = new System.Windows.Forms.Button();
            this.btnC3 = new System.Windows.Forms.Button();
            this.btnC2 = new System.Windows.Forms.Button();
            this.btnC1 = new System.Windows.Forms.Button();
            this.btnB5 = new System.Windows.Forms.Button();
            this.btnB4 = new System.Windows.Forms.Button();
            this.btnB3 = new System.Windows.Forms.Button();
            this.btnB2 = new System.Windows.Forms.Button();
            this.btnB1 = new System.Windows.Forms.Button();
            this.btnA5 = new System.Windows.Forms.Button();
            this.btnA4 = new System.Windows.Forms.Button();
            this.btnA3 = new System.Windows.Forms.Button();
            this.btnA2 = new System.Windows.Forms.Button();
            this.btnA1 = new System.Windows.Forms.Button();
            this.gbThanhToan = new System.Windows.Forms.GroupBox();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.txtThongTin = new System.Windows.Forms.TextBox();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnStat = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.gbThongTin.SuspendLayout();
            this.gbChonGhe.SuspendLayout();
            this.gbThanhToan.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbThongTin
            // 
            this.gbThongTin.Controls.Add(this.cbPhongChieu);
            this.gbThongTin.Controls.Add(this.label3);
            this.gbThongTin.Controls.Add(this.label2);
            this.gbThongTin.Controls.Add(this.txtHoTen);
            this.gbThongTin.Controls.Add(this.label1);
            this.gbThongTin.Controls.Add(this.cbChonPhim);
            this.gbThongTin.Location = new System.Drawing.Point(12, 12);
            this.gbThongTin.Name = "gbThongTin";
            this.gbThongTin.Size = new System.Drawing.Size(437, 303);
            this.gbThongTin.TabIndex = 0;
            this.gbThongTin.TabStop = false;
            this.gbThongTin.Text = "Thông tin khách hàng";
            // 
            // cbPhongChieu
            // 
            this.cbPhongChieu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPhongChieu.FormattingEnabled = true;
            this.cbPhongChieu.Location = new System.Drawing.Point(21, 238);
            this.cbPhongChieu.Name = "cbPhongChieu";
            this.cbPhongChieu.Size = new System.Drawing.Size(395, 33);
            this.cbPhongChieu.TabIndex = 5;
            this.cbPhongChieu.SelectedIndexChanged += cbPhongChieu_SelectedIndexChanged;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Chọn phòng chiếu:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Chọn phim:";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Location = new System.Drawing.Point(21, 74);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(395, 31);
            this.txtHoTen.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Họ và tên khách hàng:";
            // 
            // cbChonPhim
            // 
            this.cbChonPhim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChonPhim.FormattingEnabled = true;
            this.cbChonPhim.Location = new System.Drawing.Point(21, 151);
            this.cbChonPhim.Name = "cbChonPhim";
            this.cbChonPhim.Size = new System.Drawing.Size(395, 33);
            this.cbChonPhim.TabIndex = 3;
            this.cbChonPhim.SelectedIndexChanged += cbChonPhim_SelectedIndexChanged;
            // 
            // gbChonGhe
            // 
            this.gbChonGhe.Controls.Add(this.lblScreen);
            this.gbChonGhe.Controls.Add(this.btnC5);
            this.gbChonGhe.Controls.Add(this.btnC4);
            this.gbChonGhe.Controls.Add(this.btnC3);
            this.gbChonGhe.Controls.Add(this.btnC2);
            this.gbChonGhe.Controls.Add(this.btnC1);
            this.gbChonGhe.Controls.Add(this.btnB5);
            this.gbChonGhe.Controls.Add(this.btnB4);
            this.gbChonGhe.Controls.Add(this.btnB3);
            this.gbChonGhe.Controls.Add(this.btnB2);
            this.gbChonGhe.Controls.Add(this.btnB1);
            this.gbChonGhe.Controls.Add(this.btnA5);
            this.gbChonGhe.Controls.Add(this.btnA4);
            this.gbChonGhe.Controls.Add(this.btnA3);
            this.gbChonGhe.Controls.Add(this.btnA2);
            this.gbChonGhe.Controls.Add(this.btnA1);
            this.gbChonGhe.Location = new System.Drawing.Point(469, 12);
            this.gbChonGhe.Name = "gbChonGhe";
            this.gbChonGhe.Size = new System.Drawing.Size(514, 303);
            this.gbChonGhe.TabIndex = 1;
            this.gbChonGhe.TabStop = false;
            this.gbChonGhe.Text = "Sơ đồ phòng chiếu";
            // 
            // lblScreen
            // 
            this.lblScreen.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblScreen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblScreen.Location = new System.Drawing.Point(65, 51);
            this.lblScreen.Name = "lblScreen";
            this.lblScreen.Size = new System.Drawing.Size(406, 43);
            this.lblScreen.TabIndex = 15;
            this.lblScreen.Text = "MÀN HÌNH";
            this.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnC5
            // 
            this.btnC5.Location = new System.Drawing.Point(405, 247);
            this.btnC5.Name = "btnC5";
            this.btnC5.Size = new System.Drawing.Size(66, 44);
            this.btnC5.TabIndex = 14;
            this.btnC5.Text = "C5";
            this.btnC5.UseVisualStyleBackColor = true;
            // 
            // btnC4
            // 
            this.btnC4.Location = new System.Drawing.Point(321, 247);
            this.btnC4.Name = "btnC4";
            this.btnC4.Size = new System.Drawing.Size(66, 44);
            this.btnC4.TabIndex = 13;
            this.btnC4.Text = "C4";
            this.btnC4.UseVisualStyleBackColor = true;
            // 
            // btnC3
            // 
            this.btnC3.Location = new System.Drawing.Point(236, 247);
            this.btnC3.Name = "btnC3";
            this.btnC3.Size = new System.Drawing.Size(66, 44);
            this.btnC3.TabIndex = 12;
            this.btnC3.Text = "C3";
            this.btnC3.UseVisualStyleBackColor = true;
            // 
            // btnC2
            // 
            this.btnC2.Location = new System.Drawing.Point(150, 247);
            this.btnC2.Name = "btnC2";
            this.btnC2.Size = new System.Drawing.Size(66, 44);
            this.btnC2.TabIndex = 11;
            this.btnC2.Text = "C2";
            this.btnC2.UseVisualStyleBackColor = true;
            // 
            // btnC1
            // 
            this.btnC1.Location = new System.Drawing.Point(65, 247);
            this.btnC1.Name = "btnC1";
            this.btnC1.Size = new System.Drawing.Size(66, 44);
            this.btnC1.TabIndex = 10;
            this.btnC1.Text = "C1";
            this.btnC1.UseVisualStyleBackColor = true;
            // 
            // btnB5
            // 
            this.btnB5.Location = new System.Drawing.Point(405, 197);
            this.btnB5.Name = "btnB5";
            this.btnB5.Size = new System.Drawing.Size(66, 44);
            this.btnB5.TabIndex = 9;
            this.btnB5.Text = "B5";
            this.btnB5.UseVisualStyleBackColor = true;
            // 
            // btnB4
            // 
            this.btnB4.Location = new System.Drawing.Point(321, 197);
            this.btnB4.Name = "btnB4";
            this.btnB4.Size = new System.Drawing.Size(66, 44);
            this.btnB4.TabIndex = 8;
            this.btnB4.Text = "B4";
            this.btnB4.UseVisualStyleBackColor = true;
            // 
            // btnB3
            // 
            this.btnB3.Location = new System.Drawing.Point(236, 197);
            this.btnB3.Name = "btnB3";
            this.btnB3.Size = new System.Drawing.Size(66, 44);
            this.btnB3.TabIndex = 7;
            this.btnB3.Text = "B3";
            this.btnB3.UseVisualStyleBackColor = true;
            // 
            // btnB2
            // 
            this.btnB2.Location = new System.Drawing.Point(150, 197);
            this.btnB2.Name = "btnB2";
            this.btnB2.Size = new System.Drawing.Size(66, 44);
            this.btnB2.TabIndex = 6;
            this.btnB2.Text = "B2";
            this.btnB2.UseVisualStyleBackColor = true;
            // 
            // btnB1
            // 
            this.btnB1.Location = new System.Drawing.Point(65, 197);
            this.btnB1.Name = "btnB1";
            this.btnB1.Size = new System.Drawing.Size(66, 44);
            this.btnB1.TabIndex = 5;
            this.btnB1.Text = "B1";
            this.btnB1.UseVisualStyleBackColor = true;
            // 
            // btnA5
            // 
            this.btnA5.Location = new System.Drawing.Point(405, 147);
            this.btnA5.Name = "btnA5";
            this.btnA5.Size = new System.Drawing.Size(66, 44);
            this.btnA5.TabIndex = 4;
            this.btnA5.Text = "A5";
            this.btnA5.UseVisualStyleBackColor = true;
            // 
            // btnA4
            // 
            this.btnA4.Location = new System.Drawing.Point(321, 147);
            this.btnA4.Name = "btnA4";
            this.btnA4.Size = new System.Drawing.Size(66, 44);
            this.btnA4.TabIndex = 3;
            this.btnA4.Text = "A4";
            this.btnA4.UseVisualStyleBackColor = true;
            // 
            // btnA3
            // 
            this.btnA3.Location = new System.Drawing.Point(236, 147);
            this.btnA3.Name = "btnA3";
            this.btnA3.Size = new System.Drawing.Size(66, 44);
            this.btnA3.TabIndex = 2;
            this.btnA3.Text = "A3";
            this.btnA3.UseVisualStyleBackColor = true;
            // 
            // btnA2
            // 
            this.btnA2.Location = new System.Drawing.Point(150, 147);
            this.btnA2.Name = "btnA2";
            this.btnA2.Size = new System.Drawing.Size(66, 44);
            this.btnA2.TabIndex = 1;
            this.btnA2.Text = "A2";
            this.btnA2.UseVisualStyleBackColor = true;
            // 
            // btnA1
            // 
            this.btnA1.Location = new System.Drawing.Point(65, 147);
            this.btnA1.Name = "btnA1";
            this.btnA1.Size = new System.Drawing.Size(66, 44);
            this.btnA1.TabIndex = 0;
            this.btnA1.Text = "A1";
            this.btnA1.UseVisualStyleBackColor = true;
            // 
            // gbThanhToan
            // 
            this.gbThanhToan.Controls.Add(this.btnStat);
            this.gbThanhToan.Controls.Add(this.btnResetAll);
            this.gbThanhToan.Controls.Add(this.txtThongTin);
            this.gbThanhToan.Controls.Add(this.btnHuy);
            this.gbThanhToan.Controls.Add(this.btnThanhToan);
            this.gbThanhToan.Location = new System.Drawing.Point(12, 321);
            this.gbThanhToan.Name = "gbThanhToan";
            this.gbThanhToan.Size = new System.Drawing.Size(971, 403);
            this.gbThanhToan.TabIndex = 2;
            this.gbThanhToan.TabStop = false;
            this.gbThanhToan.Text = "Thông tin vé và thanh toán";
            this.gbThanhToan.Controls.Add(this.progressBar1);
            // 
            // btnResetAll
            // 
            this.btnResetAll.Location = new System.Drawing.Point(744, 274);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(203, 74);
            this.btnResetAll.TabIndex = 3;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new EventHandler(this.btnResetAll_Click);
            // 
            // txtThongTin
            // 
            this.txtThongTin.Location = new System.Drawing.Point(21, 38);
            this.txtThongTin.Multiline = true;
            this.txtThongTin.Name = "txtThongTin";
            this.txtThongTin.ReadOnly = true;
            this.txtThongTin.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThongTin.Size = new System.Drawing.Size(696, 321);
            this.txtThongTin.TabIndex = 2;
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(744, 110);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(203, 74);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new EventHandler(this.btnHuy_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.Location = new System.Drawing.Point(744, 26);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(203, 78);
            this.btnThanhToan.TabIndex = 0;
            this.btnThanhToan.Text = "Thanh Toán";
            this.btnThanhToan.UseVisualStyleBackColor = true;
            this.btnThanhToan.Click += new EventHandler(this.btnThanhToan_Click);
            // 
            // btnStat
            // 
            this.btnStat.Location = new System.Drawing.Point(744, 194);
            this.btnStat.Name = "btnStat";
            this.btnStat.Size = new System.Drawing.Size(203, 74);
            this.btnStat.TabIndex = 4;
            this.btnStat.Text = "Xem thống kê";
            this.btnStat.UseVisualStyleBackColor = true;
            this.btnStat.Click += new System.EventHandler(this.btnStat_Click);
            // 
            // progressBar
            this.progressBar1.Location = new System.Drawing.Point(21, 365);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(696, 23);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Visible = false;
            //
            // Lab04_Bai04
            // 
            this.ClientSize = new System.Drawing.Size(1019, 765);
            this.Controls.Add(this.gbThanhToan);
            this.Controls.Add(this.gbChonGhe);
            this.Controls.Add(this.gbThongTin);
            this.Name = "Lab04_Bai04";
            this.Text = "Lab04_Bai04";
            this.gbThongTin.ResumeLayout(false);
            this.gbThongTin.PerformLayout();
            this.gbChonGhe.ResumeLayout(false);
            this.gbThanhToan.ResumeLayout(false);
            this.gbThanhToan.PerformLayout();
            this.ResumeLayout(false);
            this.Load += new System.EventHandler(this.Datvephim_Load);
        }

        #endregion
    }
}
