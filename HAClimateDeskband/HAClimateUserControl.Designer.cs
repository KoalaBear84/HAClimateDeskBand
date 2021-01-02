﻿
namespace HAClimateDeskband
{
    partial class HAClimateUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LblInfo = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LblTemperature = new System.Windows.Forms.Label();
            this.PictureOff = new System.Windows.Forms.PictureBox();
            this.PicturePause = new System.Windows.Forms.PictureBox();
            this.PictureFire = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicturePause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureFire)).BeginInit();
            this.SuspendLayout();
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LblInfo.AutoSize = true;
            this.LblInfo.BackColor = System.Drawing.Color.Transparent;
            this.LblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInfo.ForeColor = System.Drawing.Color.White;
            this.LblInfo.Location = new System.Drawing.Point(-1, 3);
            this.LblInfo.Margin = new System.Windows.Forms.Padding(0);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(52, 12);
            this.LblInfo.TabIndex = 4;
            this.LblInfo.Text = "Initializing..";
            this.LblInfo.Click += new System.EventHandler(this.LblInfo_Click);
            // 
            // Timer
            // 
            this.Timer.Interval = 15000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // ToolTip
            // 
            this.ToolTip.IsBalloon = true;
            // 
            // LblTemperature
            // 
            this.LblTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LblTemperature.AutoSize = true;
            this.LblTemperature.BackColor = System.Drawing.Color.Transparent;
            this.LblTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTemperature.ForeColor = System.Drawing.Color.White;
            this.LblTemperature.Location = new System.Drawing.Point(12, 47);
            this.LblTemperature.Name = "LblTemperature";
            this.LblTemperature.Size = new System.Drawing.Size(34, 12);
            this.LblTemperature.TabIndex = 9;
            this.LblTemperature.Text = "12.5°C";
            this.LblTemperature.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LblTemperature_MouseDown);
            // 
            // PictureOff
            // 
            this.PictureOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureOff.BackColor = System.Drawing.Color.Transparent;
            this.PictureOff.Cursor = System.Windows.Forms.Cursors.Default;
            this.PictureOff.Image = global::HAClimateDeskband.Properties.Resources.Power;
            this.PictureOff.Location = new System.Drawing.Point(28, 5);
            this.PictureOff.Name = "PictureOff";
            this.PictureOff.Size = new System.Drawing.Size(16, 16);
            this.PictureOff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureOff.TabIndex = 8;
            this.PictureOff.TabStop = false;
            this.PictureOff.Click += new System.EventHandler(this.Picture_Click);
            // 
            // PicturePause
            // 
            this.PicturePause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PicturePause.BackColor = System.Drawing.Color.Transparent;
            this.PicturePause.Cursor = System.Windows.Forms.Cursors.Default;
            this.PicturePause.Image = global::HAClimateDeskband.Properties.Resources.Pause;
            this.PicturePause.Location = new System.Drawing.Point(28, 5);
            this.PicturePause.Name = "PicturePause";
            this.PicturePause.Size = new System.Drawing.Size(16, 16);
            this.PicturePause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicturePause.TabIndex = 7;
            this.PicturePause.TabStop = false;
            this.PicturePause.Click += new System.EventHandler(this.Picture_Click);
            // 
            // PictureFire
            // 
            this.PictureFire.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureFire.BackColor = System.Drawing.Color.Transparent;
            this.PictureFire.Cursor = System.Windows.Forms.Cursors.Default;
            this.PictureFire.Image = global::HAClimateDeskband.Properties.Resources.Flame;
            this.PictureFire.Location = new System.Drawing.Point(28, 5);
            this.PictureFire.Name = "PictureFire";
            this.PictureFire.Size = new System.Drawing.Size(16, 16);
            this.PictureFire.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureFire.TabIndex = 6;
            this.PictureFire.TabStop = false;
            this.PictureFire.Click += new System.EventHandler(this.Picture_Click);
            // 
            // HAClimateUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.LblTemperature);
            this.Controls.Add(this.PictureOff);
            this.Controls.Add(this.PicturePause);
            this.Controls.Add(this.PictureFire);
            this.Controls.Add(this.LblInfo);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "HAClimateUserControl";
            this.Size = new System.Drawing.Size(43, 60);
            this.Load += new System.EventHandler(this.HomeAssistantUserControl_Load);
            this.Resize += new System.EventHandler(this.HomeAssistantUserControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.PictureOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicturePause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureFire)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.PictureBox PictureFire;
        private System.Windows.Forms.PictureBox PicturePause;
        private System.Windows.Forms.PictureBox PictureOff;
        private System.Windows.Forms.Label LblTemperature;
    }
}