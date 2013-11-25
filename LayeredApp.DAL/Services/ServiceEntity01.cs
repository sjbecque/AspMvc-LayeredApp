using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LayeredApp.DAL.Repository;
using Base.General;

namespace LayeredApp.DAL.Services
{
	public class ServiceEntity01
	{
		public static List<Entity01> DemoGetEntity01sById( int id ) //static????
		{
			using (var repo = new RepoEntity01( UOW.Create( ) ))
			{
				return repo.GetActive( ).Where( x => x.Id == id ).ToList( );
			}
		}

		public static List<Entity01> DemoGetEntity01sFromToday( ) //static????
		{
			using (var repo = new RepoEntity01( UOW.Create( ) ))
			{
				return repo.GetActive().Where(x => x.date1 == DateTime.Now).ToList();
			}
		}

		public static List<Entity01> DemoGetDeletedEntity01s( ) //static????
		{
			using (var repo = new RepoEntity01( UOW.Create( ) ))
			{
				return repo.GetDeleted().ToList( );
			}
		}

		public static PaginationOUT getEntity01Table( PaginationIN p_in )
		{
			using (var repo = new RepoEntity01( UOW.Create( ) ))
			{
				var InitialQuery = repo.GetActive().Where( x => x.name != "bb" );
				var result = Pagination.GetPageData( InitialQuery, p_in );
				return result;	
			}					
		}	
	}
}
