﻿//-----------------------------------------------------------------------
// <copyright file="TwitterTrendDictionary.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The twitter trend collection class.</summary>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using Newtonsoft.Json;
using TwitterConnector.Core;

namespace TwitterConnector
{
    /// <summary>
    ///     The TwitterTrendCollection class. Represents multiple <see cref="TwitterConnector.TwitterTrend" /> elements.
    /// </summary>
    [JsonConverter(typeof (Converter))]
#if !SILVERLIGHT
    [Serializable]
#endif
    public class TwitterTrendDictionary : TwitterDictionary<DateTime, TwitterTrendCollection>, ITwitterObject
    {
        /// <summary>
        ///     Gets or sets as of date.
        /// </summary>
        [JsonProperty(PropertyName = "as_of")]
        [JsonConverter(typeof (TwitterConnectorDateConverter))]
        public DateTime AsOf { get; set; }

        /// <summary>
        ///     The Json converter class for the TwitterTrendCollection object
        /// </summary>
#if !SILVERLIGHT
        internal class Converter : JsonConverter
#else
        public class Converter : JsonConverter
#endif
        {
            /// <summary>
            ///     Determines whether this instance can convert the specified object type.
            /// </summary>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>
            ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof (TwitterTrendDictionary);
            }

#if !SILVERLIGHT
            private static readonly string[] dateformats = {"yyyy-MM-dd HH:mm", "yyyy-MM-dd"};
#endif

            /// <summary>
            ///     Reads the json.
            /// </summary>
            /// <param name="reader">The reader.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="existingValue">The existing value.</param>
            /// <param name="serializer">The serializer.</param>
            /// <returns>A collection of <see cref="TwitterTrend" /> items.</returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                var result = existingValue as TwitterTrendDictionary;

                if (result == null)
                    result = new TwitterTrendDictionary();

                int initialDepth = reader.Depth;

                if (reader.TokenType == JsonToken.StartArray)
                    reader.Read();

                while (reader.Read() && reader.Depth > initialDepth)
                {
                    if (reader.TokenType == JsonToken.PropertyName && reader.Depth == 1)
                    {
#if !SILVERLIGHT
                        switch ((string) reader.Value)
                        {
                                //TODO these two datetime converters don't seem to convert.
                            case "as_of":
                                reader.Read();
                                var c = new TwitterConnectorDateConverter();
                                result.AsOf = (DateTime) c.ReadJson(reader, typeof (DateTime), null, serializer);
                                continue;

                            case "trends":
                                reader.Read();
                                while (reader.Read() && reader.Depth >= 3)
                                {
                                    if (reader.TokenType == JsonToken.PropertyName && reader.Depth == 3)
                                    {
                                        try
                                        {
                                            DateTime date = DateTime.ParseExact(reader.Value.ToString(), dateformats,
                                                CultureInfo.InvariantCulture, DateTimeStyles.None);
                                            result.Add(date, new TwitterTrendCollection());

                                            var converter = new TwitterTrendCollection.Converter();
                                            result[date] =
                                                (TwitterTrendCollection)
                                                    converter.ReadJson(reader, typeof (TwitterTrendCollection), null,
                                                        serializer);
                                        }
                                        catch
                                        {
                                            //bad date format
                                            return null;
                                        }
                                    }
                                }
                                continue;
                        }
#endif
                    }
                }
                return result;
            }

            /// <summary>
            ///     Writes the json.
            /// </summary>
            /// <param name="writer">The writer.</param>
            /// <param name="value">The value.</param>
            /// <param name="serializer">The serializer.</param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                // TODO: Implement this.
                // throw new System.NotImplementedException();
            }
        }
    }
}