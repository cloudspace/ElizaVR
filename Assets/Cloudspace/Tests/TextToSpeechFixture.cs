using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cloudspace
{
	[IntegrationTest.DynamicTestAttribute("SpeechToText")]
	[IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.TimeoutAttribute(10)]
    public class TextToSpeechFixture : MonoBehaviour
    {
        NotificationCenter center = null;
        // Use this for initialization
        void Start()
        {
            center = NotificationCenter.DefaultCenter();
            center.AddObserver(this, "OnStopListening");
            center.AddObserver(this, "OnStartListening");
            center.AddObserver(this, "OnTextFromSpeech");
            center.AddObserver(this, "OnSaveDialog");

            StartCoroutine(SaySomething());
        }

        public int StopListeningCalled  { get { return MessageCount("OnStopListening"); } }
        public int StartListeningCalled  { get { return MessageCount("OnStartListening"); } }
        public int TextFromSpeechCalled { get { return MessageCount("OnTextFromSpeech"); } }
        public int SaveDialogCalled { get { return MessageCount("OnSaveDialog"); } }

        public IEnumerator SaySomething()
        {
            yield return new WaitForSeconds(3);
            center.PostNotification(this, "OnTextFromSpeech", "Hello");
            yield return new WaitForSeconds(4);

            if(StopListeningCalled == 2 &&
               StartListeningCalled == 2 &&
               TextFromSpeechCalled == 1 &&
               SaveDialogCalled == 2
            )
                IntegrationTest.Pass(gameObject);
            else
                IntegrationTest.Fail(gameObject);

        }


        public List<KeyValuePair<string, string>> Messages = new List<KeyValuePair<string, string>>();

        public int MessageCount(string method) 
        {
            return Messages.Where(t => t.Key == method).Count();
        }

        public void AddMessage(Notification notification, string method)
        {
            var data = notification.data == null ? null : notification.data.ToString();
            Messages.Add(new KeyValuePair<string, string>(method, data));
        } 

		public void OnTextFromSpeech(Notification notice) {
            AddMessage(notice, "OnTextFromSpeech");
        }

		public void OnStartListening(Notification notice) {
            AddMessage(notice, "OnStartListening");
        }

		public void OnStopListening(Notification notice) {
            AddMessage(notice, "OnStopListening");
        }

		public void OnSaveDialog(Notification notice) {
            AddMessage(notice, "OnSaveDialog");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}