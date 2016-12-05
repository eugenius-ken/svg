using svg.Contexts;
using svg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svg
{
    public class SvgManager : IDisposable
    {
        private SvgDbContext _db;

        public SvgManager()
        {
            _db = new SvgDbContext();
        }

        #region SvgObjects
        public IEnumerable<SvgObject> GetSvgObjects(string q)
        {
            var objs = _db.SvgObjects.AsQueryable();
            if (!String.IsNullOrEmpty(q))
                objs = objs.Where(_ => _.Name.Contains(q));

            return objs.AsEnumerable();
        }

        public void AddSvgObject(SvgObject obj)
        {
            _db.SvgObjects.Add(obj);
            _db.SaveChanges();
        }

        public SvgObject GetSvgObjectById(Guid id)
        {
            return _db.SvgObjects.Find(id);
        }

        public bool DeleteSvgObject(Guid id)
        {
            var obj = _db.SvgObjects.Find(id);
            if (obj != null)
            {
                _db.SvgObjects.Remove(obj);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Thumbnail

        public void AddThumbnail(Thumbnail obj)
        {
            _db.Thumbnails.Add(obj);
            _db.SaveChanges();
        }

        public Thumbnail GetThumbnailById(Guid id)
        {
            return _db.Thumbnails.Find(id);
        }

        #endregion

        #region Trees

        public IEnumerable<Tree> GetTrees(string q)
        {
            var trees = _db.Trees.AsQueryable();
            if (!String.IsNullOrEmpty(q))
                trees = trees.Where(_ => _.Name.Contains(q));

            return trees.AsEnumerable();
        }

        public Tree GetTreeById(Guid id)
        {
            return _db.Trees.Find(id);
        }

        public void AddTree(Tree obj)
        {
            _db.Trees.Add(obj);
            _db.SaveChanges();
        }

        public bool DeleteTree(Guid id)
        {
            var obj = _db.Trees.Find(id);
            if (obj != null)
            {
                _db.Trees.Remove(obj);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Elements

        public TreeElement GetParentElement(Guid treeId)
        {
            return _db.TreeElements.FirstOrDefault(e => e.TreeId == treeId && e.IsParent);
        }

        public void AddTreeElement(TreeElement element)
        {
            _db.TreeElements.Add(element);
            _db.SaveChanges();
        }

        public TreeElement GetTreeElementById(Guid id)
        {
            return _db.TreeElements.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateImageTextForElement(Guid id, string imageText)
        {
            var element = _db.TreeElements.FirstOrDefault(e => e.Id == id);
            element.Value = imageText;
            _db.SaveChanges();
        }

        public void DeleteTreeElement(Guid id)
        {
            var element = _db.TreeElements.FirstOrDefault(e => e.Id == id);
            _db.TreeElements.Remove(element);
            _db.SaveChanges();
        }

        #endregion

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}