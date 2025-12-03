namespace Lab04_Bai07
{
    partial class FoodItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picFood = new System.Windows.Forms.PictureBox();
            this.lblFoodName = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblContributor = new System.Windows.Forms.Label();
            this.btnXoaMonAn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picFood)).BeginInit();
            this.SuspendLayout();
            // 
            // picFood
            // 
            this.picFood.Location = new System.Drawing.Point(4, 5);
            this.picFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picFood.Name = "picFood";
            this.picFood.Size = new System.Drawing.Size(174, 141);
            this.picFood.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFood.TabIndex = 0;
            this.picFood.TabStop = false;
            // 
            // lblFoodName
            // 
            this.lblFoodName.AutoSize = true;
            this.lblFoodName.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFoodName.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblFoodName.Location = new System.Drawing.Point(225, 15);
            this.lblFoodName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFoodName.Name = "lblFoodName";
            this.lblFoodName.Size = new System.Drawing.Size(61, 25);
            this.lblFoodName.TabIndex = 1;
            this.lblFoodName.Text = "label1";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(226, 56);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(31, 20);
            this.lblPrice.TabIndex = 2;
            this.lblPrice.Text = "Gia";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(226, 88);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(58, 20);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Dia Chi";
            // 
            // lblContributor
            // 
            this.lblContributor.AutoSize = true;
            this.lblContributor.ForeColor = System.Drawing.Color.Green;
            this.lblContributor.Location = new System.Drawing.Point(226, 121);
            this.lblContributor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContributor.Name = "lblContributor";
            this.lblContributor.Size = new System.Drawing.Size(78, 20);
            this.lblContributor.TabIndex = 4;
            this.lblContributor.Text = "Dong gop";
            // 
            // btnXoaMonAn
            // 
            this.btnXoaMonAn.BackColor = System.Drawing.Color.Brown;
            this.btnXoaMonAn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaMonAn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnXoaMonAn.Location = new System.Drawing.Point(565, 112);
            this.btnXoaMonAn.Name = "btnXoaMonAn";
            this.btnXoaMonAn.Size = new System.Drawing.Size(139, 34);
            this.btnXoaMonAn.TabIndex = 5;
            this.btnXoaMonAn.Text = "Xoa Mon An";
            this.btnXoaMonAn.UseVisualStyleBackColor = false;
            this.btnXoaMonAn.Visible = false;
            this.btnXoaMonAn.Click += new System.EventHandler(this.btnXoaMonAn_Click);
            // 
            // FoodItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnXoaMonAn);
            this.Controls.Add(this.lblContributor);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblFoodName);
            this.Controls.Add(this.picFood);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FoodItem";
            this.Size = new System.Drawing.Size(1136, 151);
            ((System.ComponentModel.ISupportInitialize)(this.picFood)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picFood;
        private System.Windows.Forms.Label lblFoodName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblContributor;
        private System.Windows.Forms.Button btnXoaMonAn;
    }
}
