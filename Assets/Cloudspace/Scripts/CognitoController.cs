using UnityEngine;
using System.Collections;
using Amazon.CognitoIdentity;
using System;
using Amazon;
using Amazon.S3;
using Cloudspace;
using System.IO;
using Amazon.S3.Model;
using System.Collections.Generic;

public class CognitoController : MonoBehaviour {
	public AmazonS3Client s3Client;

	void Awake () {
		string guid = PlayerPrefs.GetString ("guid");
		if (guid.Length == 0) {
			PlayerPrefs.SetString ("guid", System.Guid.NewGuid().ToString());
			PlayerPrefs.Save ();
			guid = PlayerPrefs.GetString ("guid");
		}

		string IdentityPoolId = "us-east-1:4a8a0436-6cdc-4aeb-9647-5b09d557400f";
		UnityInitializer.AttachToGameObject(this.gameObject);
		try {
			CognitoAWSCredentials credentials = new CognitoAWSCredentials(IdentityPoolId, RegionEndpoint.USEast1);
			s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
		} catch(Exception ex) {
			Debug.Log ("Amazon died trying to connect client: " + ex.Message);
		}

		NotificationCenter.DefaultCenter().AddObserver(this, "OnSaveDialog"); 
//		NotificationCenter.DefaultCenter().PostNotification(this, "OnTriggerNewExperience", dialog);
	}
	
	public void OnSaveDialog(Notification notification) {
		KeyValuePair<string, string> results = (KeyValuePair<string, string>)notification.data;
		String bot_transcript = results.Value; //notification.data;
		String user_transcript = results.Key; //notification.data;

		DateTime today = System.DateTime.Now;
		string key = today.ToString ("yyyy-MM-dd") + "/" + PlayerPrefs.GetString("guid") + ".txt";

		string dialogue = "{\"actor\": \"human\", \"msg\": \"" + user_transcript + "\", \"timestamp\": \"" + today.ToString() + "\"}\n" +
			"{\"actor\": \"bot\", \"msg\": \"" + bot_transcript + "\", \"timestamp\": \"" + today.ToString() + "\"}\n";
		byte[] dialogueData = System.Text.Encoding.UTF8.GetBytes(dialogue);

		Stream stream = new MemoryStream();
		int position = 0;
		try {
			s3Client.GetObjectAsync("unitychat", key, (responseObj) => {
				if (responseObj.Response != null) {
					var response = responseObj.Response;
					if (response.ResponseStream != null) {
						Stream inputStream = response.ResponseStream;
						byte[] buffer = new byte[16*1024];
						int read;
						while((read = inputStream.Read (buffer, 0, buffer.Length)) > 0) {
							stream.Write (buffer, 0, read);
						}
						position = (int) inputStream.Length;
					}
				}
				TextWriter writer = new StreamWriter(stream);
				writer.Write(dialogue);
				writer.Flush();
				stream.Position = 0L;

				var request = new PostObjectRequest() {
					Region = RegionEndpoint.USEast1,
					Bucket = "unitychat",
					Key = key,
					InputStream = stream,
					CannedACL = S3CannedACL.PublicReadWrite
				};

				try {
					s3Client.PostObjectAsync(request, (postResponseObj) => {
						if (postResponseObj.Exception == null) {
							Debug.Log (string.Format("object {0} posted to bucket", postResponseObj.Request.Key));
						} else {
							Debug.Log (string.Format("receieved error {0}", postResponseObj.Exception.StackTrace));
						}
					});
				} catch (Exception e) {
					Debug.Log ("exception: " + e.ToString());
				}
			});
		} catch(Exception ex) {
			Debug.Log ("Amazon died trying to run:" + ex.Message);
		}
	}

}
