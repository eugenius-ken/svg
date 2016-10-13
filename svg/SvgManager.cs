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

        public IEnumerable<SvgObject> GetSvgObjects()
        {
            return _db.SvgObjects.AsEnumerable();
        }


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