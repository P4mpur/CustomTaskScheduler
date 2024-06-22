using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TaskScheduling
{
	using ClientMsg = KeyValuePair<int, object>;
	
	class Worker
	{
		const int CLIMSG_PROGRESS = 0;
		const int CLIMSG_MESSAGE_COMPLETE = 3;
		const int CLIMSG_MESSAGE_RECEIVED = 4;
		const int MSG_STOP = 0;
		const int MSG_WORK = 1;
		static int _NextWorkerId = 1;
		Client _client = null;
		SemaphoreSlim _messagesAvailable;
		ConcurrentQueue<WorkerMessage> _messages;
		int _id;

		public Worker(Client client,SemaphoreSlim messagesAvailable,ConcurrentQueue<WorkerMessage> messages)
		{
			_id = _NextWorkerId;
			Interlocked.CompareExchange(ref _NextWorkerId, 0, int.MaxValue);
			Interlocked.Increment(ref _NextWorkerId);
			_client = client;
			_messagesAvailable = messagesAvailable;
			_messages = messages;
		}
		public int Id {
			get {
				return _id;
			}
		}
		public void PostMessage(WorkerMessage msg)
		{
			_messages.Enqueue(msg);
			_messagesAvailable.Release(1);
		}
		public void Start()
		{
			// Vrti while loop i gledaj da li postoje poruke
			var done = false;
			while(!done)
			{
				// Cekaj dok poruka ne postane dostupna
				_messagesAvailable.Wait();
				WorkerMessage smsg;
				// Jos jedna provera da li je tu
				if(!done && _messages.TryDequeue(out smsg))
				{
					// Poruka klijentu da je stigla poruka
					_client.PostMessage(new ClientMsg(CLIMSG_MESSAGE_RECEIVED, new WorkerMessage(smsg.Id, Id, smsg.CommandId, smsg.Argument)));
					
					switch(smsg.CommandId)
					{
				
						case MSG_WORK: // radi
							
							_client.PostMessage(new ClientMsg(CLIMSG_PROGRESS, new KeyValuePair<WorkerMessage, float>(smsg, 0f)));
							// Simulacija posla
							for (var i = 0;i<50;++i)
							{
								Thread.Sleep(100);
								// posalji progress
								_client.PostMessage(new ClientMsg(CLIMSG_PROGRESS,new KeyValuePair<WorkerMessage,float>(smsg,(i+1)/50f)));
							}
							
							break;
						case MSG_STOP: // shut down
							
							done = true;
							break;
					}
					// Poruka klijentu da smo prosli kroz poruku
					_client.PostMessage(new ClientMsg(CLIMSG_MESSAGE_COMPLETE,new WorkerMessage(smsg.Id,Id,smsg.CommandId,smsg.Argument)));
				}
			}
		}
	}
}
