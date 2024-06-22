using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskScheduling
{
	struct WorkerMessage
	{
		static int _NextMessageId = 1;
		public int Id { get; }
		public int WorkerId { get; }
		public int CommandId { get; }
		public object Argument { get; }
		public WorkerMessage(int commandId,object argument)
		{
			WorkerId = 0;
			CommandId = commandId;
			Argument = argument;
			Id = _NextMessageId;
			Interlocked.CompareExchange(ref _NextMessageId, 0, int.MaxValue);
			Interlocked.Increment(ref _NextMessageId);
		}
		internal WorkerMessage(int id, int workerId, int commandId, object argument)
		{
			Id = id;
			WorkerId = workerId;
			CommandId = commandId;
			Argument = argument;
			if (0 == id)
			{
				Id = _NextMessageId;
				Interlocked.CompareExchange(ref _NextMessageId, 0, int.MaxValue);
				Interlocked.Increment(ref _NextMessageId);
			}
		}

	}
}
