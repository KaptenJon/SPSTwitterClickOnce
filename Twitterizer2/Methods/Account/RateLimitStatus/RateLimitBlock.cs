using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The Blocks of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitBlock : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the settings.
        /// </summary>
        /// <value>The settings rate-limit.</value>
        [DataMember, JsonProperty(PropertyName = "/blocks/ids")]
        public TwitterRateLimitResults Ids { get; set; }

        /// <summary>
        ///     Gets or sets the verify.
        /// </summary>
        /// <value>the verify rate-limit.</value>
        [DataMember, JsonProperty(PropertyName = "/blocks/list")]
        public TwitterRateLimitResults List { get; set; }

        #endregion
    }
}