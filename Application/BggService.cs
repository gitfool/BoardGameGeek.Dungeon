using Flurl.Http;
using Pocket;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using static Pocket.Logger<BoardGameGeek.Dungeon.BggService>;
// ReSharper disable StringLiteralTypo

namespace BoardGameGeek.Dungeon
{
    public interface IBggService
    {
        Task<ThingItems> DownloadThingsAsync(IEnumerable<int> ids);
        Task<CollectionItems> DownloadUserCollectionAsync(string userName);
        Task<UserPlays> DownloadUserPlaysAsync(string userName, int? year = null, int? id = null, int? page = null);
    }

    public sealed class BggService : IBggService
    {
        public BggService()
        {
            FlurlClient = new FlurlClient("https://boardgamegeek.com/xmlapi2")
            {
                Settings = { BeforeCall = call => { Log.Trace($"{call.Request.Method} {call.Request.RequestUri}"); } }
            };
            RetryPolicy = Policy.Handle<FlurlHttpException>(ex => ex.Call.Response.StatusCode == HttpStatusCode.TooManyRequests)
                .OrResult<HttpResponseMessage>(response => response.StatusCode == HttpStatusCode.Accepted)
                .WaitAndRetryAsync(EnumerateDelay(), (response, _) =>
                {
                    if (response.Exception is FlurlHttpException ex)
                    {
                        Log.Warning($"{ex.Call.HttpStatus:D} {ex.Call.HttpStatus}");
                    }
                    else
                    {
                        Log.Warning($"{response.Result.StatusCode:D} {response.Result.StatusCode}");
                    }
                });
        }

        public async Task<ThingItems> DownloadThingsAsync(IEnumerable<int> ids)
        {
            var request = FlurlClient.Request("thing")
                .SetQueryParams(new
                {
                    id = string.Join(",", ids)
                });
            var response = await RetryPolicy.ExecuteAsync(() => request.GetAsync());
            return await response.Content.ReadAsAsync<ThingItems>(XmlFormatterCollection);
        }

        public async Task<CollectionItems> DownloadUserCollectionAsync(string userName)
        {
            var request = FlurlClient.Request("collection")
                .SetQueryParams(new
                {
                    username = userName,
                    subtype = "boardgame",
                    minplays = 1
                });
            var response = await RetryPolicy.ExecuteAsync(() => request.GetAsync());
            return await response.Content.ReadAsAsync<CollectionItems>(XmlFormatterCollection);
        }

        public async Task<UserPlays> DownloadUserPlaysAsync(string userName, int? year = null, int? id = null, int? page = null)
        {
            var request = FlurlClient.Request("plays")
                .SetQueryParams(new
                {
                    username = userName,
                    subtype = "boardgame",
                    id,
                    mindate = year != null ? $"{year:D4}-01-01" : null,
                    maxdate = year != null ? $"{year:D4}-12-31" : null,
                    page
                });
            var response = await RetryPolicy.ExecuteAsync(() => request.GetAsync());
            return await response.Content.ReadAsAsync<UserPlays>(XmlFormatterCollection);
        }

        private static IEnumerable<TimeSpan> EnumerateDelay()
        {
            return Enumerable.Range(2, 5).Select(seconds => TimeSpan.FromSeconds(Math.Pow(2, seconds)))
                .Concat(Enumerable.Repeat(TimeSpan.FromMinutes(1), int.MaxValue));
        }

        private IFlurlClient FlurlClient { get; }
        private AsyncRetryPolicy<HttpResponseMessage> RetryPolicy { get; }

        private static readonly XmlMediaTypeFormatter XmlFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
        private static readonly MediaTypeFormatterCollection XmlFormatterCollection = new MediaTypeFormatterCollection(new MediaTypeFormatter[] { XmlFormatter });
    }
}
