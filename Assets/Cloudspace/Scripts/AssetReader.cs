using System;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Logging;
using System.IO;

namespace Cloudspace {
	
	public class AssetReader {
		public String path;

		public AssetReader(String path) {
			this.path = path;
		}

		public String text {
			get {
				WWW reader;
				string oriPath = System.IO.Path.Combine (Application.streamingAssetsPath, this.path);
				if (!oriPath.Contains ("://")) {
					oriPath = "file://" + oriPath;
				}
				reader = new WWW (oriPath);
				while (!reader.isDone) {
				}
//				Debug.Log ("reader text: " + reader.text);
				return reader.text;
			}
		}
	}
}

