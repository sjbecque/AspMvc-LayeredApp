using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System;

namespace LayeredApp.DAL
{
	public class Entity01
	{
		public int Id { get; set; }
		public string name { get; set; }
		public string name2 { get; set; }
		public int nr1 { get; set; }
		public DateTime? date1 { get; set; }
		public bool Deleted { get; set; }
		public DateTime LastModified { get; set; }
	}
}

namespace LayeredApp.DAL.Mappings
{
	public class Entity01Map : EntityTypeConfiguration<Entity01>
	{
		public Entity01Map( )
		{
			//Example:this.Property( x => x.Id ).HasColumnName( "someColumnName" );

			//Example:Foreign-key relations here;


			//probeersel
			//var xx = ConfigurationManager.ConnectionStrings;
			//var ee = WebConfigurationManager.AppSettings["key"];
		}
	}
}