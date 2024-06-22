using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace TaskScheduling
{
	public partial class Main : Form
	{
		const int MSG_WORK = 1;
		Client _client = new Client();
		string _currentWorkersFmt;
		string _waitingWorkItemsFmt;
		string _pendingWorkItemsFmt;
		string _availableWorkersFmt;
		int _oldMaximumWorkerCount;
		Dictionary<int, WorkerProgressControl> _progressMap = new Dictionary<int, WorkerProgressControl>();
		public Main()
		{
			// designer support:
			InitializeComponent();
			_currentWorkersFmt = CurrentWorkersLabel.Text;
			_waitingWorkItemsFmt = WaitingWorkItemsLabel.Text;
			_pendingWorkItemsFmt = PendingWorkItemsLabel.Text;
			_availableWorkersFmt = AvailableWorkersLabel.Text;

            // prikaci klijentov progres
			_client.WorkerMessageProgress += _client_WorkerMessageProgress;

            // postavi maksimalni worker sa default max worker
			MaximumWorkersUpDown.Value = _client.MaximumWorkerCount;
            // ovo je u slucaju kada smo smanjili broj maksimalni radnika
			_oldMaximumWorkerCount = _client.MaximumWorkerCount;
			
            // pokreni klijentov thread da bi pocelo da procesuira poruke
			new Thread(() => { _client.Start(); }).Start();
			
			// pokreni timer 
			StatusTimer.Enabled = true;
		}

		private void _client_WorkerMessageProgress(object sender, Client.WorkerProgressEventArgs args)
		{
			BeginInvoke(new Action(delegate () {
				WorkerProgressControl wpc;
				if(_progressMap.TryGetValue(args.Id,out wpc))
				{
					wpc.Value = args.Progress;
				}
			}));
		}

		private void StatusTimer_Tick(object sender, EventArgs e)
		{
			CurrentWorkersLabel.Text = string.Format(_currentWorkersFmt, _client.WorkerCount);
			WaitingWorkItemsLabel.Text = string.Format(_waitingWorkItemsFmt, _client.WaitingWorkerMessageCount);
			PendingWorkItemsLabel.Text = string.Format(_pendingWorkItemsFmt, _client.PendingWorkerMessageCount);
			AvailableWorkersLabel.Text = string.Format(_availableWorkersFmt, _client.AvailableWorkerCount);
			
		}

		private void EnqueueWorkButton_Click(object sender, EventArgs e)
		{
			var id = _client.DispatchWorkerMessage(new WorkerMessage(MSG_WORK,null));
			var wpc = new WorkerProgressControl(id);
			// add the id to control mapping:
			_progressMap.Add(id, wpc);
			ProgressPanel.SuspendLayout();
			ProgressPanel.Controls.Add(wpc);
			wpc.Dock = DockStyle.Top;
			ProgressPanel.ResumeLayout(true);
		}
		protected override void OnClosed(EventArgs e)
		{
			if (null != _client)
				_client.Stop();
			base.OnClosed(e);
		}

		private void MaximumWorkersUpDown_ValueChanged(object sender, EventArgs e)
		{
			var val = (int)MaximumWorkersUpDown.Value;
			if(_oldMaximumWorkerCount>val && _client.WorkerCount>val)
			{
				// ako smo manje od stare vrednosti moramo da brisemo radnike
                try
				{
			        // desava se ponekad da pukne zbog range exception, verovatno zbog trke
                    _client.DeallocateWorkers(_client.WorkerCount - val);
				}
				catch(ArgumentOutOfRangeException) { // suppress this 
				}
			}
			_oldMaximumWorkerCount = val;
			_client.MaximumWorkerCount = (int)MaximumWorkersUpDown.Value;
		}
	}
}
