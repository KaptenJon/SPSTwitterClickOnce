//-----------------------------------------------------------------------
// <copyright file="AddListMemberCommand.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The add list member command class</summary>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using TwitterConnector.Core;

namespace TwitterConnector.Commands
{
    /// <summary>
    ///     Add a member to a list. The authenticated user must own the list to be able to add members to it. Lists are limited
    ///     to having 500 members.
    /// </summary>
    [AuthorizedCommand]
#if !SILVERLIGHT
    [Serializable]
#endif
    internal class AddListMemberCommand : TwitterCommand<TwitterList>
    {
        /// <summary>
        ///     Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public decimal UserId { get; set; }

        public string OwnerScreenName { get; set; }

        public string Slug { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddListMemberCommand" /> class.
        /// </summary>
        /// <param name="requestTokens">The request tokens.</param>
        /// <param name="ownerUsername">The owner username.</param>
        /// <param name="slug">The Slug.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="options">The options.</param>
        public AddListMemberCommand(OAuthTokens requestTokens, string ownerUsername, string slug, decimal userId,
            OptionalProperties options)
            : base(HTTPVerb.POST, "lists/members/create.json", requestTokens, options)
        {
            if (requestTokens == null)
            {
                throw new ArgumentNullException("requestTokens");
            }

            if (string.IsNullOrEmpty(ownerUsername))
            {
                throw new ArgumentNullException("ownerUsername");
            }

            if (string.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException("slug");
            }

            if (userId <= 0)
            {
                throw new ArgumentNullException("userId");
            }

            UserId = userId;
            OwnerScreenName = ownerUsername;
            Slug = slug;
        }


        /// <summary>
        ///     Initializes the command.
        /// </summary>
        public override void Init()
        {
            RequestParameters.Add("user_id", UserId.ToString(CultureInfo.InvariantCulture.NumberFormat));
            RequestParameters.Add("slug", Slug);
            RequestParameters.Add("owner_screen_name", OwnerScreenName);
        }
    }
}