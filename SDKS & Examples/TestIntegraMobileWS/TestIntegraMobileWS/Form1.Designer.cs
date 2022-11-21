namespace TestIntegraMobileWS
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
            this.encryptTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.HMAC_LOCAL = new System.Windows.Forms.Label();
            this.HMAC_REMOTE = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // encryptTextBox
            // 
            this.encryptTextBox.Location = new System.Drawing.Point(64, 52);
            this.encryptTextBox.Name = "encryptTextBox";
            this.encryptTextBox.Size = new System.Drawing.Size(172, 20);
            this.encryptTextBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 182);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 52);
            this.button1.TabIndex = 1;
            this.button1.Text = "Calculate HMAC";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HMAC_LOCAL
            // 
            this.HMAC_LOCAL.AutoSize = true;
            this.HMAC_LOCAL.Location = new System.Drawing.Point(64, 94);
            this.HMAC_LOCAL.Name = "HMAC_LOCAL";
            this.HMAC_LOCAL.Size = new System.Drawing.Size(78, 13);
            this.HMAC_LOCAL.TabIndex = 2;
            this.HMAC_LOCAL.Text = "HMAC_LOCAL";
            // 
            // HMAC_REMOTE
            // 
            this.HMAC_REMOTE.AutoSize = true;
            this.HMAC_REMOTE.Location = new System.Drawing.Point(64, 128);
            this.HMAC_REMOTE.Name = "HMAC_REMOTE";
            this.HMAC_REMOTE.Size = new System.Drawing.Size(90, 13);
            this.HMAC_REMOTE.TabIndex = 3;
            this.HMAC_REMOTE.Text = "HMAC_REMOTE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Write a text";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HMAC_REMOTE);
            this.Controls.Add(this.HMAC_LOCAL);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.encryptTextBox);
            this.Name = "Form1";
            this.Text = "Test IntegraMobile WS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox encryptTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label HMAC_LOCAL;
        private System.Windows.Forms.Label HMAC_REMOTE;
        private System.Windows.Forms.Label label1;
    }
}

