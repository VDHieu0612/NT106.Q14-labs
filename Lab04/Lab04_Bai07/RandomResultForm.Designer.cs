namespace Lab04_Bai07
{
    partial class RandomResultForm
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
            this.foodItemDisplay = new Lab04_Bai07.FoodItem();
            this.SuspendLayout();
            // 
            // foodItemDisplay
            // 
            this.foodItemDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.foodItemDisplay.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foodItemDisplay.Location = new System.Drawing.Point(3, 2);
            this.foodItemDisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.foodItemDisplay.Name = "foodItemDisplay";
            this.foodItemDisplay.Size = new System.Drawing.Size(1136, 151);
            this.foodItemDisplay.TabIndex = 0;
            // 
            // RandomResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 157);
            this.Controls.Add(this.foodItemDisplay);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RandomResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "An mon nay di!!!";
            this.ResumeLayout(false);

        }

        #endregion

        private FoodItem foodItemDisplay;
    }
}