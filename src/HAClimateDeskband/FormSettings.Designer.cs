
namespace HAClimateDeskband
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.BtnSave = new System.Windows.Forms.Button();
            this.LblApiBaseUrl = new System.Windows.Forms.Label();
            this.TxtApiBaseUrl = new System.Windows.Forms.TextBox();
            this.TxtApiKey = new System.Windows.Forms.TextBox();
            this.LblApiKey = new System.Windows.Forms.Label();
            this.TxtEntityIdClimate = new System.Windows.Forms.TextBox();
            this.LblEntityIdClimate = new System.Windows.Forms.Label();
            this.TxtEntityIdTemperature = new System.Windows.Forms.TextBox();
            this.LblEntityIdTemperature = new System.Windows.Forms.Label();
            this.TxtEntityIdPowerUsage = new System.Windows.Forms.TextBox();
            this.LblEntityIdPowerUsage = new System.Windows.Forms.Label();
            this.BtnTest = new System.Windows.Forms.Button();
            this.ChkPreferLastChangeAndPowerUsage = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(349, 167);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 25);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // LblApiBaseUrl
            // 
            this.LblApiBaseUrl.AutoSize = true;
            this.LblApiBaseUrl.Location = new System.Drawing.Point(12, 15);
            this.LblApiBaseUrl.Name = "LblApiBaseUrl";
            this.LblApiBaseUrl.Size = new System.Drawing.Size(70, 13);
            this.LblApiBaseUrl.TabIndex = 1;
            this.LblApiBaseUrl.Text = "API Base Url:";
            // 
            // TxtApiBaseUrl
            // 
            this.TxtApiBaseUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtApiBaseUrl.Location = new System.Drawing.Point(133, 12);
            this.TxtApiBaseUrl.Name = "TxtApiBaseUrl";
            this.TxtApiBaseUrl.Size = new System.Drawing.Size(291, 20);
            this.TxtApiBaseUrl.TabIndex = 2;
            // 
            // TxtApiKey
            // 
            this.TxtApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtApiKey.Location = new System.Drawing.Point(133, 38);
            this.TxtApiKey.Name = "TxtApiKey";
            this.TxtApiKey.Size = new System.Drawing.Size(291, 20);
            this.TxtApiKey.TabIndex = 4;
            // 
            // LblApiKey
            // 
            this.LblApiKey.AutoSize = true;
            this.LblApiKey.Location = new System.Drawing.Point(12, 41);
            this.LblApiKey.Name = "LblApiKey";
            this.LblApiKey.Size = new System.Drawing.Size(48, 13);
            this.LblApiKey.TabIndex = 3;
            this.LblApiKey.Text = "API Key:";
            // 
            // TxtEntityIdClimate
            // 
            this.TxtEntityIdClimate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtEntityIdClimate.Location = new System.Drawing.Point(133, 64);
            this.TxtEntityIdClimate.Name = "TxtEntityIdClimate";
            this.TxtEntityIdClimate.Size = new System.Drawing.Size(291, 20);
            this.TxtEntityIdClimate.TabIndex = 6;
            // 
            // LblEntityIdClimate
            // 
            this.LblEntityIdClimate.AutoSize = true;
            this.LblEntityIdClimate.Location = new System.Drawing.Point(12, 67);
            this.LblEntityIdClimate.Name = "LblEntityIdClimate";
            this.LblEntityIdClimate.Size = new System.Drawing.Size(85, 13);
            this.LblEntityIdClimate.TabIndex = 5;
            this.LblEntityIdClimate.Text = "Climate Entity Id:";
            // 
            // TxtEntityIdTemperature
            // 
            this.TxtEntityIdTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtEntityIdTemperature.Location = new System.Drawing.Point(133, 90);
            this.TxtEntityIdTemperature.Name = "TxtEntityIdTemperature";
            this.TxtEntityIdTemperature.Size = new System.Drawing.Size(291, 20);
            this.TxtEntityIdTemperature.TabIndex = 8;
            // 
            // LblEntityIdTemperature
            // 
            this.LblEntityIdTemperature.AutoSize = true;
            this.LblEntityIdTemperature.Location = new System.Drawing.Point(12, 93);
            this.LblEntityIdTemperature.Name = "LblEntityIdTemperature";
            this.LblEntityIdTemperature.Size = new System.Drawing.Size(111, 13);
            this.LblEntityIdTemperature.TabIndex = 7;
            this.LblEntityIdTemperature.Text = "Temperature Entity Id:";
            // 
            // TxtEntityIdPowerUsage
            // 
            this.TxtEntityIdPowerUsage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtEntityIdPowerUsage.Location = new System.Drawing.Point(133, 116);
            this.TxtEntityIdPowerUsage.Name = "TxtEntityIdPowerUsage";
            this.TxtEntityIdPowerUsage.Size = new System.Drawing.Size(291, 20);
            this.TxtEntityIdPowerUsage.TabIndex = 10;
            // 
            // LblEntityIdPowerUsage
            // 
            this.LblEntityIdPowerUsage.AutoSize = true;
            this.LblEntityIdPowerUsage.Location = new System.Drawing.Point(12, 119);
            this.LblEntityIdPowerUsage.Name = "LblEntityIdPowerUsage";
            this.LblEntityIdPowerUsage.Size = new System.Drawing.Size(115, 13);
            this.LblEntityIdPowerUsage.TabIndex = 9;
            this.LblEntityIdPowerUsage.Text = "Power Usage Entity Id:";
            // 
            // BtnTest
            // 
            this.BtnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnTest.Location = new System.Drawing.Point(268, 167);
            this.BtnTest.Name = "BtnTest";
            this.BtnTest.Size = new System.Drawing.Size(75, 25);
            this.BtnTest.TabIndex = 0;
            this.BtnTest.Text = "Test";
            this.BtnTest.UseVisualStyleBackColor = true;
            this.BtnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // ChkPreferLastChangeAndPowerUsage
            // 
            this.ChkPreferLastChangeAndPowerUsage.AutoSize = true;
            this.ChkPreferLastChangeAndPowerUsage.Location = new System.Drawing.Point(15, 142);
            this.ChkPreferLastChangeAndPowerUsage.Name = "ChkPreferLastChangeAndPowerUsage";
            this.ChkPreferLastChangeAndPowerUsage.Size = new System.Drawing.Size(205, 17);
            this.ChkPreferLastChangeAndPowerUsage.TabIndex = 11;
            this.ChkPreferLastChangeAndPowerUsage.Text = "Prefer Last Change and Power Usage";
            this.ChkPreferLastChangeAndPowerUsage.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 204);
            this.Controls.Add(this.ChkPreferLastChangeAndPowerUsage);
            this.Controls.Add(this.TxtEntityIdPowerUsage);
            this.Controls.Add(this.LblEntityIdPowerUsage);
            this.Controls.Add(this.TxtEntityIdTemperature);
            this.Controls.Add(this.LblEntityIdTemperature);
            this.Controls.Add(this.TxtEntityIdClimate);
            this.Controls.Add(this.LblEntityIdClimate);
            this.Controls.Add(this.TxtApiKey);
            this.Controls.Add(this.LblApiKey);
            this.Controls.Add(this.TxtApiBaseUrl);
            this.Controls.Add(this.LblApiBaseUrl);
            this.Controls.Add(this.BtnTest);
            this.Controls.Add(this.BtnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HA Climate DeskBand";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormSettings_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label LblApiBaseUrl;
        private System.Windows.Forms.TextBox TxtApiBaseUrl;
        private System.Windows.Forms.TextBox TxtApiKey;
        private System.Windows.Forms.Label LblApiKey;
        private System.Windows.Forms.TextBox TxtEntityIdClimate;
        private System.Windows.Forms.Label LblEntityIdClimate;
        private System.Windows.Forms.TextBox TxtEntityIdTemperature;
        private System.Windows.Forms.Label LblEntityIdTemperature;
        private System.Windows.Forms.TextBox TxtEntityIdPowerUsage;
        private System.Windows.Forms.Label LblEntityIdPowerUsage;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.CheckBox ChkPreferLastChangeAndPowerUsage;
    }
}