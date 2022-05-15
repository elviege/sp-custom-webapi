using System.Data.Entity;
using System.Reflection;
using SP.Portal.Data.Conventions;
using SP.Portal.Core.Entities;

namespace SP.Portal.Data
{
    public partial class MainBaseDbDataContext : DataContextBase
    {
        public MainBaseDbDataContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<MainBaseDbDataContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true; // https://stackoverflow.com/questions/16949520/circular-reference-detected-exception-while-serializing-object-to-json
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Configurations.AddFromAssembly(Assembly.GetAssembly(GetType()));
            builder.Conventions.Add<DataTypePropertyAttributeConvention>();

            /*builder.Properties<string>()
                .Where(p => p.Name == "Name")
                .Configure(c => {
                    c.IsRequired();
                    c.HasMaxLength(ColumnSize.Name);
                });

            builder.Properties<bool>()
                .Where(p => p.Name == "IsDeleted")
                .Configure(c => c.IsRequired());
                */
        }

        public DbSet<TransportRequestEntity> TransportRequests { get; set; }
    }

    public static class ColumnSize
    {
        public const int Code = 50;
        public const int Name = 255;
        public const int LongName = 512;
    }
}
