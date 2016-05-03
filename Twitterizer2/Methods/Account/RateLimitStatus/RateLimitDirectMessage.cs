using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The DMs of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitDirectMessage : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the directmessages.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/direct_messages")]
        public TwitterRateLimitResults DirectMessages { get; set; }

        /// <summary>
        ///     Gets or sets the sent.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/direct_messages/sent")]
        public TwitterRateLimitResults Sent { get; set; }

        /// <summary>
        ///     Gets or sets the SentAndReceived.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/direct_messages/sent_and_received")]
        public TwitterRateLimitResults SentAndReceived { get; set; }

        /// <summary>
        ///     Gets or sets the show.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/direct_messages/show")]
        public TwitterRateLimitResults Show { get; set; }

        #endregion
    }
}