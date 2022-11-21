namespace PlataformaMadridTest
{
    partial class FormMadridPlatformTest
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
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOpMin = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblErrStartSess = new System.Windows.Forms.Label();
            this.lblOK = new System.Windows.Forms.Label();
            this.lblErrQuery = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblErrSetParking = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblErrEndSess = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(285, 154);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Operations/Minute";
            // 
            // txtOpMin
            // 
            this.txtOpMin.Location = new System.Drawing.Point(150, 41);
            this.txtOpMin.Name = "txtOpMin";
            this.txtOpMin.Size = new System.Drawing.Size(207, 20);
            this.txtOpMin.TabIndex = 2;
            this.txtOpMin.Text = "85";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(285, 212);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(54, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "OK";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(54, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Error Start Sess.";
            // 
            // lblErrStartSess
            // 
            this.lblErrStartSess.AutoSize = true;
            this.lblErrStartSess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStartSess.Location = new System.Drawing.Point(212, 169);
            this.lblErrStartSess.Name = "lblErrStartSess";
            this.lblErrStartSess.Size = new System.Drawing.Size(25, 20);
            this.lblErrStartSess.TabIndex = 6;
            this.lblErrStartSess.Text = "llll";
            this.lblErrStartSess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOK
            // 
            this.lblOK.AutoSize = true;
            this.lblOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOK.Location = new System.Drawing.Point(212, 111);
            this.lblOK.Name = "lblOK";
            this.lblOK.Size = new System.Drawing.Size(25, 20);
            this.lblOK.TabIndex = 7;
            this.lblOK.Text = "llll";
            this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblErrQuery
            // 
            this.lblErrQuery.AutoSize = true;
            this.lblErrQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrQuery.Location = new System.Drawing.Point(212, 138);
            this.lblErrQuery.Name = "lblErrQuery";
            this.lblErrQuery.Size = new System.Drawing.Size(25, 20);
            this.lblErrQuery.TabIndex = 9;
            this.lblErrQuery.Text = "llll";
            this.lblErrQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(54, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Error Query";
            // 
            // lblErrSetParking
            // 
            this.lblErrSetParking.AutoSize = true;
            this.lblErrSetParking.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrSetParking.Location = new System.Drawing.Point(212, 202);
            this.lblErrSetParking.Name = "lblErrSetParking";
            this.lblErrSetParking.Size = new System.Drawing.Size(25, 20);
            this.lblErrSetParking.TabIndex = 11;
            this.lblErrSetParking.Text = "llll";
            this.lblErrSetParking.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(54, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 20);
            this.label7.TabIndex = 10;
            this.label7.Text = "Error SetParking";
            // 
            // lblErrEndSess
            // 
            this.lblErrEndSess.AutoSize = true;
            this.lblErrEndSess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrEndSess.Location = new System.Drawing.Point(212, 236);
            this.lblErrEndSess.Name = "lblErrEndSess";
            this.lblErrEndSess.Size = new System.Drawing.Size(25, 20);
            this.lblErrEndSess.TabIndex = 13;
            this.lblErrEndSess.Text = "llll";
            this.lblErrEndSess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(54, 236);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 20);
            this.label9.TabIndex = 12;
            this.label9.Text = "Error End Sess";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(150, 75);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(207, 20);
            this.txtTimeout.TabIndex = 15;
            this.txtTimeout.Text = "5000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "WS Timeout(ms)";
            // 
            // FormMadridPlatformTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 316);
            this.Controls.Add(this.txtTimeout);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblErrEndSess);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblErrSetParking);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblErrQuery);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblOK);
            this.Controls.Add(this.lblErrStartSess);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtOpMin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMadridPlatformTest";
            this.Text = "Madrid Platform Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOpMin;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblErrStartSess;
        private System.Windows.Forms.Label lblOK;
        private System.Windows.Forms.Label lblErrQuery;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblErrSetParking;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblErrEndSess;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label label4;
    }
}

