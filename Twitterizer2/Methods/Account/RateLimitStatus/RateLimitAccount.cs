using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The Account of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitAccount : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the settings.
        /// </summary>
        /// <value>The settings rate-limit.</value>
        [DataMember, JsonProperty(PropertyName = "/account/settings")]
        public TwitterRateLimitResults Settings { get; set; }

        /// <summary>
        ///     Gets or sets the verify.
        /// </summary>
        /// <value>the verify rate-limit.</value>
        [DataMember, JsonProperty(PropertyName = "/account/verify_credentials")]
        public TwitterRateLimitResults Verify { get; set; }

        #endregion
    }
}