namespace TaskScheduling
{
	partial class WorkerProgressControl
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
			this.TaskIdLabel = new System.Windows.Forms.Label();
			this.TaskProgessBar = new System.Windows.Forms.ProgressBar();
			this.WaitingInQueueLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// TaskIdLabel
			// 
			this.TaskIdLabel.AutoSize = true;
			this.TaskIdLabel.Location = new System.Drawing.Point(2, 4);
			this.TaskIdLabel.Name = "TaskIdLabel";
			this.TaskIdLabel.Size = new System.Drawing.Size(48, 13);
			this.TaskIdLabel.TabIndex = 0;
			this.TaskIdLabel.Text = "Task {0}";
			// 
			// TaskProgessBar
			// 
			this.TaskProgessBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TaskProgessBar.Location = new System.Drawing.Point(70, 0);
			this.TaskProgessBar.Name = "TaskProgessBar";
			this.TaskProgessBar.Size = new System.Drawing.Size(112, 23);
			this.TaskProgessBar.TabIndex = 1;
			this.TaskProgessBar.Visible = false;
			// 
			// WaitingInQueueLabel
			// 
			this.WaitingInQueueLabel.AutoSize = true;
			this.WaitingInQueueLabel.Location = new System.Drawing.Point(81, 4);
			this.WaitingInQueueLabel.Name = "WaitingInQueueLabel";
			this.WaitingInQueueLabel.Size = new System.Drawing.Size(87, 13);
			this.WaitingInQueueLabel.TabIndex = 2;
			this.WaitingInQueueLabel.Text = "Cekam...";
			// 
			// WorkerProgressControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TaskProgessBar);
			this.Controls.Add(this.TaskIdLabel);
			this.Controls.Add(this.WaitingInQueueLabel);
			this.Name = "WorkerProgressControl";
			this.Size = new System.Drawing.Size(186, 23);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label TaskIdLabel;
		private System.Windows.Forms.ProgressBar TaskProgessBar;
		private System.Windows.Forms.Label WaitingInQueueLabel;
	}
}
