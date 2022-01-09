namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=HelpDesk")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<DetailTech> DetailTeches { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<Trouble> Troubles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.nameCategory)
                .IsUnicode(false);

            modelBuilder.Entity<Device>()
                .Property(e => e.nameDevice)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.question)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.answer)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.pwd)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.name)
                .IsUnicode(false);
        }

    }
}
