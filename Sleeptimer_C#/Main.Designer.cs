namespace Sleeptimer_C_
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.hour_label = new System.Windows.Forms.Label();
            this.minute_label = new System.Windows.Forms.Label();
            this.hour_updown = new System.Windows.Forms.NumericUpDown();
            this.minute_updown = new System.Windows.Forms.NumericUpDown();
            this.second_label = new System.Windows.Forms.Label();
            this.second_updown = new System.Windows.Forms.NumericUpDown();
            this.sleep_button = new System.Windows.Forms.Button();
            this.abort_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.hour_updown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minute_updown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.second_updown)).BeginInit();
            this.SuspendLayout();
            // 
            // hour_label
            // 
            this.hour_label.AutoSize = true;
            this.hour_label.Location = new System.Drawing.Point(12, 12);
            this.hour_label.Name = "hour_label";
            this.hour_label.Size = new System.Drawing.Size(94, 31);
            this.hour_label.TabIndex = 1;
            this.hour_label.Text = "Hours";
            // 
            // minute_label
            // 
            this.minute_label.AutoSize = true;
            this.minute_label.Location = new System.Drawing.Point(177, 12);
            this.minute_label.Name = "minute_label";
            this.minute_label.Size = new System.Drawing.Size(126, 31);
            this.minute_label.TabIndex = 2;
            this.minute_label.Text = "Minutes";
            // 
            // hour_updown
            // 
            this.hour_updown.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.hour_updown.Location = new System.Drawing.Point(112, 10);
            this.hour_updown.Name = "hour_updown";
            this.hour_updown.ReadOnly = true;
            this.hour_updown.Size = new System.Drawing.Size(59, 38);
            this.hour_updown.TabIndex = 4;
            // 
            // minute_updown
            // 
            this.minute_updown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.minute_updown.Location = new System.Drawing.Point(309, 10);
            this.minute_updown.Name = "minute_updown";
            this.minute_updown.ReadOnly = true;
            this.minute_updown.Size = new System.Drawing.Size(59, 38);
            this.minute_updown.TabIndex = 5;
            this.minute_updown.ValueChanged += new System.EventHandler(this.minute_updown_ValueChanged);
            // 
            // second_label
            // 
            this.second_label.AutoSize = true;
            this.second_label.Location = new System.Drawing.Point(374, 12);
            this.second_label.Name = "second_label";
            this.second_label.Size = new System.Drawing.Size(126, 31);
            this.second_label.TabIndex = 6;
            this.second_label.Text = "Seconds";
            // 
            // second_updown
            // 
            this.second_updown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.second_updown.Location = new System.Drawing.Point(506, 12);
            this.second_updown.Name = "second_updown";
            this.second_updown.ReadOnly = true;
            this.second_updown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.second_updown.Size = new System.Drawing.Size(59, 38);
            this.second_updown.TabIndex = 7;
            this.second_updown.ValueChanged += new System.EventHandler(this.second_updown_ValueChanged);
            // 
            // sleep_button
            // 
            this.sleep_button.Location = new System.Drawing.Point(18, 56);
            this.sleep_button.Name = "sleep_button";
            this.sleep_button.Size = new System.Drawing.Size(547, 39);
            this.sleep_button.TabIndex = 8;
            this.sleep_button.Text = "Sleep";
            this.sleep_button.UseVisualStyleBackColor = true;
            this.sleep_button.Click += new System.EventHandler(this.sleep_button_Click);
            // 
            // abort_button
            // 
            this.abort_button.Location = new System.Drawing.Point(18, 101);
            this.abort_button.Name = "abort_button";
            this.abort_button.Size = new System.Drawing.Size(547, 39);
            this.abort_button.TabIndex = 9;
            this.abort_button.Text = "Abort Sleep";
            this.abort_button.UseVisualStyleBackColor = true;
            this.abort_button.Click += new System.EventHandler(this.abort_button_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 145);
            this.Controls.Add(this.abort_button);
            this.Controls.Add(this.sleep_button);
            this.Controls.Add(this.second_updown);
            this.Controls.Add(this.second_label);
            this.Controls.Add(this.minute_updown);
            this.Controls.Add(this.hour_updown);
            this.Controls.Add(this.minute_label);
            this.Controls.Add(this.hour_label);
            this.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Main";
            this.Text = "Sleeptimer";
            ((System.ComponentModel.ISupportInitialize)(this.hour_updown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minute_updown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.second_updown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label hour_label;
        private System.Windows.Forms.Label minute_label;
        private System.Windows.Forms.NumericUpDown hour_updown;
        private System.Windows.Forms.NumericUpDown minute_updown;
        private System.Windows.Forms.Label second_label;
        private System.Windows.Forms.NumericUpDown second_updown;
        private System.Windows.Forms.Button sleep_button;
        private System.Windows.Forms.Button abort_button;
    }
}

