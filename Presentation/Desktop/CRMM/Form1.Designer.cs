namespace CRMM
{
    partial class MainForm
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.FLP = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // FLP
            // 
            this.FLP.AutoSize = true;
            this.FLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FLP.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FLP.Location = new System.Drawing.Point(0, 0);
            this.FLP.Name = "FLP";
            this.FLP.Size = new System.Drawing.Size(800, 450);
            this.FLP.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.FLP);
            this.ForeColor = System.Drawing.Color.Orange;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "CRMM";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FLP;
    }
}

