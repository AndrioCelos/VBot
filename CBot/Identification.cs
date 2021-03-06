using System.Collections.Generic;

using AnIRC;

namespace CBot {
	/// <summary>
	/// Records a user's identification to an account.
	/// </summary>
	public class Identification {
		/// <summary>The connection to the IRC network the user is on.</summary>
		public IrcClient Connection;
		/// <summary>The user's nickname.</summary>
		public string Nickname;
		/// <summary>The account to which the user has identified.</summary>
		public string AccountName;
		/// <summary>Indicates whether CBot is watching this user using the MONITOR or WATCH command.</summary>
		public bool Monitoring;
		/// <summary>The list of channels this user shares with the bot.</summary>
		public HashSet<string> Channels;
	}
}
