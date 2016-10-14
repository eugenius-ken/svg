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
        public IEnumerable<SvgObject> GetSvgObjects()
        {
            return _db.SvgObjects.AsEnumerable();
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

        public IEnumerable<Tree> GetTrees()
        {
            return _db.Trees.AsEnumerable();
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