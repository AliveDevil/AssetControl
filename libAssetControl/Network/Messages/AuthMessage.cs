using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libAssetControl.Network.Messages
{
	public struct AuthMessage
	{
		public static readonly AuthMessage Authed = new AuthMessage() { IsAuthed = true };
		public static readonly AuthMessage Error = new AuthMessage() { IsAuthed = false };

		public bool IsAuthed { get; set; }
	}
}
