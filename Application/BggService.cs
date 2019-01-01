using Pocket;
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
        public async Task<ThingItems> DownloadThingsAsync(IEnumerable<int> ids)
        {
            var query = $"id={string.Join(",", ids)}";
            var uri = new Uri($"https://boardgamegeek.com/xmlapi2/thing?{query}");
            Log.Info($"    {uri}");
            return await DownloadAsync<ThingItems>(uri);
        }

        public async Task<CollectionItems> DownloadUserCollectionAsync(string userName)
        {
            var query = $"username={userName}&subtype=boardgame&minplays=1";
            var uri = new Uri($"https://boardgamegeek.com/xmlapi2/collection?{query}");
            Log.Info($"    {uri}");
            await Task.Delay(TimeSpan.FromSeconds(5));
            var response = await HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                // collection requests are queued; http://boardgamegeek.com/wiki/page/BGG_XML_API2#toc11
                using (var delay = EnumerateDelay().GetEnumerator())
                {
                    do
                    {
                        delay.MoveNext();
                        await Task.Delay(delay.Current);
                        response = await HttpClient.GetAsync(uri);
                        response.EnsureSuccessStatusCode();
                    }
                    while (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.TooManyRequests);
                }
            }
            return await response.Content.ReadAsAsync<CollectionItems>(XmlFormatterCollection);
        }

        public async Task<UserPlays> DownloadUserPlaysAsync(string userName, int? year = null, int? id = null, int? page = null)
        {
            var query = $"username={userName}&subtype=boardgame";
            if (id != null)
            {
                query += $"&id={id}";
            }
            if (year != null)
            {
                query += $"&mindate={year}-01-01&maxdate={year}-12-31";
            }
            if (page != null)
            {
                query += $"&page={page}";
            }
            var uri = new Uri($"https://boardgamegeek.com/xmlapi2/plays?{query}");
            Log.Info($"    {uri}");
            return await DownloadAsync<UserPlays>(uri);
        }

        private async Task<T> DownloadAsync<T>(Uri uri)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            var response = await HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>(XmlFormatterCollection);
        }

        private static IEnumerable<TimeSpan> EnumerateDelay()
        {
            return Enumerable.Range(3, 5).Select(seconds => TimeSpan.FromSeconds(Math.Pow(2, seconds)))
                .Concat(Enumerable.Repeat(TimeSpan.FromMinutes(1), int.MaxValue));
        }

        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly XmlMediaTypeFormatter XmlFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
        private static readonly MediaTypeFormatterCollection XmlFormatterCollection = new MediaTypeFormatterCollection(new MediaTypeFormatter[] { XmlFormatter });
    }
}
