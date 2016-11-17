using System;
using System.Runtime.Serialization;

namespace RiveScript {
	
	[Serializable]
	public class ObjectMacro
	{
		public ObjectMacro ()
		{
		}

		public ObjectMacro(string lang, string[] code) {
			this.Code = code;
			this.Lang = lang;
		}

		public string[] Code;
		public string Lang;
	}
}

