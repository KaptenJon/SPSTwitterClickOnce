using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector.RateLimitStatus

{
    /// <summary>
    ///     The lists of rate limit status.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RateLimitLists : TwitterObject
    {
        #region API Properties

        /// <summary>
        ///     Gets or sets the list.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/list")]
        public TwitterRateLimitResults List { get; set; }

        /// <summary>
        ///     Gets or sets the members.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/members")]
        public TwitterRateLimitResults Members { get; set; }

        /// <summary>
        ///     Gets or sets the membersshow.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/members/show")]
        public TwitterRateLimitResults MembersShow { get; set; }

        /// <summary>
        ///     Gets or sets the memberships.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/memberships")]
        public TwitterRateLimitResults MemberShips { get; set; }

        /// <summary>
        ///     Gets or sets the ownerships.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/ownerships")]
        public TwitterRateLimitResults OwnerShips { get; set; }

        /// <summary>
        ///     Gets or sets the show.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/show")]
        public TwitterRateLimitResults Show { get; set; }

        /// <summary>
        ///     Gets or sets the statuses.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/statuses")]
        public TwitterRateLimitResults Statuses { get; set; }

        /// <summary>
        ///     Gets or sets the subscribers.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/subscribers")]
        public TwitterRateLimitResults Subscribers { get; set; }

        /// <summary>
        ///     Gets or sets the subscribersshow.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/subscribers/show")]
        public TwitterRateLimitResults SubscribersShow { get; set; }

        /// <summary>
        ///     Gets or sets the Subscriptions.
        /// </summary>
        [DataMember, JsonProperty(PropertyName = "/lists/subscriptions")]
        public TwitterRateLimitResults Subscriptions { get; set; }

        #endregion
    }
}