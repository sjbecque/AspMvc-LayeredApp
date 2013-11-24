using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LayeredApp.DAL.Repository;

namespace LayeredApp.DAL.Repository
{
	public class UOW //Unit Of Work Provider abbreviation
	{
		public static IUnitOfWork Create()
		{
			return new DALContext( );
		}
	}
}
