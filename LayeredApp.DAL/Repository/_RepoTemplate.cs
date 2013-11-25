using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LayeredApp.DAL.Repository;
using Base.General;

namespace LayeredApp.DAL.Repository
{
	//Template class for repository; usage: 
	//-Copy this class;	
	//-Allow the appropriate functionality by including methods, otherwise delete them;
	//-Replace "object" by an Entity-type;

	internal class Repo_SomeEntity : RepoBase<object>
	{
		public Repo_SomeEntity( IUnitOfWork db ) : base( db ) { }

		//internal IQueryable<object> Get( )
		//{
		//    return Get( );
		//}
		//internal void Update( object entity )
		//{
		//    UpdateEntity( entity );
		//}		
		//internal void Delete( object entity )
		//{
		//    DeleteEntity( entity );
		//}
	}
}