//-----------------------------------------------------------------------
// <copyright file="SearchCommand.cs" company="Patrick 'Ricky' Smith">
//  This file is part of the TwitterConnector library (http://www.TwitterConnector.net/)
// 
//  Copyright (c) 2010, Patrick "Ricky" Smith (ricky@digitally-born.com)
//  All rights reserved.
//  
//  Redistribution and use in source and binary forms, with or without modification, are 
//  permitted provided that the following conditions are met:
// 
//  - Redistributions of source code must retain the above copyright notice, this list 
//    of conditions and the following disclaimer.
//  - Redistributions in binary form must reproduce the above copyright notice, this list 
//    of conditions and the following disclaimer in the documentation and/or other 
//    materials provided with the distribution.
//  - Neither the name of the TwitterConnector nor the names of its contributors may be 
//    used to endorse or promote products derived from this software without specific 
//    prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//  POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <author>Ricky Smith</author>
// <summary>The search command class</summary>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using TwitterConnector.Core;

namespace TwitterConnector.Commands
{
#if !SILVERLIGHT
    [Serializable]
#endif
    internal sealed class SearchCommand : TwitterCommand<TwitterSearchResultCollection>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchCommand" /> class.
        /// </summary>
        /// <param name="requestTokens">The request tokens.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        public SearchCommand(OAuthTokens requestTokens, string query, SearchOptions options)
            : base(HTTPVerb.GET, "tweets.json", requestTokens, options)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }

            Query = query;

            DeserializationHandler = TwitterSearchResultCollection.Deserialize;
        }

        #endregion

        /// <summary>
        ///     Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public string Query { get; set; }

        /// <summary>
        ///     Initializes the command.
        /// </summary>
        public override void Init()
        {
#if !SILVERLIGHT
            CultureInfo unitedStatesEnglishCulture = CultureInfo.GetCultureInfo("en-us");
#else
            CultureInfo unitedStatesEnglishCulture = CultureInfo.InvariantCulture;
#endif

            RequestParameters.Add("q", Query);

            var options = OptionalProperties as SearchOptions;

            if (options == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(options.Language))
            {
                RequestParameters.Add("lang", options.Language);
            }

            if (!string.IsNullOrEmpty(options.Locale))
            {
                RequestParameters.Add("locale", options.Locale);
            }

            if (options.MaxId > 0)
            {
                RequestParameters.Add("max_id", options.MaxId.ToString(unitedStatesEnglishCulture));
            }

            if (options.NumberPerPage > 0)
            {
                RequestParameters.Add("count", options.NumberPerPage.ToString(unitedStatesEnglishCulture));
            }

            if (options.PageNumber > 0)
            {
                RequestParameters.Add("page", options.PageNumber.ToString(unitedStatesEnglishCulture));
            }

            if (options.SinceDate > new DateTime())
            {
                RequestParameters.Add("since", options.SinceDate.ToString("yyyy-MM-dd", unitedStatesEnglishCulture));
            }

            if (options.SinceId > 0)
            {
                RequestParameters.Add("since_id", options.SinceId.ToString(unitedStatesEnglishCulture));
            }

            if (!string.IsNullOrEmpty(options.GeoCode))
            {
                RequestParameters.Add("geocode", options.GeoCode);
            }

            if (options.PrefixUsername)
            {
                RequestParameters.Add("show_user", "true");
            }

            if (options.UntilDate > new DateTime())
            {
                RequestParameters.Add("until", options.UntilDate.ToString("{0:yyyy-MM-dd}", unitedStatesEnglishCulture));
            }

            switch (options.ResultType)
            {
                case SearchOptionsResultType.Mixed:
                    RequestParameters.Add("result_type", "mixed");
                    break;
                case SearchOptionsResultType.Recent:
                    RequestParameters.Add("result_type", "recent");
                    break;
                case SearchOptionsResultType.Popular:
                    RequestParameters.Add("result_type", "popular");
                    break;
            }

            if (options.WithTwitterUserID)
                RequestParameters.Add("with_twitter_user_id", "true");

            if (options.IncludeEntities)
                RequestParameters.Add("include_entities", "true");
        }
    }
}