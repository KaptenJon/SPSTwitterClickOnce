using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus
{
    /// <summary>
    ///     The Application of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitApplication : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the settings.
        /// </summary>
        /// <value>The settings rate-limit.</value>
        [DataMember, JsonProperty(PropertyName = "/application/rate_limit_status")]
        public TwitterRateLimitResults RateLimitStatus { get; set; }

        #endregion
    }
}