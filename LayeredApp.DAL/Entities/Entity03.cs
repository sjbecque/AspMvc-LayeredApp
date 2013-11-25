using System.Configuration;
using System.Data.Entity.ModelConfiguration;

namespace LayeredApp.DAL
{
    public class Entity03
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace LayeredApp.DAL.Mappings
{
	public class Entity03Map : EntityTypeConfiguration<Entity03>
	{
		public Entity03Map( )
		{
			//Example:this.Property( x => x.Id ).HasColumnName( "someColumnName" );

			//Example:Foreign-key relations here, if conventions don't suffice;
		}
	}
}