using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CS.CSObjects.CSBlocks;
using CS.CSValues.Interfaces;
using CS.Parametric;
using CS.Parametric.Channels;
using Rhino;
using Rhino.Runtime;

namespace CS.Utilities.Helpers;

public static class ParallelGraphProcessingHelper
{
	private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private static ConcurrentDictionary<CSBlock, ConcurrentBag<CSBlock>> _parentChildLinks = new ConcurrentDictionary<CSBlock, ConcurrentBag<CSBlock>>();

	private static ConcurrentQueue<CSBlock> _workingQueue = new ConcurrentQueue<CSBlock>();

	public static void GenerateChildRelationships(IEnumerable<CSBlock> graphObjects)
	{
		_parentChildLinks = new ConcurrentDictionary<CSBlock, ConcurrentBag<CSBlock>>();
		graphObjects.AsParallel().WithDegreeOfParallelism(CSApplicationHelper.MaxNumberOfAvailableCores).ForAll(delegate(CSBlock csBlock)
		{
			GetInputChannelObjectsForUpdate(csBlock).ToList().ForEach(delegate(CSInputChannel inputChannel)
			{
				if (_parentChildLinks.ContainsKey(inputChannel.OutputChannelBlock))
				{
					lock (_parentChildLinks)
					{
						if (!_parentChildLinks[inputChannel.OutputChannelBlock].Contains(csBlock))
						{
							_parentChildLinks[inputChannel.OutputChannelBlock].Add(csBlock);
						}
						return;
					}
				}
				lock (_parentChildLinks)
				{
					_parentChildLinks.TryAdd(inputChannel.OutputChannelBlock, new ConcurrentBag<CSBlock> { csBlock });
				}
			});
		});
	}

	public static void ProcessGraph(List<CSBlock> blocksToProcess)
	{
		HostUtils.DisplayOleAlerts(false);
		lock (_workingQueue)
		{
			_workingQueue = new ConcurrentQueue<CSBlock>(blocksToProcess.Where((CSBlock block) => block.ProcessingStatus != BlockProcessingStatus.Processed));
		}
		if (_workingQueue.Any())
		{
			_cancellationTokenSource = new CancellationTokenSource();
			CancellationToken token = _cancellationTokenSource.Token;
			int num = 3;
			ParallelOptions parallelOptions = new ParallelOptions
			{
				CancellationToken = token,
				MaxDegreeOfParallelism = num
			};
			try
			{
				List<Task> list = new List<Task>();
				for (int num2 = 0; num2 < num; num2++)
				{
					list.Add(Task.Factory.StartNew(WalkingAction(parallelOptions), token));
				}
				int num3 = 0;
				do
				{
					Task.WaitAll(list.ToArray(), 5000, token);
				}
				while (num3++ < 5 && !token.IsCancellationRequested && !CSApplicationHelper.Application.Document.InterruptUpdate());
				if (num3 >= 5 && list.Any((Task task) => task.Status != TaskStatus.RanToCompletion))
				{
					RhinoApp.WriteLine("Graph processing terminated, timeout [25sec] exceeded.  Indeterminate state.");
					ResetWorkingQueue();
				}
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				HostUtils.DisplayOleAlerts(true);
			}
		}
		HostUtils.DisplayOleAlerts(true);
	}

	public static void ResetWorkingQueue()
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
		}
		lock (_workingQueue)
		{
			_workingQueue = new ConcurrentQueue<CSBlock>();
		}
	}

	private static IEnumerable<CSBlock> CheckLoopBlock(CSBlock csBlock)
	{
		ICSLoopUpdate loopedBlock = csBlock as ICSLoopUpdate;
		IEnumerable<CSBlock> result = null;
		if (loopedBlock != null)
		{
			result = from _ in csBlock.InputChannels
				where _ != null && _.OutputChannelBlock != null && _ != loopedBlock.InDisplayUpdate
				select _.OutputChannelBlock;
		}
		return result;
	}

	private static void EvaluateBlockForProcessing(CSBlock csBlock)
	{
		IEnumerable<CSBlock> source = CheckLoopBlock(csBlock) ?? (from inputChannel in GetInputChannelObjectsForUpdate(csBlock)
			select inputChannel.OutputChannelBlock);
		source = source.Where((CSBlock _) => _.ProcessingStatus != BlockProcessingStatus.Processed && !_.HasIllegalLoop);
		IList<CSBlock> source2 = (source as IList<CSBlock>) ?? source.ToList();
		if (source2.Any())
		{
			csBlock.ProcessingStatus = BlockProcessingStatus.Waiting;
			lock (_workingQueue)
			{
				source2.Where((CSBlock linkedBlock) => linkedBlock.ProcessingStatus == BlockProcessingStatus.NeedsProcessing && !_workingQueue.Contains(linkedBlock)).ToList().ForEach(_workingQueue.Enqueue);
				_workingQueue.Enqueue(csBlock);
				return;
			}
		}
		ProcessAndEnqueueChildLinks(csBlock);
	}

	private static IEnumerable<CSInputChannel> GetInputChannelObjectsForUpdate(CSBlock csBlock)
	{
		List<CSInputChannel> inputChannels = new List<CSInputChannel>();
		csBlock.InputChannels.ForEach(delegate(CSInputChannel inputChannel)
		{
			if (inputChannel != null && inputChannel.OutputChannelBlock != null)
			{
				inputChannels.Add(inputChannel);
			}
		});
		return inputChannels;
	}

	private static void ProcessAndEnqueueChildLinks(CSBlock csBlock)
	{
		csBlock.Process();
		if (_parentChildLinks.ContainsKey(csBlock))
		{
			lock (_workingQueue)
			{
				_parentChildLinks[csBlock].Where((CSBlock childBlock) => childBlock.ProcessingStatus == BlockProcessingStatus.NeedsProcessing && !_workingQueue.Contains(childBlock)).ToList().ForEach(delegate(CSBlock child)
				{
					_workingQueue.Enqueue(child);
				});
			}
		}
		csBlock.ProcessingStatus = BlockProcessingStatus.Processed;
	}

	private static void RecordLoopData()
	{
		lock (_workingQueue)
		{
			_workingQueue.ToList().ForEach(delegate(CSBlock csBlock)
			{
				csBlock.LoopCheckStatus = BlockLoopCheckStatus.HasIllegalLoops;
			});
		}
	}

	private static Action WalkingAction(ParallelOptions parallelOptions)
	{
		return delegate
		{
			try
			{
				while (_workingQueue.Count != 0 && !CSApplicationHelper.Application.Document.InterruptUpdate())
				{
					parallelOptions.CancellationToken.ThrowIfCancellationRequested();
					CSBlock result = null;
					lock (_workingQueue)
					{
						_workingQueue.TryDequeue(out result);
					}
					if (result != null && result.ProcessingStatus != BlockProcessingStatus.Processed)
					{
						if (result.ProcessingStatus == BlockProcessingStatus.NeedsProcessing)
						{
							result.ProcessingStatus = BlockProcessingStatus.InProcess;
						}
						EvaluateBlockForProcessing(result);
					}
				}
			}
			catch (Exception)
			{
			}
		};
	}
}
