using AA2237A3.Data;
using AA2237A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AA2237A3.Controllers
{
    public class TrackController : Controller
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: Track
        public ActionResult Index()
        {
            return View(m.TrackGetAllWithDetail());
        }

        // GET: Track/Details/5
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var t = m.TrackGetByIdWithDetail(id.GetValueOrDefault());

            if (t == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(t);
            }
        }

        // GET: Track/Create
        public ActionResult Create()
        {
            // Create a form
            var form = new TrackAddFormViewModel();

            var albums = m.AlbumGetAll();                          
            var preSelectedAlbum = albums.FirstOrDefault();

            var mediaTypes = m.MediaTypeGetAll();
            var preSelectedMediaType = mediaTypes.ElementAt(1).MediaTypeId;

            form.AlbumId = preSelectedAlbum.AlbumId;

            // Configure the SelectList for the item-selection element on the HTML Form
            form.AlbumList = new SelectList(m.AlbumGetAll(), "AlbumId", "Title", selectedValue: preSelectedAlbum.AlbumId);

            // Configure the SelectList for the item-selection element on the HTML Form
            form.MediaTypeList = new SelectList(m.MediaTypeGetAll(), "MediaTypeId", "Name", selectedValue: preSelectedMediaType);
         
            return View(form);
        }

        // POST: Track/Create
        [HttpPost]
        public ActionResult Create(TrackAddViewModel newItem)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.TrackAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.TrackId });
            }
        }
    }
}
