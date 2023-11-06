using AA2237A3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AA2237A3.Models
{
    public class PlaylistBaseViewModel
    {
        public PlaylistBaseViewModel() 
        {
            Tracks = new List<TrackWithDetailViewModel>();
        }

        [Key]
        public int PlaylistId { get; set; }

        [StringLength(120)]
        [Display(Name = "Playlist Name")]
        public string Name { get; set; }

        public IEnumerable<TrackWithDetailViewModel> Tracks { get; set; }

        [Display(Name = "Playlist Track Count")]
        public int TracksCount { get; set; }
    }

    public class PlaylistEditTracksFormViewModel
    {
        public PlaylistEditTracksFormViewModel()
        {
            Tracks = new List<TrackBaseViewModel>();
        }

        [Key]
        public int PlaylistId { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        [Display(Name = "Track(s)")]
        public MultiSelectList TrackList { get; set; }

        public IEnumerable<TrackBaseViewModel> Tracks { get; set; }
    }

    public class PlaylistEditTracksViewModel
    {
        public PlaylistEditTracksViewModel()
        {
            TrackIds = new List<int>();
        }

        [Key]
        public int PlaylistId { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        public IEnumerable<int> TrackIds { get; set; }
    }
}