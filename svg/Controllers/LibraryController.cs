using svg.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace svg.Controllers
{
    public class LibraryController : Controller
    {
        private SvgManager _manager = new SvgManager();

        public ActionResult Index()
        {
            var objects = _manager.GetSvgObjects();
            var model = new SvgObjectsListView()
            {
                Objects = objects.Select(o => new SvgObjectVew()
                {
                    Id = o.Id,
                    Name = o.Name
                })
            };

            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult Thumbnail()
        {
            return View();
        }

        public ActionResult FullImage()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(_manager != null)
                {
                    _manager.Dispose();
                    _manager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}