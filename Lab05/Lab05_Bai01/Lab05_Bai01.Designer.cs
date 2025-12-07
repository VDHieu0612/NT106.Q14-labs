namespace Lab05
{
    partial class Lab05_Bai01
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
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Fromtb = new System.Windows.Forms.TextBox();
            this.Totb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Subtb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Contentrtb = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(689, 13);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(150, 91);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(66, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(66, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // Fromtb
            // 
            this.Fromtb.Location = new System.Drawing.Point(201, 13);
            this.Fromtb.Name = "Fromtb";
            this.Fromtb.Size = new System.Drawing.Size(415, 31);
            this.Fromtb.TabIndex = 3;
            // 
            // Totb
            // 
            this.Totb.Location = new System.Drawing.Point(201, 73);
            this.Totb.Name = "Totb";
            this.Totb.Size = new System.Drawing.Size(415, 31);
            this.Totb.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(66, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 31);
            this.label3.TabIndex = 5;
            this.label3.Text = "Subject:";
            // 
            // Subtb
            // 
            this.Subtb.Location = new System.Drawing.Point(200, 125);
            this.Subtb.Name = "Subtb";
            this.Subtb.Size = new System.Drawing.Size(639, 31);
            this.Subtb.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(66, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 31);
            this.label4.TabIndex = 7;
            this.label4.Text = "Body:";
            // 
            // Contentrtb
            // 
            this.Contentrtb.Location = new System.Drawing.Point(200, 180);
            this.Contentrtb.Name = "Contentrtb";
            this.Contentrtb.Size = new System.Drawing.Size(639, 586);
            this.Contentrtb.TabIndex = 8;
            this.Contentrtb.Text = "";
            // 
            // Lab05_Bai01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 805);
            this.Controls.Add(this.Contentrtb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Subtb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Totb);
            this.Controls.Add(this.Fromtb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Name = "Lab05_Bai01";
            this.Text = "Lab05_Bai01";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Fromtb;
        private System.Windows.Forms.TextBox Totb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Subtb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox Contentrtb;
    }
}

