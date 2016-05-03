using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The SavedSearches of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitSavedSearches : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the destroy.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/saved_searches/destroy/:id")]
        public TwitterRateLimitResults Destroy { get; set; }

        /// <summary>
        ///     Gets or sets the list.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/saved_searches/list")]
        public TwitterRateLimitResults List { get; set; }

        /// <summary>
        ///     Gets or sets the show.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/saved_searches/show/:id")]
        public TwitterRateLimitResults Show { get; set; }

        #endregion
    }
}