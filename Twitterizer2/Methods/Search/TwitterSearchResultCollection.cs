﻿//-----------------------------------------------------------------------
// <copyright file="TwitterSearchResultCollection.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The twitter search result collection class</summary>
//-----------------------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitterConnector.Core;

namespace TwitterConnector
{
    /// <summary>
    ///     The Twitter Search Result Collection class
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class TwitterSearchResultCollection : TwitterCollection<TwitterSearchResult>, ITwitterObject
    {
        /// <summary>
        ///     Gets or sets the completed_in.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public double CompletedIn { get; internal set; }

        /// <summary>
        ///     Gets or sets the max_id.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public long MaxId { get; internal set; }

        /// <summary>
        ///     Gets or sets the max_id as a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string MaxIdStr { get; internal set; }


        /// <summary>
        ///     Gets or sets the query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string Query { get; internal set; }

        /// <summary>
        ///     Gets or sets the refresh URL.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string RefreshUrl { get; internal set; }

        /// <summary>
        ///     Deserializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static TwitterSearchResultCollection Deserialize(JObject value)
        {
            if (value == null || value["statuses"] == null)
                return null;

            var result = JsonConvert.DeserializeObject<TwitterSearchResultCollection>(value["statuses"].ToString());
            result.CompletedIn = value.SelectToken("search_metadata.completed_in").Value<double>();
            result.MaxId = value.SelectToken("search_metadata.max_id").Value<long>();
            result.MaxIdStr = value.SelectToken("search_metadata.max_id_str").Value<string>();
            result.Query = value.SelectToken("search_metadata.query").Value<string>();
            // result.RefreshUrl = value.SelectToken("search_metadata.refresh_url").Value<string>();

            return result;
        }
    }
}