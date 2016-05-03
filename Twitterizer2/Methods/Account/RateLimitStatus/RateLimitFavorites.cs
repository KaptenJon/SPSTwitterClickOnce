using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The Favorites of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitFavorites : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the list.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/favorites/list")]
        public TwitterRateLimitResults List { get; set; }

        #endregion
    }
}