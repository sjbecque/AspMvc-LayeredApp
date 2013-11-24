using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LayeredApp.DAL.Repository;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Base.General;
using System.IO;
using LayeredApp.DAL.Services;
using System.Web.Providers.Entities;
using System.Reflection;
using System.Web.Hosting;
using LayeredApp.WebUI.General.T4;

namespace LayeredApp.WebApp.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult DemoTypeSafeCalls( )
		{
			return View( v.Home.DemoTypeSafeCalls );
		}
		public ActionResult DemoNavigation()
		{			
			return View();
		}


		public ActionResult DemoTable( )
		{
			return View( );
		}
		public ActionResult GetEntity01Table( PaginationIN p_in )
		{
			var p_out = ServiceEntity01.getEntity01Table( p_in );
			return Json( p_out );
		}

		public ActionResult DemoPopup( )
		{
			return View();
		}

		#region Demo ViewModel-baseclass

		#region Optie 1
		//Optie 1 geeft je een type-safe manier om een viewmodel-base te gebruiken;

		public ActionResult DemoViewmodelBaseclass_optie1( )
		{
			var vm = getVM<DemoViewModel>( );
			vm.SomeSpecificPropertyForController = "...";
			return View();
		}

		private T getVM<T>( ) where T : DemoViewModelBase
		{
			//Eventueel verplaatsen naar base-controller;

			var t = Activator.CreateInstance<T>( );
			t.SomeSharedProperty = "SomeImportantSharedValue";
			return t;
		}
		#endregion

		#region Demo Viewmodel-baseclass; optie 2
		//Optie 2 is iets te impliciet, niet type-safe, minder geschikt voor tests, doch iets minder verbose;
		//ook is een nadeel dat de properties in de viewmodel-base niet in de action te gebruiken zijn, aangezien ze pas daarna worden ingeladen;
		//deze optie is af te raden, maar demonstreert ook wat gerelateerde Asp-MVC plumbing;

		public ActionResult DemoViewmodelBaseclass_optie2( )
		{
			var vm = new DemoViewModel 
			{ 
				SomeSpecificPropertyForController = "..."
			};
			return View( );
		}
		protected override ViewResult View( IView view, object model )
		{
			//De View-methodes kunnen uiteraard eventueel naar base-Controllers worden verplaatst;
			//Bedenk dat het gebruik van Action- en Controller-filters vaak te prefereren is boven het gebruik van base-Controllers;

			if (model != null)
			{ 
				( (DemoViewModelBase)model ).SomeSharedProperty = "SomeImportantSharedValue";			
			}
			return base.View( view, model );
		}
		protected override ViewResult View( string viewName, string masterName, object model )
		{
			if (model != null)
			{
				( (DemoViewModelBase)model ).SomeSharedProperty = "SomeImportantSharedValue";
			}
			return base.View( viewName, masterName, model );
		}
		#endregion
		
		public class DemoViewModel : DemoViewModelBase
		{
			public string SomeSpecificPropertyForController { get; set; }	
		}
		public abstract class DemoViewModelBase
		{
			public string SomeSharedProperty { get; set; }
		}	

		#endregion
	}
}
