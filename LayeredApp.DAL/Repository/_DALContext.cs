using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;
using System.Configuration;
using System.Data.EntityClient;

namespace LayeredApp.DAL.Repository
{
	public class DALContext : DbContext, IUnitOfWork
	{
		public DbSet<Entity01> Entity01s { get; set; }
		public DbSet<Entity03> Entity03s { get; set; }

				
		#region Configuration
		public DALContext( )
		{
			this.Configuration.AutoDetectChangesEnabled = false;
			this.Configuration.LazyLoadingEnabled = false;
			this.Configuration.ProxyCreationEnabled = false;
			this.Configuration.ValidateOnSaveEnabled = false;		
		}

		internal sealed class EFConfiguration : DbMigrationsConfiguration<DALContext>
		{
			public EFConfiguration( )
			{
				this.AutomaticMigrationsEnabled = true;
				this.AutomaticMigrationDataLossAllowed = true;  
			}

			protected override void Seed( DALContext context )
			{
				base.Seed( context );
			}
		}
		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			Database.SetInitializer( new MigrateDatabaseToLatestVersion<DALContext, EFConfiguration>( ) );

			AddMappings( modelBuilder );

			//Note: this method gets called by context.Database.CreateIfNotExists() ), among others;
		}
		private void AddMappings( DbModelBuilder modelBuilder )
		{
			//Note: This method create replacements for "modelBuilder.Configurations.Add( new ...Map( ) );" for all entities;

			try
			{
				var MappingTypes = Assembly
					.GetExecutingAssembly()
					.GetTypes()
					.Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>))
					.ToList();

				var AddMethod = typeof(ConfigurationRegistrar).GetMethods().First(x => x.Name == "Add"); // "First()": get the right Add overload;

				foreach (var MappingType in MappingTypes)
				{
					var genericParam = MappingType.BaseType.GetGenericArguments().Single();
					MethodInfo genericMethod = AddMethod.MakeGenericMethod(new Type[] { genericParam });

					var parameter = Activator.CreateInstance(MappingType);
					genericMethod.Invoke(modelBuilder.Configurations, new object[] { parameter });
				}
			}
			catch (Exception exc)
			{
				//To show the inner exception in the Package Manager Console when calling "update-database";
				throw new Exception(exc.Message + "_" +  exc.InnerException.Message);
			}
		}
		#endregion
	}
}
