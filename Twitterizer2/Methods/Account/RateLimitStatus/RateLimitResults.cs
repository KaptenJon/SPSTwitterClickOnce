using System;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector
{
    /// <summary>
    ///     The Results of rate-limit.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TwitterRateLimitResults : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the remaining hits.
        /// </summary>
        /// <value>The remaining hits.</value>
        [JsonProperty(PropertyName = "remaining")]
        public int RemainingHits { get; set; }

        /// <summary>
        ///     Gets or sets the hourly limit.
        /// </summary>
        /// <value>The hourly limit.</value>
        [JsonProperty(PropertyName = "limit")]
        public int HourlyLimit { get; set; }

        /// <summary>
        ///     Gets or sets the UTC string value of the time rate limiting will reset.
        /// </summary>
        /// <value>The reset time string.</value>
        [JsonProperty(PropertyName = "reset")]
        [JsonConverter(typeof (TwitterConnectorDateConverter))]
        public DateTime ResetTime { get; set; }

        #endregion
    }
}