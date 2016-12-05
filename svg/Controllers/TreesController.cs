using svg.Models;
using svg.ViewModels;
using Svg;
using svgProject.AjaxModels;
using svgProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace svg.Controllers
{
    public class TreesController : Controller
    {
        private SvgManager _manager = new SvgManager();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        public ActionResult Index(string q)
        {
            var trees = _manager.GetTrees(q);
            var model = new TreesListView()
            {
                Trees = trees.Select(t => new TreeThumbnailView()
                {
                    Id = t.Id,
                    Name = t.Name,
                }),
                Keyword = q
            };
            return View(model);
        }

        [Authorize(Roles = "Administrator, User")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult Add(TreeView model)
        {


            if (ModelState.IsValid)
            {
                var newTree = new Tree()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };
                _manager.AddTree(newTree);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Review(Guid id)
        {
            var parent = _manager.GetParentElement(id);

            return View(new ReviewTreeView()
            {
                TreeId = parent.TreeId,
                ImageText = parent.Value,
                Name = parent.Tree.Name,
                ParentId = parent.Id
            });
        }

        //need for method GetThumbnailImage
        private bool ThumbnailCallBack()
        {
            return false;
        }

        [Authorize(Roles = "Administrator, User")]
        public ActionResult Edit(Guid id)
        {
            var tree = _manager.GetTreeById(id);
            var mainElement = _manager.GetParentElement(id);

            var model = new TreeEditView()
            {
                Id = tree.Id,
                Name = tree.Name,
                
                Value = mainElement == null ? String.Empty : mainElement.Value,
                CurrentId = mainElement == null ? String.Empty : mainElement.Id.ToString(),
                SvgObjects = _manager.GetSvgObjects(String.Empty).Select(o => new SvgObjectView()
                {
                    Id = o.Id,
                    Name = o.Name
                })
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator, User")]
        public ActionResult Delete(Guid id)
        {
            var deleted = _manager.DeleteTree(id);
            return deleted ? Content(id.ToString()) : Content(String.Empty);
        }

        public ActionResult Thumbnail(Guid id)
        {
            var thumbnail = _manager.GetThumbnailById(id);
            if (thumbnail == null)
                return File("/Content/no-image.png", "image/png");

            return File(thumbnail.Content, "image/jpg");
        }

        public ActionResult GetXMLTextForImage(Guid id)
        {
            var image = _manager.GetSvgObjectById(id);

            return image == null ? Content(String.Empty) : Content(image.Value);
        }

        public ActionResult GetXmlTextForElement(Guid id)
        {
            var element = _manager.GetTreeElementById(id);

            return Json(new { id = element.Id, imageText = element.Value}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateParentElement(AjaxData obj)
        {
            TreeElement parentElement = new TreeElement()
            {
                Id = Guid.NewGuid(),
                TreeId = obj.treeId,
                IsParent = true,
                Value = obj.imageText
            };
            _manager.AddTreeElement(parentElement);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(obj.imageText);

            var svgDocument = SvgDocument.Open(xmlDoc);
            var bitmap = svgDocument.Draw();

            Image.GetThumbnailImageAbort im = new Image.GetThumbnailImageAbort(ThumbnailCallBack);
            var thumb = bitmap.GetThumbnailImage(250, 200, im, IntPtr.Zero);

            using (MemoryStream stream = new MemoryStream())
            {
                thumb.Save(stream, ImageFormat.Jpeg);
                var thumbnail = new Thumbnail()
                {
                    Id = obj.treeId,
                    Content = stream.ToArray()
                };
                _manager.AddThumbnail(thumbnail);
            }

            return Content(parentElement.Id.ToString());
        }

        [HttpPost]
        public ActionResult SaveChildImage(AjaxData obj)
        {
            SvgObject objFromLibrary = _manager.GetSvgObjectById(obj.imageId);

            TreeElement child = new TreeElement()
            {
                Id = Guid.NewGuid(),
                IsParent = false,
                TreeId = obj.treeId,
                Value = objFromLibrary.Value
            };
            _manager.AddTreeElement(child);

            return Content(child.Id.ToString());
        }

        [HttpPost]
        public ActionResult ChangeCurrentImage(AjaxData obj)
        {
            _manager.UpdateImageTextForElement(obj.imageId, obj.imageText);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult GetImageText(Guid id)
        {
            TreeElement element = _manager.GetTreeElementById(id);
            return Content(element.Value);
        }


        public ActionResult UnbindImage(Guid imageId)
        {
            _manager.DeleteTreeElement(imageId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //public ActionResult SaveThumbnailForTree(Guid id)
        //{

        //}

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