namespace Multithreading_06
{
    partial class MainForm
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
            this.BilliardTable = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.ButtonRestart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LabelTime = new System.Windows.Forms.Label();
            this.LabelPoints = new System.Windows.Forms.Label();
            this.LabelHits = new System.Windows.Forms.Label();
            this.BilliardTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // BilliardTable
            // 
            this.BilliardTable.BackgroundImage = global::Multithreading_06.Properties.Resources.billiardTable;
            this.BilliardTable.Controls.Add(this.panel1);
            this.BilliardTable.Location = new System.Drawing.Point(0, 36);
            this.BilliardTable.Name = "BilliardTable";
            this.BilliardTable.Size = new System.Drawing.Size(1024, 512);
            this.BilliardTable.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(35, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 442);
            this.panel1.TabIndex = 0;
            // 
            // ButtonStart
            // 
            this.ButtonStart.BackColor = System.Drawing.Color.SaddleBrown;
            this.ButtonStart.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonStart.Location = new System.Drawing.Point(12, 554);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(165, 47);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = false;
            // 
            // ButtonRestart
            // 
            this.ButtonRestart.BackColor = System.Drawing.Color.SaddleBrown;
            this.ButtonRestart.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonRestart.Location = new System.Drawing.Point(183, 554);
            this.ButtonRestart.Name = "ButtonRestart";
            this.ButtonRestart.Size = new System.Drawing.Size(165, 47);
            this.ButtonRestart.TabIndex = 2;
            this.ButtonRestart.Text = "Restart";
            this.ButtonRestart.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "Time: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(268, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hits:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(140, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "Points:";
            // 
            // LabelTime
            // 
            this.LabelTime.AutoSize = true;
            this.LabelTime.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTime.Location = new System.Drawing.Point(74, 9);
            this.LabelTime.Name = "LabelTime";
            this.LabelTime.Size = new System.Drawing.Size(60, 22);
            this.LabelTime.TabIndex = 6;
            this.LabelTime.Text = "00:00";
            // 
            // LabelPoints
            // 
            this.LabelPoints.AutoSize = true;
            this.LabelPoints.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelPoints.Location = new System.Drawing.Point(226, 9);
            this.LabelPoints.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.LabelPoints.Name = "LabelPoints";
            this.LabelPoints.Size = new System.Drawing.Size(21, 22);
            this.LabelPoints.TabIndex = 7;
            this.LabelPoints.Text = "0";
            // 
            // LabelHits
            // 
            this.LabelHits.AutoSize = true;
            this.LabelHits.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelHits.Location = new System.Drawing.Point(327, 9);
            this.LabelHits.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.LabelHits.Name = "LabelHits";
            this.LabelHits.Size = new System.Drawing.Size(21, 22);
            this.LabelHits.TabIndex = 8;
            this.LabelHits.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(1024, 609);
            this.Controls.Add(this.LabelHits);
            this.Controls.Add(this.LabelPoints);
            this.Controls.Add(this.LabelTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonRestart);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.BilliardTable);
            this.Name = "MainForm";
            this.Text = "Multithreading_06";
            this.BilliardTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel BilliardTable;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonRestart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LabelTime;
        private System.Windows.Forms.Label LabelPoints;
        private System.Windows.Forms.Label LabelHits;
    }
}

