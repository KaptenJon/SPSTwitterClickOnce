using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The trends of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitTrends : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the available.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/trends/available")]
        public TwitterRateLimitResults Available { get; set; }

        /// <summary>
        ///     Gets or sets the closest.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/trends/closest")]
        public TwitterRateLimitResults Closest { get; set; }

        /// <summary>
        ///     Gets or sets the place.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/trends/place")]
        public TwitterRateLimitResults Place { get; set; }

        #endregion
    }
}