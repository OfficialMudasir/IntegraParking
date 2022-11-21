namespace OffstreetTest
{
    partial class Parking
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
            this.cmbParkings = new System.Windows.Forms.ComboBox();
            this.txtOpeId = new System.Windows.Forms.TextBox();
            this.btnQueryCarExitPayment = new System.Windows.Forms.Button();
            this.txtXmlOut = new System.Windows.Forms.TextBox();
            this.cmbPlates = new System.Windows.Forms.ComboBox();
            this.btnConfirmCarPayment = new System.Windows.Forms.Button();
            this.txtDiscountId = new System.Windows.Forms.TextBox();
            this.btnQueryCarDiscount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbParkings
            // 
            this.cmbParkings.FormattingEnabled = true;
            this.cmbParkings.Location = new System.Drawing.Point(57, 41);
            this.cmbParkings.Name = "cmbParkings";
            this.cmbParkings.Size = new System.Drawing.Size(315, 21);
            this.cmbParkings.TabIndex = 0;
            // 
            // txtOpeId
            // 
            this.txtOpeId.Location = new System.Drawing.Point(57, 68);
            this.txtOpeId.Name = "txtOpeId";
            this.txtOpeId.Size = new System.Drawing.Size(315, 20);
            this.txtOpeId.TabIndex = 1;
            // 
            // btnQueryCarExitPayment
            // 
            this.btnQueryCarExitPayment.Location = new System.Drawing.Point(57, 94);
            this.btnQueryCarExitPayment.Name = "btnQueryCarExitPayment";
            this.btnQueryCarExitPayment.Size = new System.Drawing.Size(315, 25);
            this.btnQueryCarExitPayment.TabIndex = 2;
            this.btnQueryCarExitPayment.Text = "Query Car Exit";
            this.btnQueryCarExitPayment.UseVisualStyleBackColor = true;
            this.btnQueryCarExitPayment.Click += new System.EventHandler(this.btnQueryCarExitPayment_Click);
            // 
            // txtXmlOut
            // 
            this.txtXmlOut.Location = new System.Drawing.Point(57, 224);
            this.txtXmlOut.Multiline = true;
            this.txtXmlOut.Name = "txtXmlOut";
            this.txtXmlOut.Size = new System.Drawing.Size(315, 354);
            this.txtXmlOut.TabIndex = 3;
            // 
            // cmbPlates
            // 
            this.cmbPlates.FormattingEnabled = true;
            this.cmbPlates.Location = new System.Drawing.Point(57, 14);
            this.cmbPlates.Name = "cmbPlates";
            this.cmbPlates.Size = new System.Drawing.Size(315, 21);
            this.cmbPlates.TabIndex = 4;
            // 
            // btnConfirmCarPayment
            // 
            this.btnConfirmCarPayment.Enabled = false;
            this.btnConfirmCarPayment.Location = new System.Drawing.Point(57, 182);
            this.btnConfirmCarPayment.Name = "btnConfirmCarPayment";
            this.btnConfirmCarPayment.Size = new System.Drawing.Size(315, 25);
            this.btnConfirmCarPayment.TabIndex = 5;
            this.btnConfirmCarPayment.Text = "Confirm Payment";
            this.btnConfirmCarPayment.UseVisualStyleBackColor = true;
            this.btnConfirmCarPayment.Click += new System.EventHandler(this.btnConfirmCarPayment_Click);
            // 
            // txtDiscountId
            // 
            this.txtDiscountId.Location = new System.Drawing.Point(57, 125);
            this.txtDiscountId.Name = "txtDiscountId";
            this.txtDiscountId.Size = new System.Drawing.Size(315, 20);
            this.txtDiscountId.TabIndex = 6;
            // 
            // btnQueryCarDiscount
            // 
            this.btnQueryCarDiscount.Enabled = false;
            this.btnQueryCarDiscount.Location = new System.Drawing.Point(57, 151);
            this.btnQueryCarDiscount.Name = "btnQueryCarDiscount";
            this.btnQueryCarDiscount.Size = new System.Drawing.Size(315, 25);
            this.btnQueryCarDiscount.TabIndex = 7;
            this.btnQueryCarDiscount.Text = "Query Discount";
            this.btnQueryCarDiscount.UseVisualStyleBackColor = true;
            this.btnQueryCarDiscount.Click += new System.EventHandler(this.btnQueryCarDiscount_Click);
            // 
            // Parking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 590);
            this.Controls.Add(this.btnQueryCarDiscount);
            this.Controls.Add(this.txtDiscountId);
            this.Controls.Add(this.btnConfirmCarPayment);
            this.Controls.Add(this.cmbPlates);
            this.Controls.Add(this.txtXmlOut);
            this.Controls.Add(this.btnQueryCarExitPayment);
            this.Controls.Add(this.txtOpeId);
            this.Controls.Add(this.cmbParkings);
            this.Name = "Parking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parking";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbParkings;
        private System.Windows.Forms.TextBox txtOpeId;
        private System.Windows.Forms.Button btnQueryCarExitPayment;
        private System.Windows.Forms.TextBox txtXmlOut;
        private System.Windows.Forms.ComboBox cmbPlates;
        private System.Windows.Forms.Button btnConfirmCarPayment;
        private System.Windows.Forms.TextBox txtDiscountId;
        private System.Windows.Forms.Button btnQueryCarDiscount;
    }
}