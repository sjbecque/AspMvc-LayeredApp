using LayeredApp.DAL.Repository;
using Base.General;
using System.Collections.Generic;
using LayeredApp.DAL.Services;
using LayeredApp.DAL;

namespace LayeredApp.zTryoutConsole
{
	class Program
	{
		static void Main( string[] args )
		{					
			//AdhocTestDAL01( );
			//RestoreDb();
		}

		static void AdhocTestDAL01( )
		{
			var l = ServiceEntity01.DemoGetEntity01sById( 1 );
		}

		static void RestoreDb( )
		{
			#if DEBUG
			ManageDb.RestoreDb( );
			#endif
		}

		static void testTablePagination( )
		{
			var p_in = new PaginationIN
			{
				MaxItemsOnPage = 1,
				PageNumber = 1,
				TableColumns = new List<TableColumn>
				{
				    new TableColumn{PropertyName = "name", SortAscending = false, FilterText = "cc"}
					,new TableColumn{PropertyName = "date1", SortAscending = false, FilterText = ""/*"2013-01-01"*/ }
				}
			};

			var result = ServiceEntity01.getEntity01Table( p_in );		
		} 
	}


}
