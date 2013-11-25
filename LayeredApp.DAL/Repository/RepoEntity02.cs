using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LayeredApp.DAL.Repository;
using Base.General;

namespace LayeredApp.DAL.Repository
{
	internal class RepoEntity02 : RepoBase<Entity02> 
	{
		public RepoEntity02( IUnitOfWork db ) : base( db ) { }

		internal void Update( Entity02 entity )
		{
			//Demo: dmv deze method bewust het updaten mogelijk maken;
			base.UpdateEntity( entity );
		}
	}
}