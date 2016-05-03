//-----------------------------------------------------------------------
// <copyright file="TwitterCommand.cs" company="Patrick Ricky Smith">
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
// <summary>The base class for all command classes.</summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace TwitterConnector.Core
{
    #if !SILVERLIGHT
#endif

    /// <summary>
    ///     The base command class.
    /// </summary>
    /// <typeparam name="T">The business object the command should return.</typeparam>
#if !SILVERLIGHT
    [Serializable]
#endif
    internal abstract class TwitterCommand<T> : ICommand<T>
        where T : ITwitterObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TwitterCommand&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="tokens">The tokens.</param>
        /// <param name="optionalProperties">The optional properties.</param>
        protected TwitterCommand(HTTPVerb method, string endPoint, OAuthTokens tokens,
            OptionalProperties optionalProperties)
        {
            RequestParameters = new Dictionary<string, object>();
            Verb = method;
            Tokens = tokens;
            OptionalProperties = optionalProperties ?? new OptionalProperties();

            SetCommandUri(endPoint);
        }

        /// <summary>
        ///     Gets or sets the optional properties.
        /// </summary>
        /// <value>The optional properties.</value>
        protected OptionalProperties OptionalProperties { get; set; }

        /// <summary>
        ///     Gets or sets the API method URI.
        /// </summary>
        /// <value>The URI for the API method.</value>
        private Uri Uri { get; set; }

        /// <summary>
        ///     Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        private HTTPVerb Verb { get; set; }

        /// <summary>
        ///     Gets or sets the request parameters.
        /// </summary>
        /// <value>The request parameters.</value>
        public Dictionary<string, object> RequestParameters { get; set; }

        /// <summary>
        ///     Gets or sets the serialization delegate.
        /// </summary>
        /// <value>The serialization delegate.</value>
        protected SerializationHelper<T>.DeserializationHandler DeserializationHandler { get; set; }

        /// <summary>
        ///     Gets the request tokens.
        /// </summary>
        /// <value>The request tokens.</value>
        internal OAuthTokens Tokens { get; private set; }

        /// <summary>
        ///     Initializes the command.
        /// </summary>
        public abstract void Init();

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="TwitterCommand&lt;T&gt;" /> is multipart.
        /// </summary>
        /// <value><c>true</c> if multipart; otherwise, <c>false</c>.</value>
        protected bool Multipart { get; set; }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns>The results of the command.</returns>
        public TwitterResponse<T> ExecuteCommand()
        {
            var twitterResponse = new TwitterResponse<T>();

            if (OptionalProperties.UseSSL)
            {
                Uri = new Uri(Uri.AbsoluteUri.Replace("http://", "https://"));
            }

            // Loop through all of the custom attributes assigned to the command class
            foreach (Attribute attribute in GetType().GetCustomAttributes(false))
            {
                if (attribute is AuthorizedCommandAttribute)
                {
                    if (Tokens == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                            "Tokens are required for the \"{0}\" command.", GetType()));
                    }

                    if (string.IsNullOrEmpty(Tokens.ConsumerKey) ||
                        string.IsNullOrEmpty(Tokens.ConsumerSecret) ||
                        string.IsNullOrEmpty(Tokens.AccessToken) ||
                        string.IsNullOrEmpty(Tokens.AccessTokenSecret))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                            "Token values cannot be null when executing the \"{0}\" command.", GetType()));
                    }
                }
            }

            // Prepare the query parameters
            var queryParameters = new Dictionary<string, object>();
            foreach (var item in RequestParameters)
            {
                queryParameters.Add(item.Key, item.Value);
            }

            // Declare the variable to be returned
            twitterResponse.ResponseObject = default(T);
            twitterResponse.RequestUrl = Uri.AbsoluteUri;
            RateLimiting rateLimiting;
            AccessLevel accessLevel;
            byte[] responseData;

            try
            {
                var requestBuilder = new WebRequestBuilder(Uri, Verb, Tokens) {Multipart = Multipart};

#if !SILVERLIGHT
                if (OptionalProperties != null)
                    requestBuilder.Proxy = OptionalProperties.Proxy;
#endif

                foreach (var item in queryParameters)
                {
                    requestBuilder.Parameters.Add(item.Key, item.Value);
                }

                HttpWebResponse response = requestBuilder.ExecuteRequest();

                if (response == null)
                {
                    twitterResponse.Result = RequestResult.Unknown;
                    return twitterResponse;
                }

                responseData = ConversionUtility.ReadStream(response.GetResponseStream());
                twitterResponse.Content = Encoding.UTF8.GetString(responseData, 0, responseData.Length);

                twitterResponse.RequestUrl = requestBuilder.RequestUri.AbsoluteUri;

#if !SILVERLIGHT
                // Parse the rate limiting HTTP Headers
                rateLimiting = ParseRateLimitHeaders(response.Headers);

                // Parse Access Level
                accessLevel = ParseAccessLevel(response.Headers);
#else
                rateLimiting = null;
                accessLevel = AccessLevel.Unknown;

#endif

                // Lookup the status code and set the status accordingly
                SetStatusCode(twitterResponse, response.StatusCode, rateLimiting);

                twitterResponse.RateLimiting = rateLimiting;
                twitterResponse.AccessLevel = accessLevel;
            }
            catch (WebException wex)
            {
                if (new[]
                {
#if !SILVERLIGHT
                    WebExceptionStatus.Timeout,
                    WebExceptionStatus.ConnectionClosed,
#endif
                    WebExceptionStatus.ConnectFailure
                }.Contains(wex.Status))
                {
                    twitterResponse.Result = RequestResult.ConnectionFailure;
                    twitterResponse.ErrorMessage = wex.Message;
                    return twitterResponse;
                }

                // The exception response should always be an HttpWebResponse, but we check for good measure.
                var exceptionResponse = wex.Response as HttpWebResponse;

                if (exceptionResponse == null)
                {
                    throw;
                }

                responseData = ConversionUtility.ReadStream(exceptionResponse.GetResponseStream());
                twitterResponse.Content = Encoding.UTF8.GetString(responseData, 0, responseData.Length);

#if !SILVERLIGHT
                rateLimiting = ParseRateLimitHeaders(exceptionResponse.Headers);

                // Parse Access Level
                accessLevel = ParseAccessLevel(exceptionResponse.Headers);
#else
                rateLimiting = null;
                accessLevel = AccessLevel.Unknown;
#endif

                // Try to read the error message, if there is one.
                try
                {
                    TwitterErrorDetails errorDetails = SerializationHelper<TwitterErrorDetails>.Deserialize(responseData);
                    twitterResponse.ErrorMessage = errorDetails.ErrorMessage;
                }
                catch (Exception)
                {
                    // Occasionally, Twitter responds with XML error data even though we asked for json.
                    // This is that scenario. We will deal with it by doing nothing. It's up to the developer to deal with it.
                }

                // Lookup the status code and set the status accordingly
                SetStatusCode(twitterResponse, exceptionResponse.StatusCode, rateLimiting);

                twitterResponse.RateLimiting = rateLimiting;
                twitterResponse.AccessLevel = accessLevel;

                if (wex.Status == WebExceptionStatus.UnknownError)
                    throw;

                return twitterResponse;
            }

            try
            {
                twitterResponse.ResponseObject = SerializationHelper<T>.Deserialize(responseData, DeserializationHandler);
            }
            catch (JsonReaderException)
            {
                twitterResponse.ErrorMessage = "Unable to parse JSON";
                twitterResponse.Result = RequestResult.Unknown;
                return twitterResponse;
            }
            catch (JsonSerializationException)
            {
                twitterResponse.ErrorMessage = "Unable to parse JSON";
                twitterResponse.Result = RequestResult.Unknown;
                return twitterResponse;
            }

            // Pass the current oauth tokens into the new object, so method calls from there will keep the authentication.
            twitterResponse.Tokens = Tokens;

            return twitterResponse;
        }

        /// <summary>
        ///     Sets the status code.
        /// </summary>
        /// <param name="twitterResponse">The twitter response.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="rateLimiting">The rate limiting.</param>
        private static void SetStatusCode(TwitterResponse<T> twitterResponse, HttpStatusCode statusCode,
            RateLimiting rateLimiting)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    twitterResponse.Result = RequestResult.Success;
                    break;

                case HttpStatusCode.BadRequest:
                    twitterResponse.Result = (rateLimiting != null && rateLimiting.Remaining == 0)
                        ? RequestResult.RateLimited
                        : RequestResult.BadRequest;
                    break;

                case (HttpStatusCode) 420: //Rate Limited from Search/Trends API
                    twitterResponse.Result = RequestResult.RateLimited;
                    break;

                case HttpStatusCode.Unauthorized:
                    twitterResponse.Result = RequestResult.Unauthorized;
                    break;

                case HttpStatusCode.NotFound:
                    twitterResponse.Result = RequestResult.FileNotFound;
                    break;

                case HttpStatusCode.ProxyAuthenticationRequired:
                    twitterResponse.Result = RequestResult.ProxyAuthenticationRequired;
                    break;

                case HttpStatusCode.RequestTimeout:
                    twitterResponse.Result = RequestResult.TwitterIsOverloaded;
                    break;

                case HttpStatusCode.Forbidden:
                    twitterResponse.Result = RequestResult.Unauthorized;
                    break;

                default:
                    twitterResponse.Result = RequestResult.Unknown;
                    break;
            }
        }

        /// <summary>
        ///     Sets the command URI.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        protected void SetCommandUri(string endPoint)
        {
            if (endPoint.StartsWith("/"))
                throw new ArgumentException(
                    "The API endpoint cannot begin with a forward slash. This will result in 404 errors and headaches.",
                    "endPoint");

            Uri = new Uri(string.Concat(OptionalProperties.APIBaseAddress, endPoint));
        }

        /// <summary>
        ///     Parses the rate limit headers.
        /// </summary>
        /// <param name="responseHeaders">The headers of the web response.</param>
        /// <returns>An object that contains the rate-limiting info contained in the response headers</returns>
        private static RateLimiting ParseRateLimitHeaders(WebHeaderCollection responseHeaders)
        {
            var rateLimiting = new RateLimiting();

            if (responseHeaders.AllKeys.Contains("x-rate-limit-limit"))
            {
                rateLimiting.Total = int.Parse(responseHeaders["x-rate-limit-limit"], CultureInfo.InvariantCulture);
            }

            if (responseHeaders.AllKeys.Contains("x-rate-limit-remaining"))
            {
                rateLimiting.Remaining = int.Parse(responseHeaders["x-rate-limit-remaining"],
                    CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(responseHeaders["x-rate-limit-reset"]))
            {
                rateLimiting.ResetDate = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0)
                    .AddSeconds(double.Parse(responseHeaders["x-rate-limit-reset"], CultureInfo.InvariantCulture)),
                    DateTimeKind.Utc);
            }
            else if (!string.IsNullOrEmpty(responseHeaders["Retry-After"]))
            {
                rateLimiting.ResetDate = DateTime.UtcNow.AddSeconds(Convert.ToInt32(responseHeaders["Retry-After"]));
            }

            return rateLimiting;
        }

        /// <summary>
        ///     Parses the access level headers.
        /// </summary>
        /// <param name="responseHeaders">The headers of the web response.</param>
        /// <returns>An enum of the current access level of the OAuth Token being used.</returns>
        private AccessLevel ParseAccessLevel(WebHeaderCollection responseHeaders)
        {
            if (responseHeaders.AllKeys.Contains("X-Access-Level"))
            {
                switch (responseHeaders["X-Access-Level"].ToLower())
                {
                    case "read":
                        return AccessLevel.Read;
                    case "read-write":
                        return AccessLevel.ReadWrite;
                    case "read-write-privatemessages":
                    case "read-write-directmessages":
                        return AccessLevel.ReadWriteDirectMessage;
                }
                return AccessLevel.Unknown;
            }
            return AccessLevel.Unavailable;
        }
    }
}