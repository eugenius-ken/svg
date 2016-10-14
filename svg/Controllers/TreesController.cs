using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace svg.Controllers
{
    public class TreesController : Controller
    {
        private SvgManager _manager = new SvgManager();

        public ActionResult Index()
        {
            var trees = _manager.GetTrees();

            return View();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_manager != null)
                {
                    _manager.Dispose();
                    _manager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}