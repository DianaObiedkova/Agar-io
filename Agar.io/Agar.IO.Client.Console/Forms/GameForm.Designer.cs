namespace Agar.IO.Client.WinForms.Forms
{
    partial class GameForm
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
            this.GamePanel = new System.Windows.Forms.PictureBox();
            this.labelScores = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GamePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // GamePanel
            // 
            this.GamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GamePanel.Location = new System.Drawing.Point(0, 0);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(800, 450);
            this.GamePanel.TabIndex = 0;
            this.GamePanel.TabStop = false;
            // 
            // labelScores
            // 
            this.labelScores.AutoSize = true;
            this.labelScores.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelScores.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelScores.Location = new System.Drawing.Point(647, 9);
            this.labelScores.Name = "labelScores";
            this.labelScores.Size = new System.Drawing.Size(70, 25);
            this.labelScores.TabIndex = 1;
            this.labelScores.Text = "Scores";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelScores);
            this.Controls.Add(this.GamePanel);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.Load += new System.EventHandler(this.GameForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GamePanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox GamePanel;
        public System.Windows.Forms.Label labelScores;
    }
}