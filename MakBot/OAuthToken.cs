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
        public string AuthToken { get; set; }
        public int TokenExpiration { get; set; }
    }

}
