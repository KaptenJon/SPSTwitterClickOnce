using TwitterConnector.Core;

namespace TwitterConnector.Commands
{
    [AuthorizedCommand]
    internal class DestroyListSubscriber : TwitterCommand<TwitterList>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DestroyListSubscriber" /> class.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="listId">The list id.</param>
        /// <param name="options">The options.</param>
        /// <remarks></remarks>
        public DestroyListSubscriber(OAuthTokens tokens, decimal listId, OptionalProperties options)
            : base(HTTPVerb.POST, "lists/subscribers/destroy.json", tokens, options)
        {
            ListId = listId;
        }

        /// <summary>
        ///     Gets or sets the list id.
        /// </summary>
        /// <value>The list id.</value>
        /// <remarks></remarks>
        public decimal ListId { get; set; }

        /// <summary>
        ///     Inits this instance.
        /// </summary>
        /// <remarks></remarks>
        public override void Init()
        {
            RequestParameters.Add("list_id", ListId.ToString());
        }
    }
}