using System;
using System.Collections.Generic;
using System.Text;

namespace MakBot
{
    /// <summary>
    /// Represents the JSON for a returning OAuth Token
    /// </summary>
    public class OAuthToken
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
    }

}
