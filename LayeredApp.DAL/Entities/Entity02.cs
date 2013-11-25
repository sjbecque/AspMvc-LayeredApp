using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System;

namespace LayeredApp.DAL
{
	public class Entity02
	{
		public int Id { get; set; }
		public int Entity01Id { get; set; }
		public Entity01 Entity01 { get; set; } //Relatie via conventie;
		public DateTime LastModified { get; set; }
	}
}

namespace LayeredApp.DAL.Mappings
{
	public class Entity02Map : EntityTypeConfiguration<Entity02>
	{
		public Entity02Map( )
		{
			//Example:this.Property( x => x.Id ).HasColumnName( "someColumnName" );

			//Example:Foreign-key relations here, if conventions don't suffice;


			//probeersel
			//var xx = ConfigurationManager.ConnectionStrings;
			//var ee = WebConfigurationManager.AppSettings["key"];
		}
	}
}