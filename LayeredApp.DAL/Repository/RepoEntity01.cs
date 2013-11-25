using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LayeredApp.DAL.Repository;
using Base.General;

namespace LayeredApp.DAL.Repository
{
	internal class RepoEntity01 : RepoBase<Entity01> 
	{
		public RepoEntity01( IUnitOfWork db ) : base(db) { }

		internal IQueryable<Entity01> GetActive( )
		{
			return DBSet( ).Where( x => !x.Deleted );
		}
		internal IQueryable<Entity01> GetDeleted( )
		{
			return DBSet( ).Where( x => x.Deleted );
		}

		internal void Update( Entity01 entity )
		{
			entity.LastModified = DateTime.Now; //Demo: tweak een property;
			UpdateEntity( entity );
		}
	}
}