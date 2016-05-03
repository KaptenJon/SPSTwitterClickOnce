using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The geo of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitGeo : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the placeid.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/geo/id/:place_id")]
        public TwitterRateLimitResults PlaceId { get; set; }

        /// <summary>
        ///     Gets or sets the reverse_geocode.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/geo/reverse_geocode")]
        public TwitterRateLimitResults ReverseGeoCode { get; set; }

        /// <summary>
        ///     Gets or sets the search.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/geo/search")]
        public TwitterRateLimitResults Search { get; set; }

        /// <summary>
        ///     Gets or sets the SimilarPlaces.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/geo/similar_places")]
        public TwitterRateLimitResults SimilarPlaces { get; set; }

        #endregion
    }
}