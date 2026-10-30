using System.Collections.ObjectModel;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric.Channels;

namespace CS;

public interface IInputChoosers
{
	CSBlock GetBlockFromPrompt(string prompt);

	CSOutputChannel GetOutputFromPrompt(string prompt, ChannelArrayType arrayTypeFilter, ChannelDataType dataTypeFilter, bool showAll, bool autoSelect, bool autoHiddenBlocks);

	Collection<CSOutputChannel> GetOutputsFromPrompt(string prompt, ChannelArrayType arrayTypeFilter, ChannelDataType dataTypeFilter);

	CSObject GetStartFromPrompt(string prompt);
}
