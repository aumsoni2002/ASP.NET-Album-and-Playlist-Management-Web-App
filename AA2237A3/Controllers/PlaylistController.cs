using AA2237A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AA2237A3.Controllers
{
    public class PlaylistController : Controller
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: Playlist
        public ActionResult Index()
        {
            return View(m.PlaylistGetAll());
        }

        // GET: Playlist/Details/5
        public ActionResult Details(int? id)
        {
            var p = m.PlaylistGetById(id.GetValueOrDefault());

            if (p == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(p);
            }
        }

        
        // GET: Playlist/Edit/5
        public ActionResult Edit(int? id)
        {
            // Attempt to fetch the matching object
            var p = m.PlaylistGetById(id.GetValueOrDefault());

            if (p == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Create a form, based on the fetched matching object
                var form = m.mapper.Map<PlaylistEditTracksFormViewModel>(p);

                // For the multi select list, configure the "selected" items
                // Notice the use of the Select() method, 
                // which allows us to select/return/use only some properties from the source
                var selectedValues = p.Tracks.Select(e => e.TrackId);

                // For clarity, use the named parameter feature of C#
                form.TrackList = new MultiSelectList
                    (items: m.TrackGetAllWithDetail(),
                    dataValueField: "TrackId",
                    dataTextField: "NameFull",
                    selectedValues: selectedValues);

                return View(form);
            }
        }

        // POST: Playlist/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, PlaylistEditTracksViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("edit", new { id = newItem.PlaylistId });
            }

            if (id.GetValueOrDefault() != newItem.PlaylistId)
            {
                // This appears to be data tampering, so redirect the user away
                return RedirectToAction("Index");
            }

            // Attempt to do the update
            var editedItem = m.PlaylistEditTracks(newItem);

            if (editedItem == null)
            {
                // There was a problem updating the object
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("edit", new { id = newItem.PlaylistId });
            }
            else
            {
                // Show the details view, which will have the updated data
                return RedirectToAction("details", new { id = newItem.PlaylistId });
            }
        }      
    }
}
