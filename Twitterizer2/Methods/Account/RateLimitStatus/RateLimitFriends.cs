using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The Friends of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitFriends : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the ids.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/friends/ids")]
        public TwitterRateLimitResults Ids { get; set; }

        /// <summary>
        ///     Gets or sets the list.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/friends/list")]
        public TwitterRateLimitResults List { get; set; }

        #endregion
    }
}