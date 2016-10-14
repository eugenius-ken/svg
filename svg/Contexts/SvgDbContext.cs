using svg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace svg.Contexts
{
    public class SvgDbContext : DbContext
    {
        public SvgDbContext() : base("DefaultConnection") { }

        public DbSet<SvgObject> SvgObjects { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Tree> Trees { get; set; }
        public DbSet<TreeElement> TreeElements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thumbnail>()
                .HasRequired(obj => obj.SvgObject)
                .WithRequiredDependent()
                .WillCascadeOnDelete(true);
        }
    }

    public class SvgDbInitializer : DropCreateDatabaseIfModelChanges<SvgDbContext>
    {
        protected override void Seed(SvgDbContext context)
        {

        }
    }
}