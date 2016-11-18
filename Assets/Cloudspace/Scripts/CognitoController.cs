using UnityEngine;
using System.Collections;
using Amazon.CognitoIdentity;
using System;
using Amazon;
using Amazon.S3;

public class CognitoController : MonoBehaviour {
	public AmazonS3Client s3Client;

	// Use this for initialization
	void Start () {
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
	}
	
	public void OnSaveDialog() {
	}		
}
