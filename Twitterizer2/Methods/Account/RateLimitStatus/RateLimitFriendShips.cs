using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The FriendShips of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitFriendShips : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the incoming.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/friendships/incoming")]
        public TwitterRateLimitResults Incoming { get; set; }

        /// <summary>
        ///     Gets or sets the lookup.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/friendships/lookup")]
        public TwitterRateLimitResults Lookup { get; set; }

        /// <summary>
        ///     Gets or sets the nortids.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/friendships/no_retweets/ids")]
        public TwitterRateLimitResults NoRetweetsIds { get; set; }

        /// <summary>
        ///     Gets or sets the outgoing.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/friendships/outgoing")]
        public TwitterRateLimitResults Outgoing { get; set; }

        /// <summary>
        ///     Gets or sets the show
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/friendships/show")]
        public TwitterRateLimitResults Show { get; set; }

        #endregion
    }
}