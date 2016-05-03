//-----------------------------------------------------------------------
// <copyright file="WebRequestBuilder.cs" company="Patrick 'Ricky' Smith">
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
// <summary>Provides the means of preparing and executing Anonymous and OAuth signed web requests.</summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TwitterConnector
{
#if SILVERLIGHT
	using System.Net.Browser;
	using System.Threading;
	using System.Windows.Threading;
#endif


    /// <summary>
    ///     Enumeration of the supported HTTP verbs supported by the <see cref="TwitterConnector.Core.CommandPerformer" />
    /// </summary>
    public enum HTTPVerb
    {
        /// <summary>
        ///     The HTTP GET method is used to retrieve data.
        /// </summary>
        GET,

        /// <summary>
        ///     The HTTP POST method is used to transmit data.
        /// </summary>
        POST,

        /// <summary>
        ///     The HTTP DELETE method is used to indicate that a resource should be deleted.
        /// </summary>
        DELETE
    }

    /// <summary>
    ///     The Web Request Builder class.
    /// </summary>
    public sealed class WebRequestBuilder
    {
        /// <summary>
        ///     Holds file data form performing multipart form posts.
        /// </summary>
        private byte[] formData;

        /// <summary>
        ///     The HTTP Authorization realm.
        /// </summary>
        public const string Realm = "Twitter API";

        /// <summary>
        ///     Gets or sets the request URI.
        /// </summary>
        /// <value>The request URI.</value>
        public Uri RequestUri { get; set; }

        /// <summary>
        ///     Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public Dictionary<string, object> Parameters { get; private set; }

        /// <summary>
        ///     Gets or sets the verb.
        /// </summary>
        /// <value>The verb.</value>
        public HTTPVerb Verb { get; set; }

        /// <summary>
        ///     Gets or sets the oauth tokens.
        /// </summary>
        /// <value>The tokens.</value>
        public OAuthTokens Tokens { private get; set; }

        /// <summary>
        ///     Gets or sets the UserAgent.
        /// </summary>
        /// <value>The User Agent.</value>
        private readonly string userAgent;

        /// <summary>
        ///     Gets or sets the Basic Auth Credentials.
        /// </summary>
        /// <value>The Basic Auth Credentials.</value>
        private readonly NetworkCredential networkCredentials;

        /// <summary>
        ///     Gets or sets the Multipart config
        /// </summary>
        /// <value>Multipart</value>
        public bool Multipart { get; set; }

        /// <summary>
        ///     Gets or sets whether to use accept compression on this request
        /// </summary>
        /// <value>UseCompression</value>
        public bool UseCompression { get; set; }

#if !SILVERLIGHT
        /// <summary>
        ///     Gets or sets the proxy.
        /// </summary>
        /// <value>The proxy.</value>
        public WebProxy Proxy { get; set; }
#endif

        /// <summary>
        ///     Gets or sets a value indicating whether the request will be signed with an OAuth authorization header.
        /// </summary>
        /// <value><c>true</c> if [use O auth]; otherwise, <c>false</c>.</value>
        public bool UseOAuth { get; private set; }

        /// <summary>
        ///     OAuth Parameters key names to include in the Authorization header.
        /// </summary>
        private static readonly string[] OAuthParametersToIncludeInHeader =
        {
            "oauth_version",
            "oauth_nonce",
            "oauth_timestamp",
            "oauth_signature_method",
            "oauth_consumer_key",
            "oauth_token",
            "oauth_verifier"
            // Leave signature omitted from the list, it is added manually
            // "oauth_signature",
        };

        /// <summary>
        ///     Parameters that may appear in the list, but should never be included in the header or the request.
        /// </summary>
        private static readonly string[] SecretParameters =
        {
            "oauth_consumer_secret",
            "oauth_token_secret",
            "oauth_signature"
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebRequestBuilder" /> class.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="verb">The http verb.</param>
        /// <param name="userAgent">The http user agent.</param>
        /// <param name="networkCredentials">The network credentials.</param>
        /// <remarks></remarks>
        public WebRequestBuilder(Uri requestUri, HTTPVerb verb, String userAgent, NetworkCredential networkCredentials)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            RequestUri = requestUri;
            Verb = verb;
            this.userAgent = userAgent;
            UseOAuth = false;
            if (networkCredentials != null)
                this.networkCredentials = networkCredentials;

            Parameters = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(RequestUri.Query)) return;

            foreach (Match item in Regex.Matches(RequestUri.Query, @"(?<key>[^&?=]+)=(?<value>[^&?=]+)"))
            {
                Parameters.Add(item.Groups["key"].Value, item.Groups["value"].Value);
            }

            RequestUri = new Uri(RequestUri.AbsoluteUri.Replace(RequestUri.Query, ""));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebRequestBuilder" /> class.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="tokens">The tokens.</param>
        /// <param name="userAgent">The user agent.</param>
        public WebRequestBuilder(Uri requestUri, HTTPVerb verb, OAuthTokens tokens, string userAgent = "")
            : this(requestUri, verb, userAgent, null)
        {
            Tokens = tokens;

            if (tokens != null)
            {
                if (string.IsNullOrEmpty(Tokens.ConsumerKey) || string.IsNullOrEmpty(Tokens.ConsumerSecret))
                {
                    throw new ArgumentException("Consumer key and secret are required for OAuth requests.");
                }

                if (string.IsNullOrEmpty(Tokens.AccessToken) ^ string.IsNullOrEmpty(Tokens.AccessTokenSecret))
                {
                    throw new ArgumentException(
                        "The access token is invalid. You must specify the key AND secret values.");
                }

                UseOAuth = true;
            }
        }

        /// <summary>
        ///     Executes the request.
        /// </summary>
        /// <returns></returns>
        public HttpWebResponse ExecuteRequest()
        {
            HttpWebRequest request = PrepareRequest();

#if !SILVERLIGHT
            return (HttpWebResponse) request.GetResponse();
#else
			request.AllowReadStreamBuffering = true;
			HttpWebResponse response = null;
			AutoResetEvent alldone = new AutoResetEvent(false);
			request.BeginGetResponse(param =>
			{
				HttpWebRequest req = (HttpWebRequest)param.AsyncState;
				try
				{
					response = (HttpWebResponse)req.EndGetResponse(param);
				}
				catch (WebException we)
				{
					response = (HttpWebResponse)we.Response;
				}
				finally
				{
					alldone.Set();
				}
			}, request);
			alldone.WaitOne();
			return response;
#endif
        }

        /// <summary>
        ///     Prepares the request. It is not nessisary to call this method unless additional configuration is required.
        /// </summary>
        /// <returns>A <see cref="HttpWebRequest" /> object fully configured and ready for execution.</returns>
        public HttpWebRequest PrepareRequest()
        {
            SetupOAuth();

            formData = null;
            string contentType = string.Empty;

            if (!Multipart)
            {
                //We don't add the parameters to the query if we are multipart-ing
                AddQueryStringParametersToUri();
            }
            else
            {
                string dataBoundary = "--------------------r4nd0m";
                contentType = "multipart/form-data; boundary=" + dataBoundary;

                formData = GetMultipartFormData(Parameters, dataBoundary);

                Verb = HTTPVerb.POST;
            }

            HttpWebRequest request;
#if SILVERLIGHT
			request = (HttpWebRequest)WebRequestCreator.ClientHttp.Create(this.RequestUri);
			
			if (this.RequestUri.Host.Contains("search.twitter.com") || this.RequestUri.Host.Contains("api.twitter.com")) //-- DG HACK FOR TWITTER QUERIES TO FIX BAD COOKIE DOMAIN= DATA BEING RETURNED FROM TWITTER.
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(this.RequestUri, new Cookie("k", "TwitterConnector hack for bad twitter cookie"));
			}
#else
            request = (HttpWebRequest) WebRequest.Create(RequestUri);

            if (UseCompression)
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            else
                request.AutomaticDecompression = DecompressionMethods.None;

            if (Proxy != null)
                request.Proxy = Proxy;
#endif
            if (!UseOAuth && networkCredentials != null)
            {
                request.Credentials = networkCredentials;
                request.UseDefaultCredentials = false;
            }
            else
            {
                request.UseDefaultCredentials = true;
            }

            request.Method = Verb.ToString();

            request.ContentLength = Multipart ? ((formData != null) ? formData.Length : 0) : 0;

#if !SILVERLIGHT
            // No silverlight user-agent as Assembly.GetName() isn't supported and setting the request.UserAgent is also not supported.
            request.UserAgent = (string.IsNullOrEmpty(userAgent))
                ? string.Format(CultureInfo.InvariantCulture, "TwitterConnector/{0}",
                    Assembly.GetExecutingAssembly().GetName().Version)
                : userAgent;

            request.ServicePoint.Expect100Continue = false;
#endif
            if (UseOAuth)
            {
#if !SILVERLIGHT
                request.Headers.Add("Authorization", GenerateAuthorizationHeader());
#else
				request.Headers["Authorization"] = GenerateAuthorizationHeader();
#endif
            }

            if (Multipart)
            {
                //Parameters are not added to the query string, post them in the request body instead

                request.ContentType = contentType;


#if !SILVERLIGHT
                using (Stream requestStream = request.GetRequestStream())
#else
				IAsyncResult getRequestStreamResult = request.BeginGetRequestStream(
					res =>
					{

					}, null);

				using (Stream requestStream = request.EndGetRequestStream(getRequestStreamResult))
#endif
                {
                    if (formData != null)
                    {
                        requestStream.Write(formData, 0, formData.Length);
                    }
                }
            }

            return request;
        }

        /// <summary>
        ///     Adds the parameters to request uri.
        /// </summary>
        private void AddQueryStringParametersToUri()
        {
            var requestParametersBuilder = new StringBuilder(RequestUri.AbsoluteUri);
            requestParametersBuilder.Append(RequestUri.Query.Length == 0 ? "?" : "&");


            var fieldsToInclude =
                new Dictionary<string, object>(Parameters.Where(
                    p => !OAuthParametersToIncludeInHeader.Contains(p.Key) &&
                         !SecretParameters.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value));

            foreach (var item in fieldsToInclude)
            {
                if (item.Value is string)
                    requestParametersBuilder.AppendFormat("{0}={1}&", item.Key, UrlEncode((string) item.Value));
            }

            if (requestParametersBuilder.Length == 0)
                return;

            requestParametersBuilder.Remove(requestParametersBuilder.Length - 1, 1);

            RequestUri = new Uri(requestParametersBuilder.ToString());
        }

        private byte[] GetMultipartFormData(Dictionary<string, object> param, string boundary)
        {
            Stream formDataStream = new MemoryStream();
            Encoding encoding = Encoding.UTF8;

            var fieldsToInclude =
                new Dictionary<string, object>(param.Where(p => !OAuthParametersToIncludeInHeader.Contains(p.Key) &&
                                                                !SecretParameters.Contains(p.Key))
                    .ToDictionary(p => p.Key, p => p.Value));

            foreach (var kvp in fieldsToInclude)
            {
                if (kvp.Value.GetType() == typeof (byte[]))
                {
                    //assume this to be a byte stream
                    var data = (byte[]) kvp.Value;

                    string header =
                        string.Format(
                            "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: application/octet-stream\r\n\r\n",
                            boundary,
                            kvp.Key,
                            kvp.Key);

                    byte[] headerBytes = encoding.GetBytes(header);

                    formDataStream.Write(headerBytes, 0, headerBytes.Length);
                    formDataStream.Write(data, 0, data.Length);
                }
                else
                {
                    //this is normal text data
                    string header = string.Format(
                        "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n",
                        boundary,
                        kvp.Key,
                        kvp.Value);

                    byte[] headerBytes = encoding.GetBytes(header);

                    formDataStream.Write(headerBytes, 0, headerBytes.Length);
                }
            }

            string footer = string.Format("\r\n--{0}--\r\n", boundary);
            formDataStream.Write(encoding.GetBytes(footer), 0, footer.Length);
            formDataStream.Position = 0;
            var returndata = new byte[formDataStream.Length];

            formDataStream.Read(returndata, 0, returndata.Length);
            formDataStream.Close();

            return returndata;
        }

        #region OAuth Helper Methods

        /// <summary>
        ///     Sets up the OAuth request details.
        /// </summary>
        private void SetupOAuth()
        {
            // We only sign oauth requests
            if (!UseOAuth)
            {
                return;
            }

            // Add the OAuth parameters
            Parameters.Add("oauth_version", "1.0");
            Parameters.Add("oauth_nonce", GenerateNonce());
            Parameters.Add("oauth_timestamp", GenerateTimeStamp());
            Parameters.Add("oauth_signature_method", "HMAC-SHA1");
            Parameters.Add("oauth_consumer_key", Tokens.ConsumerKey);
            Parameters.Add("oauth_consumer_secret", Tokens.ConsumerSecret);

            if (!string.IsNullOrEmpty(Tokens.AccessToken))
            {
                Parameters.Add("oauth_token", Tokens.AccessToken);
            }

            if (!string.IsNullOrEmpty(Tokens.AccessTokenSecret))
            {
                Parameters.Add("oauth_token_secret", Tokens.AccessTokenSecret);
            }

            string signature = GenerateSignature();

            // Add the signature to the oauth parameters
            Parameters.Add("oauth_signature", signature);
        }

        /// <summary>
        ///     Generates the signature.
        /// </summary>
        /// <returns></returns>
        public string GenerateSignature()
        {
            IEnumerable<KeyValuePair<string, object>> nonSecretParameters;

            if (Multipart)
            {
                nonSecretParameters = (from p in Parameters
                    where (!SecretParameters.Contains(p.Key) && p.Key.StartsWith("oauth_"))
                    select p);
            }
            else
            {
                nonSecretParameters = (from p in Parameters
                    where (!SecretParameters.Contains(p.Key))
                    select p);
            }

            Uri urlForSigning = RequestUri;

            // Create the base string. This is the string that will be hashed for the signature.
            string signatureBaseString = string.Format(
                CultureInfo.InvariantCulture,
                "{0}&{1}&{2}",
                Verb.ToString().ToUpper(CultureInfo.InvariantCulture),
                UrlEncode(NormalizeUrl(urlForSigning)),
                UrlEncode(nonSecretParameters));

            // Create our hash key (you might say this is a password)
            string key = string.Format(
                CultureInfo.InvariantCulture,
                "{0}&{1}",
                UrlEncode(Tokens.ConsumerSecret),
                UrlEncode(Tokens.AccessTokenSecret));


            // Generate the hash
            var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            byte[] signatureBytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(signatureBaseString));
            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>
        ///     Generate the timestamp for the signature
        /// </summary>
        /// <returns>A timestamp value in a string.</returns>
        public static string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Generate a nonce
        /// </summary>
        /// <returns>A random number between 123400 and 9999999 in a string.</returns>
        public static string GenerateNonce()
        {
            // Just a simple implementation of a random number between 123400 and 9999999
            return new Random()
                .Next(123400, int.MaxValue)
                .ToString("X", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Normalizes the URL.
        /// </summary>
        /// <param name="url">The URL to normalize.</param>
        /// <returns>The normalized url string.</returns>
        public static string NormalizeUrl(Uri url)
        {
            string normalizedUrl = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }

            normalizedUrl += url.AbsolutePath;
            return normalizedUrl;
        }

        /// <summary>
        ///     Encodes a value for inclusion in a URL querystring.
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            value = RFC3986Encode.EscapeDataString(value);
            // these characters are not escaped by UrlEncode() but needed to be escaped
            value = value
                .Replace("(", "%28")
                .Replace(")", "%29")
                .Replace("[", "%5B")
                .Replace("]", "%5D")
                .Replace("$", "%24")
                .Replace("!", "%21")
                .Replace("*", "%2A")
                .Replace("'", "%27");
            return value;
        }

        /// <summary>
        ///     Encodes a series of key/value pairs for inclusion in a URL querystring.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A string of all the <paramref name="parameters" /> keys and value pairs with the values encoded.</returns>
        private static string UrlEncode(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var parameterString = new StringBuilder();

            IOrderedEnumerable<KeyValuePair<string, object>> paramsSorted = from p in parameters
                orderby p.Key, p.Value
                select p;

            foreach (var item in paramsSorted)
            {
                if (item.Value is string)
                {
                    if (parameterString.Length > 0)
                    {
                        parameterString.Append("&");
                    }

                    parameterString.Append(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}={1}",
                            UrlEncode(item.Key),
                            UrlEncode((string) item.Value)));
                }
            }

            return UrlEncode(parameterString.ToString());
        }

        /// <summary>
        ///     Generates the authorization header.
        /// </summary>
        /// <returns>The string value of the HTTP header to be included for OAuth requests.</returns>
        public string GenerateAuthorizationHeader()
        {
            var authHeaderBuilder = new StringBuilder();
            authHeaderBuilder.AppendFormat("OAuth realm=\"{0}\"", Realm);

            IOrderedEnumerable<KeyValuePair<string, object>> sortedParameters = from p in Parameters
                where OAuthParametersToIncludeInHeader.Contains(p.Key)
                orderby p.Key, UrlEncode((p.Value is string) ? (string) p.Value : string.Empty)
                select p;

            foreach (var item in sortedParameters)
            {
                authHeaderBuilder.AppendFormat(
                    ",{0}=\"{1}\"",
                    UrlEncode(item.Key),
                    UrlEncode(item.Value as string));
            }

            authHeaderBuilder.AppendFormat(",oauth_signature=\"{0}\"",
                UrlEncode(Parameters["oauth_signature"] as string));

            return authHeaderBuilder.ToString();
        }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class RFC3986Encode
    {
        /// <summary>
        /// </summary>
        public static readonly string Dontescapecharacters =
            "-._~";

        /// <summary>
        /// </summary>
        /// <param name="stringToEscape"></param>
        /// <returns></returns>
        public static string EscapeDataString(string stringToEscape)
        {
            return PercentEncodeString(
                stringToEscape, Dontescapecharacters);
        }

        internal static string PercentEncodeString(string stringToEscape,
            string dontEscapeCharacters)
        {
            var encodedString =
                new StringBuilder();

            foreach (char c in stringToEscape)
            {
                if (('0' <= c && c <= '9') ||
                    ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') ||
                    (0 <= dontEscapeCharacters.IndexOf(c)))
                {
                    //Using enocode
                    encodedString.Append(c);
                }
                else
                {
                    //Not using encode
                    encodedString.Append(HexEscape(c));
                }
            }

            return Regex.Replace(encodedString.ToString(), "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
        }

        /// <summary>
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string HexEscape(char character)
        {
            if (255 < character)
            {
                //when character is over 255, can't use HexEscape.
                var buf = new StringBuilder();
                byte[] characterBytes =
                    Encoding.UTF8.GetBytes(character.ToString());
                foreach (byte b in characterBytes)
                {
                    buf.AppendFormat("%{0:X2}", b);
                }

                return buf.ToString();
            }

            return Uri.HexEscape(character);
        }
    }
}