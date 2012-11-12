using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using TRock.Music;
using TRock.Party.Models;

namespace TRock.Party.Controllers
{
    public class QueryController : ApiController
    {
        private readonly ISongProvider _songProvider;

        public QueryController(ISongProvider songProvider)
        {
            _songProvider = songProvider;
        }

        public async Task<SearchResultModel> GetSongs(string query, CancellationToken token)
        {
            var songs = await _songProvider.GetSongs(query, token);
            var model = new SearchResultModel();
            model.Query = query;
            model.Songs = songs.ToArray();

            var artistGroup = model.Songs.GroupBy(q => q.Artist);
            var albumGroup = model.Songs.GroupBy(q => q.Album);

            model.Albums = albumGroup.Select(album => new ArtistAlbum
            {
                Album = album.Key,
                Artist = album.First().Artist,
                Songs = album.ToArray()
            }).ToArray();

            model.Artists = artistGroup.Select(artist => new ArtistAlbum
            {
                Album = artist.First().Album,
                Artist = artist.Key,
                Songs = artist.ToArray()
            }).ToArray();

            return model;
        }

        public async Task<IEnumerable<Album>> GetAlbums(string artistId, CancellationToken token)
        {
            return await _songProvider.GetAlbums(artistId, token);
        }

        public async Task<ArtistAlbum> GetAlbum(string albumId, CancellationToken token)
        {
            return await _songProvider.GetAlbum(albumId, token);
        }

        public async Task<Artist> GetArtist(string artistId, CancellationToken token)
        {
            return await _songProvider.GetArtist(artistId, token);
        }
    }
}
