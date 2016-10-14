using svg.Models;
using svg.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Svg;
using System.Xml;
using System.Drawing.Imaging;
using System.Drawing;
using svgProject.ViewModels;

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
                Objects = objects.Select(o => new SvgObjectView()
                {
                    Id = o.Id,
                    Name = o.Name
                })
            };

            return View(model);
        }

        public ActionResult Add()
        {
            return View(new SvgObjectView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SvgObjectView model, HttpPostedFileBase image)
        {
            if (image == null)
                ModelState.AddModelError("image", "Please, choose some image");

            if(ModelState.IsValid)
            {
                var xmlText = new StreamReader(image.InputStream).ReadToEnd();
                var newObject = new SvgObject()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Value = xmlText
                };
                _manager.AddSvgObject(newObject);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlText);

                var svgDocument = SvgDocument.Open(xmlDoc);
                var bitmap = svgDocument.Draw();

                Image.GetThumbnailImageAbort im = new Image.GetThumbnailImageAbort(ThumbnailCallBack);
                var thumb = bitmap.GetThumbnailImage(250, 200, im, IntPtr.Zero);
                
                using (MemoryStream stream = new MemoryStream())
                {
                    thumb.Save(stream, ImageFormat.Jpeg);
                    var thumbnail = new Thumbnail()
                    {
                        Id = newObject.Id,
                        Content = stream.ToArray()
                    };
                    _manager.AddThumbnail(thumbnail);
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //need for method GetThumbnailImage
        private bool ThumbnailCallBack()
        {
            return false;
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var deleted = _manager.DeleteSvgObject(id);
            return deleted ? Content(id.ToString()) : Content(String.Empty);
        }

        public ActionResult Thumbnail(Guid id)
        {
            var thumbnail = _manager.GetThumbnailById(id);
            if (thumbnail == null)
                return View();

            return File(thumbnail.Content, "image/jpg");
        }

        public ActionResult FullImage(Guid id)
        {
            var obj = _manager.GetSvgObjectById(id);
            if (obj == null)
                return HttpNotFound();

            return View(new SvgObjectFullView(obj.Id, obj.Name));
        }

        public ActionResult GetSvgImage(Guid id)
        {
            var obj = _manager.GetSvgObjectById(id);
            if (obj == null)
                return View();

            return Content(obj.Value, "image/svg+xml");
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