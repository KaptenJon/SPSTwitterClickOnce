using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The statuses of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitStatuses : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the hometimeline.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/home_timeline")]
        public TwitterRateLimitResults HomeTimeline { get; set; }

        /// <summary>
        ///     Gets or sets the mentionstimeline.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/mentions_timeline")]
        public TwitterRateLimitResults Mentions { get; set; }

        /// <summary>
        ///     Gets or sets the oembed.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/oembed")]
        public TwitterRateLimitResults Oembed { get; set; }

        /// <summary>
        ///     Gets or sets the retweeters.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/retweeters/ids")]
        public TwitterRateLimitResults RetweetersIds { get; set; }

        /// <summary>
        ///     Gets or sets the retweets.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/retweets/:id")]
        public TwitterRateLimitResults Retweetes { get; set; }

        /// <summary>
        ///     Gets or sets the retweets_of_me.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/retweets_of_me")]
        public TwitterRateLimitResults RetweetsOfMe { get; set; }

        /// <summary>
        ///     Gets or sets the show.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/show/:id")]
        public TwitterRateLimitResults Show { get; set; }

        /// <summary>
        ///     Gets or sets the usertimeline.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/statuses/user_timeline")]
        public TwitterRateLimitResults UserTimeline { get; set; }

        #endregion
    }
}