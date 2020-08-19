namespace Multithreading_05
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
            this.PnlBilliardTable = new System.Windows.Forms.Panel();
            this.PnlGame = new System.Windows.Forms.Panel();
            this.lblGameState = new System.Windows.Forms.Label();
            this.BtnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblPoints = new System.Windows.Forms.Label();
            this.LblHits = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.PnlBilliardTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBilliardTable
            // 
            this.PnlBilliardTable.BackgroundImage = Properties.Resources.billiardTable;
            this.PnlBilliardTable.Controls.Add(this.PnlGame);
            this.PnlBilliardTable.Location = new System.Drawing.Point(0, 36);
            this.PnlBilliardTable.Name = "PnlBilliardTable";
            this.PnlBilliardTable.Size = new System.Drawing.Size(1024, 512);
            this.PnlBilliardTable.TabIndex = 0;
            // 
            // PnlGame
            // 
            this.PnlGame.BackColor = System.Drawing.Color.Transparent;
            this.PnlGame.Location = new System.Drawing.Point(37, 37);
            this.PnlGame.Name = "PnlGame";
            this.PnlGame.Size = new System.Drawing.Size(951, 439);
            this.PnlGame.TabIndex = 0;
            this.PnlGame.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlGame_Paint);
            this.PnlGame.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PnlGame_MouseClick);
            this.PnlGame.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PnlGame_MouseDoubleClick);
            // 
            // lblGameState
            // 
            this.lblGameState.BackColor = System.Drawing.Color.Transparent;
            this.lblGameState.Font = new System.Drawing.Font("Stencil", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameState.ForeColor = System.Drawing.Color.Black;
            this.lblGameState.Location = new System.Drawing.Point(731, 4);
            this.lblGameState.Name = "lblGameState";
            this.lblGameState.Size = new System.Drawing.Size(281, 32);
            this.lblGameState.TabIndex = 9;
            this.lblGameState.Text = "YOU LOSE";
            this.lblGameState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnStart
            // 
            this.BtnStart.BackColor = System.Drawing.Color.SaddleBrown;
            this.BtnStart.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStart.Location = new System.Drawing.Point(12, 554);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(165, 47);
            this.BtnStart.TabIndex = 1;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = false;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(121, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hits Left:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "Points:";
            // 
            // LblPoints
            // 
            this.LblPoints.AutoSize = true;
            this.LblPoints.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPoints.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LblPoints.Location = new System.Drawing.Point(85, 9);
            this.LblPoints.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.LblPoints.Name = "LblPoints";
            this.LblPoints.Size = new System.Drawing.Size(21, 22);
            this.LblPoints.TabIndex = 7;
            this.LblPoints.Text = "0";
            // 
            // LblHits
            // 
            this.LblHits.AutoSize = true;
            this.LblHits.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHits.Location = new System.Drawing.Point(223, 9);
            this.LblHits.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.LblHits.Name = "LblHits";
            this.LblHits.Size = new System.Drawing.Size(21, 22);
            this.LblHits.TabIndex = 8;
            this.LblHits.Text = "0";
            // 
            // BtnStop
            // 
            this.BtnStop.BackColor = System.Drawing.Color.SaddleBrown;
            this.BtnStop.Enabled = false;
            this.BtnStop.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStop.Location = new System.Drawing.Point(183, 554);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(165, 47);
            this.BtnStop.TabIndex = 10;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(1024, 609);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.lblGameState);
            this.Controls.Add(this.LblHits);
            this.Controls.Add(this.LblPoints);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.PnlBilliardTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Multithreading_05";
            this.PnlBilliardTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PnlBilliardTable;
        private System.Windows.Forms.Panel PnlGame;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LblPoints;
        private System.Windows.Forms.Label LblHits;
        private System.Windows.Forms.Label lblGameState;
        private System.Windows.Forms.Button BtnStop;
    }
}

