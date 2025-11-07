namespace Lab03_Bai04
{
    partial class Lab03_Bai04
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
            this.btnOpenSV = new System.Windows.Forms.Button();
            this.btnOpenCli = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenSV
            // 
            this.btnOpenSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSV.Location = new System.Drawing.Point(88, 27);
            this.btnOpenSV.Name = "btnOpenSV";
            this.btnOpenSV.Size = new System.Drawing.Size(381, 93);
            this.btnOpenSV.TabIndex = 0;
            this.btnOpenSV.Text = "Open TCP Server";
            this.btnOpenSV.UseVisualStyleBackColor = true;
            this.btnOpenSV.Click += new System.EventHandler(this.btnOpenSV_Click);
            // 
            // btnOpenCli
            // 
            this.btnOpenCli.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenCli.Location = new System.Drawing.Point(88, 146);
            this.btnOpenCli.Name = "btnOpenCli";
            this.btnOpenCli.Size = new System.Drawing.Size(381, 93);
            this.btnOpenCli.TabIndex = 1;
            this.btnOpenCli.Text = "Open TCP Client";
            this.btnOpenCli.UseVisualStyleBackColor = true;
            this.btnOpenCli.Click += new System.EventHandler(this.btnOpenCli_Click);
            // 
            // Lab03_Bai04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 318);
            this.Controls.Add(this.btnOpenCli);
            this.Controls.Add(this.btnOpenSV);
            this.Name = "Lab03_Bai04";
            this.Text = "Lab03_Bai04";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenSV;
        private System.Windows.Forms.Button btnOpenCli;
    }
}

