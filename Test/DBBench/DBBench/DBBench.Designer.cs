namespace DBBench
{
    partial class DBBench
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTestPG = new System.Windows.Forms.Button();
            this.btnTestPG2 = new System.Windows.Forms.Button();
            this.btnOra1 = new System.Windows.Forms.Button();
            this.btnORA2 = new System.Windows.Forms.Button();
            this.btnMy1 = new System.Windows.Forms.Button();
            this.btnMy2 = new System.Windows.Forms.Button();
            this.txtNumRegistries = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtThreads = new System.Windows.Forms.TextBox();
            this.btnMy3 = new System.Windows.Forms.Button();
            this.btnORA3 = new System.Windows.Forms.Button();
            this.btnTestPG3 = new System.Windows.Forms.Button();
            this.btnTestMy4 = new System.Windows.Forms.Button();
            this.btnTestOra4 = new System.Windows.Forms.Button();
            this.btnTestPG4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTestPG
            // 
            this.btnTestPG.Location = new System.Drawing.Point(41, 115);
            this.btnTestPG.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestPG.Name = "btnTestPG";
            this.btnTestPG.Size = new System.Drawing.Size(135, 29);
            this.btnTestPG.TabIndex = 0;
            this.btnTestPG.Text = "Test INS PG Hib";
            this.btnTestPG.UseVisualStyleBackColor = true;
            this.btnTestPG.Click += new System.EventHandler(this.btnTestPG_Click);
            // 
            // btnTestPG2
            // 
            this.btnTestPG2.Location = new System.Drawing.Point(192, 115);
            this.btnTestPG2.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestPG2.Name = "btnTestPG2";
            this.btnTestPG2.Size = new System.Drawing.Size(135, 29);
            this.btnTestPG2.TabIndex = 1;
            this.btnTestPG2.Text = "Test INS PG ADO";
            this.btnTestPG2.UseVisualStyleBackColor = true;
            this.btnTestPG2.Click += new System.EventHandler(this.btnTestPG2_Click);
            // 
            // btnOra1
            // 
            this.btnOra1.Location = new System.Drawing.Point(41, 162);
            this.btnOra1.Margin = new System.Windows.Forms.Padding(2);
            this.btnOra1.Name = "btnOra1";
            this.btnOra1.Size = new System.Drawing.Size(135, 31);
            this.btnOra1.TabIndex = 2;
            this.btnOra1.Text = "Test INS ORA Hib";
            this.btnOra1.UseVisualStyleBackColor = true;
            this.btnOra1.Click += new System.EventHandler(this.btnOra1_Click);
            // 
            // btnORA2
            // 
            this.btnORA2.Location = new System.Drawing.Point(192, 162);
            this.btnORA2.Margin = new System.Windows.Forms.Padding(2);
            this.btnORA2.Name = "btnORA2";
            this.btnORA2.Size = new System.Drawing.Size(135, 31);
            this.btnORA2.TabIndex = 3;
            this.btnORA2.Text = "Test INS ORA ADO";
            this.btnORA2.UseVisualStyleBackColor = true;
            this.btnORA2.Click += new System.EventHandler(this.btnORA2_Click);
            // 
            // btnMy1
            // 
            this.btnMy1.Location = new System.Drawing.Point(41, 214);
            this.btnMy1.Margin = new System.Windows.Forms.Padding(2);
            this.btnMy1.Name = "btnMy1";
            this.btnMy1.Size = new System.Drawing.Size(135, 27);
            this.btnMy1.TabIndex = 4;
            this.btnMy1.Text = "Test INS My Hib";
            this.btnMy1.UseVisualStyleBackColor = true;
            this.btnMy1.Click += new System.EventHandler(this.btnMy1_Click);
            // 
            // btnMy2
            // 
            this.btnMy2.Location = new System.Drawing.Point(192, 214);
            this.btnMy2.Margin = new System.Windows.Forms.Padding(2);
            this.btnMy2.Name = "btnMy2";
            this.btnMy2.Size = new System.Drawing.Size(135, 27);
            this.btnMy2.TabIndex = 5;
            this.btnMy2.Text = "Test INS My ADO";
            this.btnMy2.UseVisualStyleBackColor = true;
            this.btnMy2.Click += new System.EventHandler(this.btnMy2_Click);
            // 
            // txtNumRegistries
            // 
            this.txtNumRegistries.Location = new System.Drawing.Point(137, 20);
            this.txtNumRegistries.Margin = new System.Windows.Forms.Padding(2);
            this.txtNumRegistries.Name = "txtNumRegistries";
            this.txtNumRegistries.Size = new System.Drawing.Size(114, 20);
            this.txtNumRegistries.TabIndex = 6;
            this.txtNumRegistries.Text = "1000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Número de registros";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Número de threads";
            // 
            // txtThreads
            // 
            this.txtThreads.Location = new System.Drawing.Point(137, 54);
            this.txtThreads.Margin = new System.Windows.Forms.Padding(2);
            this.txtThreads.Name = "txtThreads";
            this.txtThreads.Size = new System.Drawing.Size(114, 20);
            this.txtThreads.TabIndex = 8;
            this.txtThreads.Text = "1";
            // 
            // btnMy3
            // 
            this.btnMy3.Location = new System.Drawing.Point(346, 214);
            this.btnMy3.Margin = new System.Windows.Forms.Padding(2);
            this.btnMy3.Name = "btnMy3";
            this.btnMy3.Size = new System.Drawing.Size(135, 27);
            this.btnMy3.TabIndex = 12;
            this.btnMy3.Text = "Test SEL My Hib";
            this.btnMy3.UseVisualStyleBackColor = true;
            this.btnMy3.Click += new System.EventHandler(this.btnMy3_Click);
            // 
            // btnORA3
            // 
            this.btnORA3.Location = new System.Drawing.Point(346, 162);
            this.btnORA3.Margin = new System.Windows.Forms.Padding(2);
            this.btnORA3.Name = "btnORA3";
            this.btnORA3.Size = new System.Drawing.Size(135, 31);
            this.btnORA3.TabIndex = 11;
            this.btnORA3.Text = "Test SEL ORA Hib";
            this.btnORA3.UseVisualStyleBackColor = true;
            this.btnORA3.Click += new System.EventHandler(this.btnORA3_Click);
            // 
            // btnTestPG3
            // 
            this.btnTestPG3.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnTestPG3.Location = new System.Drawing.Point(346, 115);
            this.btnTestPG3.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestPG3.Name = "btnTestPG3";
            this.btnTestPG3.Size = new System.Drawing.Size(135, 29);
            this.btnTestPG3.TabIndex = 10;
            this.btnTestPG3.Text = "Test SEL PG Hib";
            this.btnTestPG3.UseVisualStyleBackColor = true;
            this.btnTestPG3.Click += new System.EventHandler(this.btnTestPG3_Click);
            // 
            // btnTestMy4
            // 
            this.btnTestMy4.Location = new System.Drawing.Point(503, 214);
            this.btnTestMy4.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestMy4.Name = "btnTestMy4";
            this.btnTestMy4.Size = new System.Drawing.Size(135, 27);
            this.btnTestMy4.TabIndex = 15;
            this.btnTestMy4.Text = "Test MIX My Hib";
            this.btnTestMy4.UseVisualStyleBackColor = true;
            this.btnTestMy4.Click += new System.EventHandler(this.btnTestMy4_Click);
            // 
            // btnTestOra4
            // 
            this.btnTestOra4.Location = new System.Drawing.Point(503, 162);
            this.btnTestOra4.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestOra4.Name = "btnTestOra4";
            this.btnTestOra4.Size = new System.Drawing.Size(135, 31);
            this.btnTestOra4.TabIndex = 14;
            this.btnTestOra4.Text = "Test MIX ORA Hib";
            this.btnTestOra4.UseVisualStyleBackColor = true;
            this.btnTestOra4.Click += new System.EventHandler(this.btnTestOra4_Click);
            // 
            // btnTestPG4
            // 
            this.btnTestPG4.Location = new System.Drawing.Point(503, 115);
            this.btnTestPG4.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestPG4.Name = "btnTestPG4";
            this.btnTestPG4.Size = new System.Drawing.Size(135, 29);
            this.btnTestPG4.TabIndex = 13;
            this.btnTestPG4.Text = "Test MIX PG Hib";
            this.btnTestPG4.UseVisualStyleBackColor = true;
            this.btnTestPG4.Click += new System.EventHandler(this.btnTestPG4_Click);
            // 
            // DBBench
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 344);
            this.Controls.Add(this.btnTestMy4);
            this.Controls.Add(this.btnTestOra4);
            this.Controls.Add(this.btnTestPG4);
            this.Controls.Add(this.btnMy3);
            this.Controls.Add(this.btnORA3);
            this.Controls.Add(this.btnTestPG3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtThreads);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumRegistries);
            this.Controls.Add(this.btnMy2);
            this.Controls.Add(this.btnMy1);
            this.Controls.Add(this.btnORA2);
            this.Controls.Add(this.btnOra1);
            this.Controls.Add(this.btnTestPG2);
            this.Controls.Add(this.btnTestPG);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "DBBench";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBBench";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTestPG;
        private System.Windows.Forms.Button btnTestPG2;
        private System.Windows.Forms.Button btnOra1;
        private System.Windows.Forms.Button btnORA2;
        private System.Windows.Forms.Button btnMy1;
        private System.Windows.Forms.Button btnMy2;
        private System.Windows.Forms.TextBox txtNumRegistries;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtThreads;
        private System.Windows.Forms.Button btnMy3;
        private System.Windows.Forms.Button btnORA3;
        private System.Windows.Forms.Button btnTestPG3;
        private System.Windows.Forms.Button btnTestMy4;
        private System.Windows.Forms.Button btnTestOra4;
        private System.Windows.Forms.Button btnTestPG4;
    }
}

