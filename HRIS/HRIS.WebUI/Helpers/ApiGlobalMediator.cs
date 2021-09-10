using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace HRIS.WebUI.Helpers
{
    public class ApiGlobalMediator : HttpClient
    {
        private readonly string _baseUri;
        private TokenResponses _token;
        public ApiGlobalMediator(string baseUri)
        {
            _baseUri = baseUri;

            if (string.IsNullOrWhiteSpace(_baseUri))
            {
                throw new ArgumentNullException(nameof(baseUri), "Service Base Url cannot be null");
            }

            InitializeTlsProtocol();
        }

        public class TokenResponses
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public string Expiry { get; set; }

            //[JsonProperty("expiration")]
            //public DateTime Expiration { get; set; }
        }

        public async Task<string> GetAsyncs<T>(string serviceUrl, Dictionary<string, string> headers, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            if (!string.IsNullOrWhiteSpace(token))
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await GetAsync(AddQueryString(_baseUri + serviceUrl, headers));
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                return responseString;
            }
            else
                response.EnsureSuccessStatusCode();
            throw new Exception(response.ReasonPhrase);
        }
        public async Task<string> GetAsyncs<T>(string url, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await GetAsync(_baseUri + url);
            
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                return responseString;
            }
            else
                response.EnsureSuccessStatusCode();
            throw new Exception(response.ReasonPhrase);
        }

        public async Task<T> GetAsync<T>(string serviceUrl, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (HttpResponseMessage response = await GetAsync(_baseUri + serviceUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<T>();
                }
                else
                    response.EnsureSuccessStatusCode();
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<T> GetAsync<T>(string serviceUrl, Dictionary<string, string> headers, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            if (!string.IsNullOrEmpty(token))
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            using (HttpResponseMessage response = await GetAsync(AddQueryString(_baseUri + serviceUrl, headers)))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<T>();
                }
                else
                    response.EnsureSuccessStatusCode();
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<T> GetAsync<T>(string serviceUrl, string data, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (HttpResponseMessage response = await GetAsync(_baseUri + serviceUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<T>();
                }
                else
                    response.EnsureSuccessStatusCode();
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<string> PostAsync<T>(string serviceUrl, string data, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await PostAsync(_baseUri + serviceUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                return responseString;
            }
            else
                response.EnsureSuccessStatusCode();
            throw new Exception(response.ReasonPhrase);
        }

        public async Task<T> PostAsync<T>(string serviceUrl, StringContent data, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await PostAsync(_baseUri + serviceUrl, data);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<T>();
            }
            else
                response.EnsureSuccessStatusCode();
            throw new Exception(response.ReasonPhrase);
        }

        public async Task<string> PostStringAsync<T>(string serviceUrl, StringContent data, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await PostAsync(_baseUri + serviceUrl, data);
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                return responseString;
            }
            else
                response.EnsureSuccessStatusCode();
            throw new Exception(response.ReasonPhrase);
        }

        public async Task<string> PutAsync<T>(string url, FileStream fs, string token = null)
        {
            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await PutAsync(_baseUri + url, new StreamContent(fs));
            
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                return responseString;
            }
            else
                response.EnsureSuccessStatusCode();
            throw new Exception(response.ReasonPhrase);
        }
        private void InitializeTlsProtocol()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public string GetToken(string tokenUrl, string userName, string password, string granttype)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "grant_type", granttype ),
                new KeyValuePair<string, string>( "username", userName ),
                new KeyValuePair<string, string> ( "Password", password )
            };
            var request = new FormUrlEncodedContent(pairs);
            using var client = GetDefaultHttpClient(userName, password);
            //client.BaseAddress = new Uri(url);
            //authtoken
            var httpResponseMessage = client.PostAsync(tokenUrl, request).Result;
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return response;
        }

        public TokenResponses SetTokenAsync(string tokenUrl, string userName, string password, string granttype)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "grant_type", granttype ),
                new KeyValuePair<string, string>( "username", userName ),
                new KeyValuePair<string, string> ( "Password", password )
            };
            var request = new FormUrlEncodedContent(pairs);
            using var client = GetDefaultHttpClient(userName, password);
            var httpResponseMessage = client.PostAsync(tokenUrl, request).Result;
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                //httpResponseMessage.EnsureSuccessStatusCode();
                return _token;
            }
            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            _token = JsonConvert.DeserializeObject<TokenResponses>(response);
            return _token;
        }

        private HttpClient GetDefaultHttpClient(string _username, string _password)
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUri) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(_username))
                client.DefaultRequestHeaders.Authorization = CreateBasicCredentials(_username, _password);
            return client;
        }

        private HttpClient GetDefaultHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUri) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        static AuthenticationHeaderValue CreateBasicCredentials(string userName, string password)
        {
            string toEncode = userName + ":" + password;
            // The current HTTP specification says characters here are ISO-8859-1.
            // However, the draft specification for the next version of HTTP indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            //Encoding encoding = Encoding.GetEncoding("iso-8859-1");  //Moses
            Encoding encoding = Encoding.UTF8;
            byte[] toBase64 = encoding.GetBytes(toEncode);
            string parameter = Convert.ToBase64String(toBase64);

            return new AuthenticationHeaderValue("Basic", parameter);
        }

        //Utility that translates any dictionary into the query string
        public static string AddQueryString(string uri, IDictionary<string, string> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            if (queryString == null)
            {
                throw new ArgumentNullException(nameof(queryString));
            }
            return AddQueryString(uri, (IEnumerable<KeyValuePair<string, string>>)queryString);
        }
        private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            if (queryString == null)
            {
                throw new ArgumentNullException(nameof(queryString));
            }
            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }
            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;
            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(WebUtility.UrlEncode(parameter.Key));
                sb.Append('=');
                sb.Append(WebUtility.UrlEncode(parameter.Value));
                hasQuery = true;
            }
            sb.Append(anchorText);
            return sb.ToString();
        }
    }
}
