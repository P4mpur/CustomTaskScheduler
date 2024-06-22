using System;
using System.Windows.Forms;

namespace TaskScheduling
{
	public partial class WorkerProgressControl : UserControl
	{
		public WorkerProgressControl()
		{
			InitializeComponent();
		}
		public WorkerProgressControl(int taskId)
		{
			InitializeComponent();
			TaskIdLabel.Text = string.Format(TaskIdLabel.Text,taskId);
		}
		public float Value {
			get {
				return Value / 100f;
			}
			set {
				TaskProgessBar.Visible = true;
				TaskProgessBar.Value = (int)Math.Round(value * 100);
			}
		}
	}
}
