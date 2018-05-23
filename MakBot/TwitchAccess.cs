using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// http://matthiasshapiro.com/2015/10/26/simple-json-api-calls-in-c-in-10-minutes/
// https://blog.jayway.com/2012/03/13/httpclient-makes-get-and-post-very-simple/
// https://discuss.dev.twitch.tv/t/getting-oauth-token-for-newly-registered-twitch-application-chat-bot/9745

namespace MakBot
{
    public class TwitchAccess
    {
        private readonly string TwitchApiUrl = @"https://api.twitch.tv/helix/";
        private readonly string TwitchOAuthUrl = @"https://id.twitch.tv/oauth2/";
        private static HttpClient twitchClient = new HttpClient();
        private string apiKey;
        public string ClientID { get; set; }       

        public TwitchAccess() { }

        public TwitchAccess(string id)
        {
            ClientID = id;
        }
        
        /// <summary>
        /// Get the stream information JSON from the Twitch API
        /// </summary>
        /// <returns>TwitchStream object with the stream's information in a StreamData array and a Pagination object</returns>
        public async Task<TwitchStream> GetStreamInfoAsync()
        {
            TwitchStream streamInfo = null;
            string url = $"{TwitchApiUrl}streams?user_login=dellor"; // Should I not hardcode Mak's username?
            apiKey = await GetAuthTokenAsync();

            twitchClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await twitchClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var streamJSON = await response.Content.ReadAsStringAsync();
                streamInfo = JsonConvert.DeserializeObject<TwitchStream>(streamJSON);

                RevokeAuthTokenAsync(apiKey);
                return streamInfo;
            }

            RevokeAuthTokenAsync(apiKey);
            return streamInfo;
        }

        public bool IsStreamUp(TwitchStream streamInfo)
        {
            if (streamInfo.Data.Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Fetches the OAuthToken necessary for API access
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAuthTokenAsync()
        {
            string token = string.Empty;
            string url = $"{TwitchOAuthUrl}token?client_id={ClientID}&client_secret=&grant_type=client_credentials"; // Hide client secret

            var response = await twitchClient.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var tokenAPI = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<OAuthToken>(tokenAPI);

                return token = json.AuthToken;
            }

            return token;
        }

        /// <summary>
        /// Revokes the OAuthToken for API access in order to clean up
        /// </summary>
        /// <param name="token">The OAuth token as a string</param>
        private async void RevokeAuthTokenAsync(string token)
        {
            string url = $"{TwitchOAuthUrl}revoke?client_id={ClientID}&token={token}";
            var response = await twitchClient.PostAsync(url, null); // https://stackoverflow.com/a/7908243

            if (response.IsSuccessStatusCode)
            {
                return;
            }
        }
    }
}
