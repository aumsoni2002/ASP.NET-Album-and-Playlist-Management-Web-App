using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AA2237A3.Controllers
{
    public class MediaTypeController : Controller
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: MediaType
        public ActionResult Index()
        {
            return View(m.MediaTypeGetAll());
        }
    }
}
