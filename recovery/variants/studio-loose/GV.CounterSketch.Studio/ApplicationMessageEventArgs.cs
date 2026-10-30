using System;
using GV.CounterSketch.Studio.Model.Objects.Enums;

namespace GV.CounterSketch.Studio;

public class ApplicationMessageEventArgs : EventArgs
{
	public bool ModalMessage { get; set; }

	public string AlertMessageText { get; set; }

	public ApplicationMessageType AlertMessageType { get; set; }

	public ApplicationMessageEventArgs(ApplicationMessageType messageType, string messageText)
	{
		AlertMessageType = messageType;
		AlertMessageText = messageText;
	}

	public ApplicationMessageEventArgs(ApplicationMessageType messageType, string messageText, bool messageIsModal)
	{
		ModalMessage = messageIsModal;
		AlertMessageType = messageType;
		AlertMessageText = messageText;
	}
}
