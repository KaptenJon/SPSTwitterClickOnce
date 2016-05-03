//-----------------------------------------------------------------------
// <copyright file="DeleteListCommand.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The delete list command class</summary>
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
#if !SILVERLIGHT
    [Serializable]
#endif
    internal sealed class DeleteListCommand : TwitterCommand<TwitterList>
    {
        public string Screen_name { get; set; }
        public string Slug { get; set; }

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeleteListCommand" /> class.
        /// </summary>
        /// <param name="requestTokens">The request tokens.</param>
        /// <param name="screen_name">The username.</param>
        /// <param name="slug">The slug.</param>
        /// <param name="options">The options.</param>
        public DeleteListCommand(OAuthTokens requestTokens, string screen_name, string slug, OptionalProperties options)
            : base(HTTPVerb.POST,
                string.Format(CultureInfo.CurrentCulture, "lists/destroy.json", screen_name, slug),
                requestTokens,
                options)
        {
            if (Tokens == null)
            {
                throw new ArgumentNullException("requestTokens");
            }

            if (string.IsNullOrEmpty(screen_name))
            {
                throw new ArgumentNullException("screenName");
            }

            if (string.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException("Slug");
            }
            Slug = slug;
            Screen_name = screen_name;
        }

        #endregion

        /// <summary>
        ///     Initializes the command.
        /// </summary>
        public override void Init()
        {
            RequestParameters.Add("owner_screen_name", Screen_name);
            RequestParameters.Add("slug", Slug);
        }
    }
}