using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The Searches of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitSearch : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the search tweets.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/search/tweets")]
        public TwitterRateLimitResults Tweets { get; set; }

        #endregion
    }
}