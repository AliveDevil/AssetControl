using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libAssetControl.Network.Messages
{
	public struct AuthClientMessage
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
