//-----------------------------------------------------------------------
// <copyright file="GetListMembersCommand.cs" company="Patrick 'Ricky' Smith">
//  This file is part of the TwitterConnector library (http://www.TwitterConnector.net)
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
// <summary>The get list members command class.</summary>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using TwitterConnector.Core;

namespace TwitterConnector.Commands
{
    /// <summary>
    ///     Returns the members of the specified list.
    /// </summary>
    [AuthorizedCommand]
    internal class GetListMembersCommand : TwitterCommand<TwitterUserCollection>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetListsCommand" /> class.
        /// </summary>
        /// <param name="requestTokens">The request tokens.</param>
        /// <param name="username">The username.</param>
        /// <param name="slug">The list id or slug.</param>
        /// <param name="options">The options.</param>
        public GetListMembersCommand(OAuthTokens requestTokens, string username, string slug,
            GetListMembersOptions options)
            : base(HTTPVerb.GET, "lists/members.json", requestTokens, options)
        {
            if (requestTokens == null)
            {
                throw new ArgumentNullException("requestTokens");
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            if (string.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException("slug");
            }

            Slug = slug;
            Username = username;

            DeserializationHandler = TwitterUserCollection.DeserializeWrapper;
        }


        /// <summary>
        ///     Gets or sets the slug
        /// </summary>
        /// <value>The user ID.</value>
        public string Slug { get; set; }

        /// <summary>
        ///     Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string Username { get; set; }

        /// <summary>
        ///     Initializes the command.
        /// </summary>
        public override void Init()
        {
            var options = OptionalProperties as GetListMembersOptions;

            if (!String.IsNullOrEmpty(Slug))
                RequestParameters.Add("slug", Slug);

            if (!String.IsNullOrEmpty(Username))
                RequestParameters.Add("owner_screen_name", Username);


            if (options == null || options.Cursor == 0)
            {
                RequestParameters.Add("cursor", "-1");
            }
            else
            {
                RequestParameters.Add("cursor", options.Cursor.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}