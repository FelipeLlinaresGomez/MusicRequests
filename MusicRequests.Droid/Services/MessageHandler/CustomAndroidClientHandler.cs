using System.Net;
using Java.Util.Concurrent;
using Square.OkHttp3;
using Java.Net;
using MusicRequests.Core.Models;
using Javax.Net.Ssl;

namespace MusicRequests.Droid.Services
{
    public class CustomAndroidClientHandler : HttpClientHandler
    {
        readonly OkHttpClient client;
        readonly CacheControl noCacheCacheControl = default(CacheControl);

        readonly Dictionary<HttpRequestMessage, WeakReference> registeredProgressCallbacks =
            new Dictionary<HttpRequestMessage, WeakReference>();
        readonly Dictionary<string, string> headerSeparators =
            new Dictionary<string, string>(){
                {"User-Agent", " "}
            };

        public bool DisableCaching { get; set; }

        public CustomAndroidClientHandler() 
        {
            var clientbuilder = new OkHttpClient.Builder();

            // TLS 1.2
            ConnectionSpec conSpecTls12 = new ConnectionSpec.Builder(ConnectionSpec.ModernTls).TlsVersions(TlsVersion.Tls12).Build();
            ConnectionSpec conSpecClearText = new ConnectionSpec.Builder(ConnectionSpec.Cleartext).Build();
            clientbuilder.ConnectionSpecs(new List<ConnectionSpec> { conSpecTls12, conSpecClearText });

            // Timeout 
            var timeOut = TimeOut?.TotalMilliseconds ?? 180 * 1000; // 90 sec is default value provided by orginal HttpClient in .NET
            clientbuilder.ConnectTimeout((long)timeOut, TimeUnit.Milliseconds);
            clientbuilder.WriteTimeout((long)timeOut, TimeUnit.Milliseconds);
            clientbuilder.ReadTimeout((long)timeOut, TimeUnit.Milliseconds);

            // Cache control
            noCacheCacheControl = (new CacheControl.Builder()).NoCache().Build();

            client = clientbuilder.Build();
        }

        ProgressDelegate GetAndRemoveCallbackFromRegister(HttpRequestMessage request)
        {
            ProgressDelegate emptyDelegate = delegate { };

            lock (registeredProgressCallbacks)
            {
                if (!registeredProgressCallbacks.ContainsKey(request)) return emptyDelegate;

                var weakRef = registeredProgressCallbacks[request];
                if (weakRef == null) return emptyDelegate;

                var callback = weakRef.Target as ProgressDelegate;
                if (callback == null) return emptyDelegate;

                registeredProgressCallbacks.Remove(request);
                return callback;
            }
        }

        string GetHeaderSeparator(string name)
        {
            if (headerSeparators.ContainsKey(name))
            {
                return headerSeparators[name];
            }

            return ",";
        }

        /// <summary>
        /// Gets or sets the number of milliseconds to wait before the request times out.
        /// </summary>
        public TimeSpan? TimeOut { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var java_uri = request.RequestUri.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped);
            var url = new Java.Net.URL(java_uri);

            var body = default(RequestBody);
            if (request.Content != null)
            {
                var bytes = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                var contentType = "text/plain";
                if (request.Content.Headers.ContentType != null)
                {
                    contentType = String.Join(" ", request.Content.Headers.GetValues("Content-Type"));
                }
                body = RequestBody.Create(MediaType.Parse(contentType), bytes);
            }

            var requestBuilder = new Request.Builder()
                .Method(request.Method.Method.ToUpperInvariant(), body)
                .Url(url);

            if (DisableCaching)
            {
                requestBuilder.CacheControl(noCacheCacheControl);
            }

            var keyValuePairs = request.Headers
                .Union(request.Content != null ?
                    (IEnumerable<KeyValuePair<string, IEnumerable<string>>>)request.Content.Headers :
                    Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>());

            foreach (var kvp in keyValuePairs) requestBuilder.AddHeader(kvp.Key, String.Join(GetHeaderSeparator(kvp.Key), kvp.Value));

            cancellationToken.ThrowIfCancellationRequested();

            var rq = requestBuilder.Build();
            var call = client.NewCall(rq);

            // NB: Even closing a socket must be done off the UI thread. Cray!
            cancellationToken.Register(() => Task.Run(() => call.Cancel()));

            var resp = default(Response);
            try
            {
                resp = await call.ExecuteAsync();
                var newReq = resp.Request();
                var newUri = newReq == null ? null : newReq.Url();
                request.RequestUri = new Uri(newUri.ToString());

                var respBody = resp.Body();

                cancellationToken.ThrowIfCancellationRequested();

                var ret = new HttpResponseMessage((HttpStatusCode)resp.Code());
                ret.RequestMessage = request;
                ret.ReasonPhrase = resp.Message();
                if (respBody != null)
                {
                    var content = new ProgressStreamContent(respBody.ByteStream(), CancellationToken.None);
                    content.Progress = GetAndRemoveCallbackFromRegister(request);
                    ret.Content = content;
                }
                else
                {
                    ret.Content = new ByteArrayContent(new byte[0]);
                }

                var respHeaders = resp.Headers();
                foreach (var k in respHeaders.Names())
                {
                    ret.Headers.TryAddWithoutValidation(k, respHeaders.Get(k));
                    ret.Content.Headers.TryAddWithoutValidation(k, respHeaders.Get(k));
                }

                return ret;
            }
            catch (SocketException sex)
            {
                throw new System.OperationCanceledException();
            }
            catch (SSLException exssl)
            {
                throw new MusicRequestBackendException()
                {
                    FriendlyMessage = "An error ocurred while sending the request",
                    Code = "-1"
                };
            }
            catch (Java.IO.IOException ex)
            {
                if (ex.Message.ToLowerInvariant().Contains("canceled"))
                {
                    throw new System.OperationCanceledException();
                }

                throw;
            }
        }

    }
}