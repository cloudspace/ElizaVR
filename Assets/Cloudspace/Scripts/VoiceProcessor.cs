using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text.RegularExpressions;
using RiveScript;
using MsgPack.Serialization;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Services.Dialog.v1;

using System.IO;

namespace Cloudspace
{

	/// <summary>
	/// Demonstrates use of the Salsa3D public methods
	/// </summary>
	public class VoiceProcessor : MonoBehaviour
	{

		public string url;
		public AudioSource source;

		private RiveScript.RiveScript chatbot;
		TextToSpeech m_TextToSpeech = new TextToSpeech ();

		public RiveScript.RiveScript GetChatbot () {
			return chatbot;
		}

		private CharacterHandler controller;
		public void Start() {
			controller = this.GetComponentInParent<CharacterHandler> ();
		}

		int startFrame = 0;
		public void Update() {
			
			if (startFrame == 5) {
				StartStuff ();
				foreach (SkinnedMeshRenderer renderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ()) {
					renderer.enabled = true;
				}
			}
			if (startFrame < 6) {
				startFrame++;
			}
		}

		public void StartStuff() {
 			m_TextToSpeech.Voice = VoiceType.en_US_Allison;

			string botBinPath = (Application.persistentDataPath+ "/bot.msgpack");

			SerializationContext ctx = SerializationContext.Default;
			//			ctx.Serializers.RegisterOverride (new RiveScript.DictionarySerializerWorkaround (ctx));
			var serializer = ctx.GetSerializer<RiveScript.RiveScript>();

			if (System.IO.File.Exists (botBinPath)) {
				try {
					FileStream BotStream = new FileStream (botBinPath, FileMode.Open);
					chatbot = serializer.Unpack (BotStream);
					BotStream.Close ();	
					chatbot.setHandler ("csharp", new RiveScript.lang.CSharp ());
				} catch (System.Exception buildException) {
					System.Exception bex = buildException;
					Debug.Log (bex.Message);
					return;
				}
			} else {
				Debug.Log ("File does not exist, will create new chatbot");
			}

			if (chatbot == null) {
				chatbot = new RiveScript.RiveScript (false);

				AssetReader reader = new AssetReader ("eliza.rive");

				if (reader.text.Length == 0 || !chatbot.stream (reader.text, false)) {
					Debug.Log ("Error loading chat files: " + chatbot.error ());
				} else {
					Debug.Log ("No error loading chat files");
				}


				chatbot.sortReplies ();
				// Won't match anything, so it'll be forced to build regexes for common topics.
				chatbot.reply ("user", "cstriggerbuildqazwsx");

				FileStream BotStream = new FileStream (botBinPath, FileMode.Create);
				try { 
					serializer.Pack (BotStream, chatbot);
					BotStream.Close ();		
				} catch (System.Exception ex) {
					Debug.Log ("Couldn't pack bot: " + ex.Message);
				}
				Debug.Log ("Packed bot to: " + botBinPath);
			}
			StartCoroutine (GetDialogCo ("hello"));
		}


		IEnumerator GetDialogCo (string command) {
			GetDialog (command);
			yield return null;
		}


		// This is getting speech from a person.
		public void OnNewSpeech(Notification notification)
		{
			GetDialog ((string)notification.data);
		}

		// This is getting audio from the system.
		public void OnAudio(Notification notification) {
			AudioClip clip = (AudioClip)notification.data;
			StartCoroutine(PlayAudioClip(clip));
		}

		public void GetDialog (string response) {
			NotificationCenter.DefaultCenter ().PostNotification (this, "OnStopUserSpeech");
			System.DateTime starttime = System.DateTime.Now;

			Debug.Log (System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff") + " Bot Hears: " + response);
			string reply = chatbot.reply ("user", response);
			Debug.Log (System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff") + " Bot Answers: " + reply);

			//			Debug.Log("Got chatbot reply in "+ System.DateTime.Now.Subtract(starttime).TotalSeconds + " seconds");
			NotificationCenter.DefaultCenter ().PostNotification (this, "OnSaveDialog", new KeyValuePair<string, string>(response, reply));
			TextToSpeech(reply);		
		}

		public void OnTextFromSpeech(Notification notice) {
			GetDialog ((string)notice.data);
		}

		public void TextToSpeech(string text) {
			m_TextToSpeech.ToSpeech (text, t => StartCoroutine(PlayAudioClip(t)));
		}

		public IEnumerator PlayAudioClip(AudioClip clip) {
			NotificationCenter.DefaultCenter ().PostNotification (this, "OnStopListening");

			controller.RunBotTalkingActions ();
			controller.RunActions ();

			if (clip != null) {
				source.spatialBlend = 0.0f;
				source.loop = false;
				source.clip = clip;
				source.Play();
			}
			yield return new WaitUntil (() => source.clip.length - source.time < .25);
			yield return new WaitForSeconds (.75f);
			source.clip = null;
			NotificationCenter.DefaultCenter ().PostNotification (this, "OnStartListening");
		}

		public void Awake () {
			NotificationCenter.DefaultCenter ().AddObserver(this, "OnTextFromSpeech");
		}

		/// <summary>
		/// Draw the GUI buttons
		/// </summary>
		void OnGUI ()
		{
		}
	}
}

