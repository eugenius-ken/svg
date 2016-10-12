using svg.Contexts;
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