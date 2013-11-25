using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LayeredApp.DAL.Repository;
using System.Data.Entity;

namespace LayeredApp.DAL.Repository
{
	internal abstract class RepoBase<T> : IRepository where T : class
	{	
		private DALContext db { get; set; }
		public IUnitOfWork UnitOfWork
		{
			get
			{
				return this.db;
			}
		}
		public RepoBase( IUnitOfWork db )
		{
			this.db = (DALContext)db;
		}


		protected DbSet<T> DBSet( )
		{
			return db.Set<T>( );
		}
		protected void UpdateEntity( T entity )
		{
			db.Set<T>( ).Attach( entity );
			db.Entry<T>( entity ).State = System.Data.EntityState.Modified;
		}
		protected void DeleteEntity( T entity )
		{
			db.Set<T>( ).Attach( entity );
			db.Entry<T>( entity ).State = System.Data.EntityState.Deleted;
		}


		public int SaveChanges( )
		{
			return db.SaveChanges( );
		}
		public void Dispose( )
		{
			db.Dispose( );
		}


		#region Debugging
		public void CheckDatabase( )
		{
			string ConnectionStringMessage = this.db.Database.Connection.ConnectionString;
			bool DbExists = this.db.Database.Exists( );
			bool DbIsCompatible = !this.db.Database.CompatibleWithModel( false );
		}
		public void InspectChangeTracker( )
		{
			var d1 = db.ChangeTracker.Entries<Entity01>( ).ToList( );
		}
		#endregion
	}
}
