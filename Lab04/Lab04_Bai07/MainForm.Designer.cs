namespace Lab04_Bai07
{
    partial class MainForm
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
            this.header = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRandom = new System.Windows.Forms.Button();
            this.lbHeader = new System.Windows.Forms.Label();
            this.footer = new System.Windows.Forms.Panel();
            this.pgbLoading = new System.Windows.Forms.ProgressBar();
            this.lkLogOut = new System.Windows.Forms.LinkLabel();
            this.cboPageSize = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboPage = new System.Windows.Forms.ComboBox();
            this.lblPage = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flpFoodList = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flpMyFood = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.header.SuspendLayout();
            this.footer.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.Controls.Add(this.btnAdd);
            this.header.Controls.Add(this.btnRandom);
            this.header.Controls.Add(this.lbHeader);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(745, 107);
            this.header.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.AntiqueWhite;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(599, 56);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(134, 43);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Thêm món ăn";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRandom
            // 
            this.btnRandom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRandom.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnRandom.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRandom.Location = new System.Drawing.Point(438, 56);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(134, 43);
            this.btnRandom.TabIndex = 1;
            this.btnRandom.Text = "Ăn gì giờ?";
            this.btnRandom.UseVisualStyleBackColor = false;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // lbHeader
            // 
            this.lbHeader.AutoSize = true;
            this.lbHeader.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeader.ForeColor = System.Drawing.Color.Tomato;
            this.lbHeader.Location = new System.Drawing.Point(7, 9);
            this.lbHeader.Name = "lbHeader";
            this.lbHeader.Size = new System.Drawing.Size(425, 65);
            this.lbHeader.TabIndex = 0;
            this.lbHeader.Text = "HÔM NAY ĂN GÌ?";
            // 
            // footer
            // 
            this.footer.Controls.Add(this.pgbLoading);
            this.footer.Controls.Add(this.lkLogOut);
            this.footer.Controls.Add(this.cboPageSize);
            this.footer.Controls.Add(this.label1);
            this.footer.Controls.Add(this.cboPage);
            this.footer.Controls.Add(this.lblPage);
            this.footer.Controls.Add(this.lblWelcome);
            this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer.Location = new System.Drawing.Point(0, 641);
            this.footer.Name = "footer";
            this.footer.Size = new System.Drawing.Size(745, 92);
            this.footer.TabIndex = 1;
            // 
            // pgbLoading
            // 
            this.pgbLoading.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pgbLoading.Location = new System.Drawing.Point(0, 75);
            this.pgbLoading.MarqueeAnimationSpeed = 80;
            this.pgbLoading.Name = "pgbLoading";
            this.pgbLoading.Size = new System.Drawing.Size(745, 17);
            this.pgbLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgbLoading.TabIndex = 6;
            // 
            // lkLogOut
            // 
            this.lkLogOut.AutoSize = true;
            this.lkLogOut.ForeColor = System.Drawing.Color.ForestGreen;
            this.lkLogOut.LinkColor = System.Drawing.SystemColors.MenuHighlight;
            this.lkLogOut.Location = new System.Drawing.Point(118, 35);
            this.lkLogOut.Name = "lkLogOut";
            this.lkLogOut.Size = new System.Drawing.Size(62, 20);
            this.lkLogOut.TabIndex = 5;
            this.lkLogOut.TabStop = true;
            this.lkLogOut.Text = "Log Out";
            this.lkLogOut.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkLogOut_LinkClicked);
            // 
            // cboPageSize
            // 
            this.cboPageSize.FormattingEnabled = true;
            this.cboPageSize.Items.AddRange(new object[] {
            "5",
            "10",
            "20"});
            this.cboPageSize.Location = new System.Drawing.Point(585, 32);
            this.cboPageSize.Name = "cboPageSize";
            this.cboPageSize.Size = new System.Drawing.Size(121, 28);
            this.cboPageSize.TabIndex = 4;
            this.cboPageSize.SelectedIndexChanged += new System.EventHandler(this.cboPageSize_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(496, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Page Size ";
            // 
            // cboPage
            // 
            this.cboPage.FormattingEnabled = true;
            this.cboPage.Location = new System.Drawing.Point(356, 32);
            this.cboPage.Name = "cboPage";
            this.cboPage.Size = new System.Drawing.Size(121, 28);
            this.cboPage.TabIndex = 2;
            this.cboPage.SelectedIndexChanged += new System.EventHandler(this.cboPage_SelectedIndexChanged);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(309, 32);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(41, 20);
            this.lblPage.TabIndex = 1;
            this.lblPage.Text = "Page";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblWelcome.Location = new System.Drawing.Point(12, 35);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(100, 20);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome,Bao";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 107);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(745, 534);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flpFoodList);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(737, 501);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "All";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flpFoodList
            // 
            this.flpFoodList.AutoScroll = true;
            this.flpFoodList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFoodList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpFoodList.Location = new System.Drawing.Point(3, 3);
            this.flpFoodList.Name = "flpFoodList";
            this.flpFoodList.Size = new System.Drawing.Size(731, 495);
            this.flpFoodList.TabIndex = 0;
            this.flpFoodList.WrapContents = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flpMyFood);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(737, 501);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tôi đóng góp";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // flpMyFood
            // 
            this.flpMyFood.AutoScroll = true;
            this.flpMyFood.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMyFood.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMyFood.Location = new System.Drawing.Point(3, 3);
            this.flpMyFood.Name = "flpMyFood";
            this.flpMyFood.Size = new System.Drawing.Size(731, 495);
            this.flpMyFood.TabIndex = 0;
            this.flpMyFood.WrapContents = false;
            this.flpMyFood.Paint += new System.Windows.Forms.PaintEventHandler(this.flpMyFood_Paint);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(965, 196);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(745, 733);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.footer);
            this.Controls.Add(this.header);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "HÔM NAY ĂN GÌ?";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.footer.ResumeLayout(false);
            this.footer.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel header;
        private System.Windows.Forms.Panel footer;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lbHeader;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.FlowLayoutPanel flpFoodList;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.ComboBox cboPageSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPage;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flpMyFood;
        private System.Windows.Forms.LinkLabel lkLogOut;
        private System.Windows.Forms.ProgressBar pgbLoading;
    }
}

