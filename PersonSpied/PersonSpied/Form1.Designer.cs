namespace PersonSpied
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
            this.tabCopyEmail = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtEmails = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblRoute = new System.Windows.Forms.Label();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkSaveToDB = new System.Windows.Forms.CheckBox();
            this.checkExportExcel = new System.Windows.Forms.CheckBox();
            this.checkExportTXT = new System.Windows.Forms.CheckBox();
            this.tabCopyEmail.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCopyEmail
            // 
            this.tabCopyEmail.Controls.Add(this.tabPage1);
            this.tabCopyEmail.Controls.Add(this.tabPage2);
            this.tabCopyEmail.Location = new System.Drawing.Point(12, 12);
            this.tabCopyEmail.Name = "tabCopyEmail";
            this.tabCopyEmail.SelectedIndex = 0;
            this.tabCopyEmail.Size = new System.Drawing.Size(325, 322);
            this.tabCopyEmail.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSearch);
            this.tabPage1.Controls.Add(this.txtEmails);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(317, 296);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Type email (comma separated)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(123, 252);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click_1);
            // 
            // txtEmails
            // 
            this.txtEmails.Location = new System.Drawing.Point(20, 50);
            this.txtEmails.Multiline = true;
            this.txtEmails.Name = "txtEmails";
            this.txtEmails.Size = new System.Drawing.Size(281, 179);
            this.txtEmails.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Emals:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblRoute);
            this.tabPage2.Controls.Add(this.btnSearch2);
            this.tabPage2.Controls.Add(this.btnOpen);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(317, 296);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Load email from file";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblRoute
            // 
            this.lblRoute.AutoSize = true;
            this.lblRoute.Location = new System.Drawing.Point(18, 35);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Size = new System.Drawing.Size(0, 13);
            this.lblRoute.TabIndex = 9;
            // 
            // btnSearch2
            // 
            this.btnSearch2.Enabled = false;
            this.btnSearch2.Location = new System.Drawing.Point(171, 250);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(75, 23);
            this.btnSearch2.TabIndex = 8;
            this.btnSearch2.Text = "Search";
            this.btnSearch2.UseVisualStyleBackColor = true;
            this.btnSearch2.Click += new System.EventHandler(this.btnSearch2_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(47, 250);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(97, 23);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.Text = "Open document";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // checkSaveToDB
            // 
            this.checkSaveToDB.AutoSize = true;
            this.checkSaveToDB.Checked = true;
            this.checkSaveToDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSaveToDB.Location = new System.Drawing.Point(16, 351);
            this.checkSaveToDB.Name = "checkSaveToDB";
            this.checkSaveToDB.Size = new System.Drawing.Size(81, 17);
            this.checkSaveToDB.TabIndex = 5;
            this.checkSaveToDB.Text = "Save to DB";
            this.checkSaveToDB.UseVisualStyleBackColor = true;
            // 
            // checkExportExcel
            // 
            this.checkExportExcel.AutoSize = true;
            this.checkExportExcel.Checked = true;
            this.checkExportExcel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkExportExcel.Location = new System.Drawing.Point(123, 351);
            this.checkExportExcel.Name = "checkExportExcel";
            this.checkExportExcel.Size = new System.Drawing.Size(85, 17);
            this.checkExportExcel.TabIndex = 6;
            this.checkExportExcel.Text = "Export Excel";
            this.checkExportExcel.UseVisualStyleBackColor = true;
            // 
            // checkExportTXT
            // 
            this.checkExportTXT.AutoSize = true;
            this.checkExportTXT.Location = new System.Drawing.Point(248, 351);
            this.checkExportTXT.Name = "checkExportTXT";
            this.checkExportTXT.Size = new System.Drawing.Size(80, 17);
            this.checkExportTXT.TabIndex = 7;
            this.checkExportTXT.Text = "Export TXT";
            this.checkExportTXT.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 380);
            this.Controls.Add(this.checkExportTXT);
            this.Controls.Add(this.checkExportExcel);
            this.Controls.Add(this.checkSaveToDB);
            this.Controls.Add(this.tabCopyEmail);
            this.Name = "Form1";
            this.Text = "Person Spied";
            this.tabCopyEmail.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabCopyEmail;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtEmails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblRoute;
        private System.Windows.Forms.Button btnSearch2;
        private System.Windows.Forms.CheckBox checkSaveToDB;
        private System.Windows.Forms.CheckBox checkExportExcel;
        private System.Windows.Forms.CheckBox checkExportTXT;
    }
}

