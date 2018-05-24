using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MakBot
{
    public class ClipAccess
    {
        private readonly string clipArchive = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}", "clips.json");
        private string clipJSON;
        private string userRequest;

        public ClipAccess(string message)
        {
            userRequest = GetShortHandFromRequest(message);
            clipJSON = LoadJSON();
        }

        /// <summary>
        /// Fetches the user specified clip from the json
        /// </summary>
        /// <returns>The user specified clip as a Clip object</returns>
        public Clip GetClip()
        {
            var json = JsonConvert.DeserializeObject<ClipData>(clipJSON);
            var selectedClip = json.Clips.Where(x => x.Shorthand.Equals(userRequest)).FirstOrDefault();

            return selectedClip;
        }

        /// <summary>
        /// Retrieves the requested clip shorthand from the command
        /// </summary>
        /// <param name="request">The !clip message sent to the channel as a string</param>
        /// <returns>The clip shorthand as a string</returns>
        private string GetShortHandFromRequest(string request)
        {
            var reqParts = request.Split(' ');
            var clipShorthand = reqParts[1];
            return clipShorthand;
        }

        /// <summary>
        /// Reads the json file containing the clips
        /// </summary>
        /// <returns>The contents of the json file as a string</returns>
        private string LoadJSON()
        {
            string json = string.Empty;

            using (StreamReader reader = new StreamReader(clipArchive))
            {
                json = reader.ReadToEnd();
            }

            return json;
        }
    }
}
