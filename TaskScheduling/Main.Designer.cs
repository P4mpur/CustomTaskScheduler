namespace TaskScheduling
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
			this.components = new System.ComponentModel.Container();
			this.CurrentWorkersLabel = new System.Windows.Forms.Label();
			this.MaximumWorkersLabel = new System.Windows.Forms.Label();
			this.MaximumWorkersUpDown = new System.Windows.Forms.NumericUpDown();
			this.StatusTimer = new System.Windows.Forms.Timer(this.components);
			this.EnqueueWorkButton = new System.Windows.Forms.Button();
			this.WaitingWorkItemsLabel = new System.Windows.Forms.Label();
			this.AvailableWorkersLabel = new System.Windows.Forms.Label();
			this.PendingWorkItemsLabel = new System.Windows.Forms.Label();
			this.ProgressPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.MaximumWorkersUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// CurrentWorkersLabel
			// 
			this.CurrentWorkersLabel.AutoSize = true;
			this.CurrentWorkersLabel.Location = new System.Drawing.Point(12, 9);
			this.CurrentWorkersLabel.Name = "CurrentWorkersLabel";
			this.CurrentWorkersLabel.Size = new System.Drawing.Size(104, 13);
			this.CurrentWorkersLabel.TabIndex = 0;
			this.CurrentWorkersLabel.Text = "Trenutni broj Radnika: {0}";
			// 
			// MaximumWorkersLabel
			// 
			this.MaximumWorkersLabel.AutoSize = true;
			this.MaximumWorkersLabel.Location = new System.Drawing.Point(8, 60);
			this.MaximumWorkersLabel.Name = "MaximumWorkersLabel";
			this.MaximumWorkersLabel.Size = new System.Drawing.Size(110, 13);
			this.MaximumWorkersLabel.TabIndex = 2;
			this.MaximumWorkersLabel.Text = "Maksimalni broj radnika:";
			// 
			// MaximumWorkersUpDown
			// 
			this.MaximumWorkersUpDown.Location = new System.Drawing.Point(120, 58);
			this.MaximumWorkersUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.MaximumWorkersUpDown.Name = "MaximumWorkersUpDown";
			this.MaximumWorkersUpDown.Size = new System.Drawing.Size(40, 20);
			this.MaximumWorkersUpDown.TabIndex = 3;
			this.MaximumWorkersUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.MaximumWorkersUpDown.ValueChanged += new System.EventHandler(this.MaximumWorkersUpDown_ValueChanged);
			// 
			// StatusTimer
			// 
			this.StatusTimer.Interval = 50;
			this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
			// 
			// EnqueueWorkButton
			// 
			this.EnqueueWorkButton.Location = new System.Drawing.Point(207, 110);
			this.EnqueueWorkButton.Name = "EnqueueWorkButton";
			this.EnqueueWorkButton.Size = new System.Drawing.Size(120, 23);
			this.EnqueueWorkButton.TabIndex = 4;
			this.EnqueueWorkButton.Text = "Zapocni";
			this.EnqueueWorkButton.UseVisualStyleBackColor = true;
			this.EnqueueWorkButton.Click += new System.EventHandler(this.EnqueueWorkButton_Click);
			// 
			// WaitingWorkItemsLabel
			// 
			this.WaitingWorkItemsLabel.AutoSize = true;
			this.WaitingWorkItemsLabel.Location = new System.Drawing.Point(5, 92);
			this.WaitingWorkItemsLabel.Name = "WaitingWorkItemsLabel";
			this.WaitingWorkItemsLabel.Size = new System.Drawing.Size(120, 13);
			this.WaitingWorkItemsLabel.TabIndex = 5;
			this.WaitingWorkItemsLabel.Text = "Broj Taskova na cekanju: {0}";
			// 
			// AvailableWorkersLabel
			// 
			this.AvailableWorkersLabel.AutoSize = true;
			this.AvailableWorkersLabel.Location = new System.Drawing.Point(12, 35);
			this.AvailableWorkersLabel.Name = "AvailableWorkersLabel";
			this.AvailableWorkersLabel.Size = new System.Drawing.Size(113, 13);
			this.AvailableWorkersLabel.TabIndex = 6;
			this.AvailableWorkersLabel.Text = "Dostupni Radnici: {0}";
			// 
			// PendingWorkItemsLabel
			// 
			this.PendingWorkItemsLabel.AutoSize = true;
			this.PendingWorkItemsLabel.Location = new System.Drawing.Point(12, 115);
			this.PendingWorkItemsLabel.Name = "PendingWorkItemsLabel";
			this.PendingWorkItemsLabel.Size = new System.Drawing.Size(123, 13);
			this.PendingWorkItemsLabel.TabIndex = 7;
			this.PendingWorkItemsLabel.Text = "Taskovi u toku: {0}";
			// 
			// ProgressPanel
			// 
			this.ProgressPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProgressPanel.AutoScroll = true;
			this.ProgressPanel.Location = new System.Drawing.Point(12, 139);
			this.ProgressPanel.Name = "ProgressPanel";
			this.ProgressPanel.Size = new System.Drawing.Size(311, 100);
			this.ProgressPanel.TabIndex = 8;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(335, 245);
			this.Controls.Add(this.ProgressPanel);
			this.Controls.Add(this.PendingWorkItemsLabel);
			this.Controls.Add(this.AvailableWorkersLabel);
			this.Controls.Add(this.WaitingWorkItemsLabel);
			this.Controls.Add(this.EnqueueWorkButton);
			this.Controls.Add(this.MaximumWorkersUpDown);
			this.Controls.Add(this.MaximumWorkersLabel);
			this.Controls.Add(this.CurrentWorkersLabel);
			this.Name = "Main";
			this.Text = "Task Scheduling Demo";
			((System.ComponentModel.ISupportInitialize)(this.MaximumWorkersUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label CurrentWorkersLabel;
		private System.Windows.Forms.Label MaximumWorkersLabel;
		private System.Windows.Forms.NumericUpDown MaximumWorkersUpDown;
		private System.Windows.Forms.Timer StatusTimer;
		private System.Windows.Forms.Button EnqueueWorkButton;
		private System.Windows.Forms.Label WaitingWorkItemsLabel;
		private System.Windows.Forms.Label AvailableWorkersLabel;
		private System.Windows.Forms.Label PendingWorkItemsLabel;
		private System.Windows.Forms.Panel ProgressPanel;
	}
}

