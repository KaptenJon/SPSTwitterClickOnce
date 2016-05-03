﻿//-----------------------------------------------------------------------
// <copyright file="TwitterSpam.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The TwitterSpam class.</summary>
//-----------------------------------------------------------------------

using TwitterConnector.Commands;
using TwitterConnector.Core;

namespace TwitterConnector
{
    /// <summary>
    ///     Provides methods for reporting users and tweets as inappropriate or spam.
    /// </summary>
    public class TwitterSpam
    {
        /// <summary>
        ///     Blocks the user and reports them for spam/abuse.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="options">The options.</param>
        /// <returns>The user details.</returns>
        public static TwitterResponse<TwitterUser> ReportUser(OAuthTokens tokens, decimal userId,
            OptionalProperties options)
        {
            var command = new ReportSpamCommand(tokens, userId, string.Empty, options);

            return CommandPerformer.PerformAction(command);
        }

        /// <summary>
        ///     Blocks the user and reports them for spam/abuse.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The user details.</returns>
        public static TwitterResponse<TwitterUser> ReportUser(OAuthTokens tokens, decimal userId)
        {
            return ReportUser(tokens, userId, null);
        }

        /// <summary>
        ///     Blocks the user and reports them for spam/abuse.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="screenName">The user's screen name.</param>
        /// <param name="options">The options.</param>
        /// <returns>The user details.</returns>
        public static TwitterResponse<TwitterUser> ReportUser(OAuthTokens tokens, string screenName,
            OptionalProperties options)
        {
            var command = new ReportSpamCommand(tokens, 0, screenName, options);

            return CommandPerformer.PerformAction(command);
        }

        /// <summary>
        ///     Blocks the user and reports them for spam/abuse.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="screenName">The user's screen name.</param>
        /// <returns>The user details.</returns>
        public static TwitterResponse<TwitterUser> ReportUser(OAuthTokens tokens, string screenName)
        {
            return ReportUser(tokens, screenName, null);
        }
    }
}