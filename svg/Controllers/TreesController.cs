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

        public ActionResult Index()
        {
            var trees = _manager.GetTrees();
            var model = new TreesListView()
            {
                Trees = trees.Select(t => new TreeThumbnailView()
                {
                    Id = t.Id,
                    Name = t.Name,
                })
            };
            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        //need for method GetThumbnailImage
        private bool ThumbnailCallBack()
        {
            return false;
        }

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
                SvgObjects = _manager.GetSvgObjects().Select(o => new SvgObjectView()
                {
                    Id = o.Id,
                    Name = o.Name
                })
            };

            return View(model);
        }

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

        [HttpPost]
        public ActionResult CreateParentElement(AjaxData obj)
        {
            //AjaxData obj = (AjaxData)serializer.DeserializeObject(data);

            TreeElement parentElement = new TreeElement()
            {
                Id = Guid.NewGuid(),
                TreeId = obj.treeId,
                IsParent = true,
                Value = obj.imageText
            };
            _manager.AddTreeElement(parentElement);

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