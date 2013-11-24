using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LayeredApp.DAL.Repository;

namespace LayeredApp.DAL.Services
{
	public class SomeServiceDemo
	{
		public static void DemoMultipleRepos( int id )
		{
			//Demo: gebruik van meerdere Repo's binnen één transactie;

			using (var repo01 = new RepoEntity01( UOW.Create( ) ))
			{
				var list01 = repo01.GetActive( ).ToList( );

				var repo02 = new RepoEntity02( repo01.UnitOfWork );
				
				var entity01 = new Entity02( );
				repo02.Update( entity01 );			
			}
		}
	}
}
