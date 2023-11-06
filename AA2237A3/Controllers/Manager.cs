using AA2237A3.Data;
using AA2237A3.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

// ************************************************************************************
// WEB524 Project Template V1 == 2237-b6459d96-60b8-4e5e-885d-102a194a7741
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************

namespace AA2237A3.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private DataContext ds = new DataContext();

        // AutoMapper instance
        public IMapper mapper;

        public Manager()
        {
            // If necessary, add more constructor code here...

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Product, ProductBaseViewModel>();

                // Album
                cfg.CreateMap<Album, AlbumBaseViewModel>();

                // Artist
                cfg.CreateMap<Artist, ArtistBaseViewModel>();

                // MediaType
                cfg.CreateMap<MediaType, MediaTypeBaseViewModel>();

                // Track
                cfg.CreateMap<Track, TrackWithDetailViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();

                // Playlist
                cfg.CreateMap<Playlist, PlaylistBaseViewModel>();
                cfg.CreateMap<Playlist, PlaylistEditTracksFormViewModel>();
                cfg.CreateMap<PlaylistBaseViewModel, PlaylistEditTracksFormViewModel>();
                cfg.CreateMap<Playlist, PlaylistEditTracksViewModel>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }


        // Add your methods below and call them from controllers. Ensure that your methods accept
        // and deliver ONLY view model objects and collections. When working with collections, the
        // return type is almost always IEnumerable<T>.
        //
        // Remember to use the suggested naming convention, for example:
        // ProductGetAll(), ProductGetById(), ProductAdd(), ProductEdit(), and ProductDelete().


        public IEnumerable<AlbumBaseViewModel> AlbumGetAll()
        {
            var allAlbums = ds.Albums.OrderBy(a => a.Title);
            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(allAlbums);
        }

        public IEnumerable<ArtistBaseViewModel> ArtistGetAll()
        {
            var allArtists = ds.Artists.OrderBy(a => a.Name);
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(allArtists);
        }

        public IEnumerable<MediaTypeBaseViewModel> MediaTypeGetAll()
        {
            var allMediaTypes = ds.MediaTypes.OrderBy(m => m.Name);
            return mapper.Map<IEnumerable<MediaType>, IEnumerable<MediaTypeBaseViewModel>>(allMediaTypes);
        }

        public IEnumerable<TrackWithDetailViewModel> TrackGetAllWithDetail()
        {
            var allTracks = ds.Tracks
                              .Include("MediaType")
                              .Include("Album")
                              .Include("Album.Artist")
                              .OrderBy(t => t.Name);
            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackWithDetailViewModel>>(allTracks);
        }

        public TrackWithDetailViewModel TrackGetByIdWithDetail(int theId)
        {
            var track = ds.Tracks
                          .Include("MediaType")
                          .Include("Album")
                          .Include("Album.Artist")
                          .SingleOrDefault(t => t.TrackId == theId);

            if (track == null)
            {
                return null; // Return null if no invoice is found
            }

            return mapper.Map<Track, TrackWithDetailViewModel>(track);
        }

        // TODO: TrackAdd
        public TrackWithDetailViewModel TrackAdd(TrackAddViewModel newItem)
        {
            var album = ds.Albums.Find(newItem.AlbumId);
            var mediaType = ds.MediaTypes.Find(newItem.MediaTypeId);

            if (album == null || mediaType == null) 
            { 
                return null;
            }
            else
            {
                var addedItem = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newItem));
                addedItem.MediaType = mediaType;
                addedItem.Album = album;
                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Track, TrackWithDetailViewModel>(addedItem);
            }
        }

        // TODO: PlaylistGetAll – Sort the playlists by playlist name.
        public IEnumerable<PlaylistBaseViewModel> PlaylistGetAll()
        {
            var allPlaylist = ds.Playlists
                              .Include("Tracks")                           
                              .OrderBy(p => p.Name);
            return mapper.Map<IEnumerable<Playlist>, IEnumerable<PlaylistBaseViewModel>>(allPlaylist);
        }

        // TODO: PlaylistGetById
        public PlaylistBaseViewModel PlaylistGetById(int theId)
        {
            var playlist = ds.Playlists
                          .Include("Tracks")
                          .SingleOrDefault(t => t.PlaylistId == theId);

            if (playlist == null)
            {
                return null; // Return null if no invoice is found
            }
            playlist.Tracks = playlist.Tracks.OrderBy(t => t.Name).ToList();
            return mapper.Map<Playlist, PlaylistBaseViewModel>(playlist);
        }

        // TODO: PlaylistEditTracks
        public PlaylistEditTracksViewModel PlaylistEditTracks(PlaylistEditTracksViewModel newItem)
        {

            // Attempt to fetch the object

            // When editing an object with a to-many collection,
            // and you wish to edit the collection,
            // MUST fetch its associated collection
            var playlist = ds.Playlists.Include("Tracks").SingleOrDefault(p => p.PlaylistId == newItem.PlaylistId);
            if (playlist == null)
            {
                // Problem - object was not found, so return
                return null;
            }
            else
            {
                // Update the object with the incoming values

                // First, clear out the existing collection
                playlist.Tracks.Clear();

                // Then, go through the incoming items
                // For each one, add to the fetched object's collection
                foreach (var item in newItem.TrackIds)
                {
                    var a = ds.Tracks.Find(item);
                    playlist.Tracks.Add(a);
                }
                // Save changes
                ds.SaveChanges();

                return mapper.Map<Playlist, PlaylistEditTracksViewModel>(playlist);
            }
        }
    }
}