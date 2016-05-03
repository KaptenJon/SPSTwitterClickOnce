//-----------------------------------------------------------------------
// <copyright file="GetListSubscriptionsCommand.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The get list subscriptions command class</summary>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using TwitterConnector.Core;

namespace TwitterConnector.Commands
{
    /// <summary>
    ///     The create list command class
    /// </summary>
    [AuthorizedCommand]
    internal sealed class GetListSubscriptionsCommand : TwitterCommand<TwitterListCollection>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetListSubscriptionsCommand" /> class.
        /// </summary>
        /// <param name="requestTokens">The request tokens.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="options">The options.</param>
        /// <remarks></remarks>
        public GetListSubscriptionsCommand(OAuthTokens requestTokens, string userName,
            GetListSubscriptionsOptions options)
            : base(HTTPVerb.GET, string.Format("/lists/subscriptions.json", userName), requestTokens, options)
        {
            if (requestTokens == null)
            {
                throw new ArgumentNullException("requestTokens");
            }

            DeserializationHandler = TwitterListCollection.Deserialize;
        }

        /// <summary>
        ///     Initializes the command.
        /// </summary>
        public override void Init()
        {
            var options = OptionalProperties as GetListSubscriptionsOptions;
            if (options == null || options.Cursor <= 0)
            {
                RequestParameters.Add("cursor", "-1");
            }
            else
                RequestParameters.Add("cursor", options.Cursor.ToString(CultureInfo.CurrentCulture));
        }
    }
}