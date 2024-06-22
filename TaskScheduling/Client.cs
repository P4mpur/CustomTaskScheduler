using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
namespace TaskScheduling
{
	using ClientMsg = KeyValuePair<int, object>;
	class Client : IDisposable
	{
		const int MSG_PROGRESS = 0;
		const int MSG_DISPATCH = 1;
		const int MSG_STOP = 2;
		const int MSG_MESSAGE_COMPLETE = 3;
		const int MSG_MESSAGE_RECEIVED = 4;
		const int WMSG_STOP = 0;
		
		// the message queue components
		SemaphoreSlim _messagesAvailable = new SemaphoreSlim(0);
		ConcurrentQueue<ClientMsg> _messages = new ConcurrentQueue<ClientMsg>();
		
		// in this case we want all workers to share the same queue
		// so we create it here in order that we can pass it down
		// to each worker we create
		SemaphoreSlim _workerMessagesAvailable = new SemaphoreSlim(0);
		ConcurrentQueue<WorkerMessage> _workerMessages = new ConcurrentQueue<WorkerMessage>();
		
		// max worker setting, defaults to CPU cores-1, minimum of 1
		int _maxWorkerCount = Math.Max(1,Environment.ProcessorCount-1);
		
		// statistics
		int _workerCount = 0;
		int _availableWorkerCount = 0;
		int _pendingWorkerMessageCount = 0;

		public int MaximumWorkerCount {
			get { return _maxWorkerCount; }
			set {
				if (1 > value)
					throw new ArgumentOutOfRangeException();
				// thread safe
				Interlocked.Exchange(ref _maxWorkerCount, value); 
			}
		}
	
		public void DeallocateWorkers(int workerCount=0) { 
			if (0 > workerCount || workerCount > _workerCount)
				throw new ArgumentOutOfRangeException("workerCount");
			if (0 == workerCount)
				workerCount = _workerCount;
			for (var i = 0; i < workerCount; ++i)
				_PostWorkerMessage(new WorkerMessage(WMSG_STOP, null));
		}
		
		public void PostMessage(ClientMsg msg)
		{
			_messages.Enqueue(msg);
			_messagesAvailable.Release(1);
		}
		void _PostWorkerMessage(WorkerMessage msg)
		{
			_workerMessages.Enqueue(msg);
			_workerMessagesAvailable.Release(1);
		}
		public int DispatchWorkerMessage(WorkerMessage msg)
		{
			PostMessage(new ClientMsg(MSG_DISPATCH, msg));
			return msg.Id;
		}
		public event WorkerMessageCompleteEventHandler WorkerMessageComplete;
		public event WorkerProgressEventHandler WorkerMessageProgress;
		public void Stop()
		{
			PostMessage(new ClientMsg(MSG_STOP, null));
		}
		public int WorkerCount {
			get {
				return _workerCount;
			}
		}
		public int AvailableWorkerCount {
			get {
				return _availableWorkerCount;
			}
		}
		public int WaitingWorkerMessageCount {
			get {
				return _workerMessages.Count;
			}
		}
		public int PendingWorkerMessageCount {
			get {
				return _pendingWorkerMessageCount;
			}
		}
		public void Start()
		{
			// Vrti
			var done = false;
			while (!done)
			{
				// Sacekaj na poruku
				_messagesAvailable.Wait();
				ClientMsg climsg;
				if (_messages.TryDequeue(out climsg))
				{
					switch (climsg.Key)
					{
                        // radnik je primio poruku i poceo je da je procesuira
						
						case MSG_MESSAGE_RECEIVED:
                            // Povecaj 'u toku' message count
							Interlocked.Increment(ref _pendingWorkerMessageCount);
                            // posto je radnik zaposljen smanji broj dostupnih radnika
							Interlocked.Decrement(ref _availableWorkerCount);
							break;
                            // radnik je zavrsio sa procesuiranjem poruke
						case MSG_MESSAGE_COMPLETE:
                            // smanji broj radnika koji rade
							Interlocked.Decrement(ref _pendingWorkerMessageCount);
							var wrkmsg = (WorkerMessage)climsg.Value;
							if (WMSG_STOP == wrkmsg.CommandId)
							{
                                // ako je radnik poslao poruku  da se stopira posao
                                // smanji broj radnika
								Interlocked.Decrement(ref _workerCount);
							}
							else // ako ne onda povecaj broj radnika
								Interlocked.Increment(ref _availableWorkerCount);
							// povecaj completed event
							WorkerMessageComplete?.Invoke(this, new WorkerMessageCompleteEventArgs(wrkmsg));
							break;
							// radnik ima raport 
						case MSG_PROGRESS:
							var arg = (KeyValuePair<WorkerMessage, float>)climsg.Value;
							WorkerMessageProgress?.Invoke(this, new WorkerProgressEventArgs(arg.Key, arg.Value));
							break;
						case MSG_DISPATCH: // posalji poruku radniku
                            // poruka je pokrenuta od DispatchWorkerMessage()
                            // ovde je handle-ujemo da bi nasa poruka bila thread safe
							
							// ako ne postoje dostupni radnici
							if (0 == _availableWorkerCount)
							{
                                // i ako nismo postigli max thread quota
								if (_workerCount < _maxWorkerCount)
								{
									// napravi novog workera
									Interlocked.Increment(ref _workerCount);
									Interlocked.Increment(ref _availableWorkerCount);
									var ts = new Worker(this, _workerMessagesAvailable, _workerMessages);
									new Thread(() => { ts.Start(); }).Start();
									
									_PostWorkerMessage((WorkerMessage)climsg.Value);
								}
								else
								{
                                    // moramo da stavimo u queue
                                    // vec zauzetog radnika
									_PostWorkerMessage((WorkerMessage)climsg.Value);
								}
							}
							else
							{
                                // postoji dostupan radnik
                                // samo ga pokreni
								_PostWorkerMessage((WorkerMessage)climsg.Value);
							}

							break;
                            // response da stane
						case MSG_STOP:
							// mora da bude isti broj stop poruka kao i sto ima radnika
                            for (var i = 0; i < _workerCount; ++i)
							{

                                // povecaj broj pending poruka
								Interlocked.Increment(ref _pendingWorkerMessageCount);
                                // stop radnici
								_PostWorkerMessage(new WorkerMessage(WMSG_STOP, null));
							}

							done = true;
							break;
					}

				}
			}
		}

		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			Stop();
		}
		~Client()
		{
			(this as IDisposable).Dispose();
		}

		public sealed class WorkerMessageCompleteEventArgs :EventArgs
		{
			public WorkerMessage Message { get; }
			internal WorkerMessageCompleteEventArgs(WorkerMessage message)
			{
				Message = message;
			}
		}
		public delegate void WorkerMessageCompleteEventHandler(object sender, WorkerMessageCompleteEventArgs args);
		public sealed class WorkerProgressEventArgs : EventArgs
		{
			public int Id { get { return Message.Id; } }
			public WorkerMessage Message { get; }
			public float Progress { get; }
			internal WorkerProgressEventArgs(WorkerMessage message,float progress)
			{
				Message = message;
				Progress = progress;
			}
		}
		public delegate void WorkerProgressEventHandler(object sender, WorkerProgressEventArgs args);


	}
}
