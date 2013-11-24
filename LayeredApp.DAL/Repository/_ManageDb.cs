using System;
using LayeredApp.DAL.Repository;
using System.Configuration;

namespace LayeredApp.DAL
{
	public class ManageDb
	{
		public static void RestoreDb( )
		{
			using (var db = new DALContext( ))
			{
				DataTruncate( );
				AddData01( db );
				db.SaveChanges( );
			}
		}

		public static void DataTruncate( )
		{
			//TODO
			//Hier code om data te verwijderen;
		}

		private static void AddData01( DALContext db )
		{
			//Uitleg; static method zodat Seed() dit kan aanroepen;
			
			var entity01 = Add( db, new Entity01 { name = "a1" } );
			Add( db, new Entity01 { name = "a2" } );
			Add( db, new Entity01 { name = "a3" } );
			Add( db, new Entity01 { name = "a4" } );

			//Add( db, new EntityXX { Entity01 = entity01 } );
		}
		private static T Add<T>( DALContext db, T entity ) where T : class
		{
			db.Set<T>( ).Add( entity );
			return entity;
		}
	}
}
