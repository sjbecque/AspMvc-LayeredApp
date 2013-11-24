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
	public interface IUnitOfWork : IDisposable
	{
		int SaveChanges( );
		void Dispose( );
	}
	public interface IRepository : IDisposable
	{
		int SaveChanges( );
		void Dispose( );
	}
}
