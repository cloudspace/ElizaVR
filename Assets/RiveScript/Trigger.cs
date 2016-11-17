using System.Collections.Generic;
using System;

namespace RiveScript
{
    /// <summary>
    /// Trigger class for RiveScript.
    /// </summary>
	[Serializable]
    public class Trigger
    {
        public string pattern = "";
        public string inTopic = "";
        public ICollection<string> redirect = new List<string>();
        public ICollection<string> reply = new List<string>();
        public ICollection<string> condition = new List<string>();
        public bool previous = false;

        public Trigger(string topic, string pattern)
        {
            inTopic = topic;
            this.pattern = pattern;
        }

		public Trigger() 
		{
		}

        public string topic()
        {
            return inTopic;
        }

        public bool hasPrevious()
        {
            return previous;
        }

        public void hasPrevious(bool paired)
        {
            //original implementation
            previous = true;
            //previous = paired; - My inserstant
        }

        public void addReply(string reply) {
            this.reply.Add(reply);
        }

        public string[] listReplies()
        {
            return reply.ToArray();
        }

        public void addRedirect(string meant)
        {
            redirect.Add(meant);
        }

        public string[] listRedirects()
        {
            return redirect.ToArray();
        }

        public void addCondition(string condition)
        {
            this.condition.Add(condition);
        }

        public string[] listConditions()
        {
            return condition.ToArray();
        }
    }
}
