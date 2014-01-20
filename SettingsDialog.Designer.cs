namespace GitSpy
{
    partial class SettingsDialog
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericUpDownUpdateInterval = new System.Windows.Forms.NumericUpDown();
            this.textBoxSettingsPath = new System.Windows.Forms.TextBox();
            this.labelSettingsPath = new System.Windows.Forms.Label();
            this.labelUpdateInterval = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUpdateInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(195, 95);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&Save";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(276, 95);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // numericUpDownUpdateInterval
            // 
            this.numericUpDownUpdateInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownUpdateInterval.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::GitSpy.Properties.Settings.Default, "UpdateInterval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownUpdateInterval.Location = new System.Drawing.Point(12, 64);
            this.numericUpDownUpdateInterval.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownUpdateInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownUpdateInterval.Name = "numericUpDownUpdateInterval";
            this.numericUpDownUpdateInterval.Size = new System.Drawing.Size(339, 20);
            this.numericUpDownUpdateInterval.TabIndex = 2;
            this.numericUpDownUpdateInterval.Value = global::GitSpy.Properties.Settings.Default.UpdateInterval;
            // 
            // textBoxSettingsPath
            // 
            this.textBoxSettingsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSettingsPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GitSpy.Properties.Settings.Default, "RepositoryFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSettingsPath.Location = new System.Drawing.Point(12, 25);
            this.textBoxSettingsPath.Name = "textBoxSettingsPath";
            this.textBoxSettingsPath.Size = new System.Drawing.Size(339, 20);
            this.textBoxSettingsPath.TabIndex = 4;
            this.textBoxSettingsPath.Text = global::GitSpy.Properties.Settings.Default.RepositoryFile;
            // 
            // labelSettingsPath
            // 
            this.labelSettingsPath.AutoSize = true;
            this.labelSettingsPath.Location = new System.Drawing.Point(12, 9);
            this.labelSettingsPath.Name = "labelSettingsPath";
            this.labelSettingsPath.Size = new System.Drawing.Size(72, 13);
            this.labelSettingsPath.TabIndex = 5;
            this.labelSettingsPath.Text = "Settings path:";
            // 
            // labelUpdateInterval
            // 
            this.labelUpdateInterval.AutoSize = true;
            this.labelUpdateInterval.Location = new System.Drawing.Point(12, 48);
            this.labelUpdateInterval.Name = "labelUpdateInterval";
            this.labelUpdateInterval.Size = new System.Drawing.Size(132, 13);
            this.labelUpdateInterval.TabIndex = 6;
            this.labelUpdateInterval.Text = "Update interval in minutes:";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 130);
            this.Controls.Add(this.labelUpdateInterval);
            this.Controls.Add(this.labelSettingsPath);
            this.Controls.Add(this.textBoxSettingsPath);
            this.Controls.Add(this.numericUpDownUpdateInterval);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GitSpy Settings";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUpdateInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numericUpDownUpdateInterval;
        private System.Windows.Forms.TextBox textBoxSettingsPath;
        private System.Windows.Forms.Label labelSettingsPath;
        private System.Windows.Forms.Label labelUpdateInterval;
    }
}

