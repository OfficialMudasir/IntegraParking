﻿namespace CSFacturacionTest
{
    partial class Form1
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
            this.buttonSendInvoices = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSendInvoices
            // 
            this.buttonSendInvoices.Location = new System.Drawing.Point(12, 12);
            this.buttonSendInvoices.Name = "buttonSendInvoices";
            this.buttonSendInvoices.Size = new System.Drawing.Size(168, 23);
            this.buttonSendInvoices.TabIndex = 0;
            this.buttonSendInvoices.Text = "Send Invoices";
            this.buttonSendInvoices.UseVisualStyleBackColor = true;
            this.buttonSendInvoices.Click += new System.EventHandler(this.buttonSendInvoices_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 259);
            this.Controls.Add(this.buttonSendInvoices);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSendInvoices;
    }
}

