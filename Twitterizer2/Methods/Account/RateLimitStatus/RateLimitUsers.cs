using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The users of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitUsers : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the contributees.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/contributees")]
        public TwitterRateLimitResults Contributees { get; set; }

        /// <summary>
        ///     Gets or sets the contributors.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/contributors")]
        public TwitterRateLimitResults Contributors { get; set; }

        /// <summary>
        ///     Gets or sets the lookup.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/lookup")]
        public TwitterRateLimitResults Lookup { get; set; }

        /// <summary>
        ///     Gets or sets the profile_banner.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/profile_banner")]
        public TwitterRateLimitResults ProfileBanner { get; set; }

        /// <summary>
        ///     Gets or sets the search.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/search")]
        public TwitterRateLimitResults Search { get; set; }

        /// <summary>
        ///     Gets or sets the show.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/show/:id")]
        public TwitterRateLimitResults Show { get; set; }

        /// <summary>
        ///     Gets or sets the suggestions.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/suggestions")]
        public TwitterRateLimitResults Suggestions { get; set; }

        /// <summary>
        ///     Gets or sets the suggestions slug.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/suggestions/:slug")]
        public TwitterRateLimitResults SuggestionsSlug { get; set; }

        /// <summary>
        ///     Gets or sets the suggestions slug members.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/users/suggestions/:slug/members")]
        public TwitterRateLimitResults SuggestionsSlugMembers { get; set; }

        #endregion
    }
}