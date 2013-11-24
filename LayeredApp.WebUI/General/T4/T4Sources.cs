using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using LayeredApp.WebApp.Controllers;

namespace LayeredApp.WebUI.General.T4
{
	public class T4Sources
	{
		//T4MVC light-weight alternative;

		public static Dictionary<string, List<string>> getViewFolder_to_cshtmls( )
		{
			//Note: Assembly-properties can differ when called from within T4-templates;

			var locationDll_Str = Assembly.GetExecutingAssembly( ).CodeBase.Replace( "file:///", "" );
			var locationDll = Path.GetDirectoryName( locationDll_Str );
			var folderWebUI = new DirectoryInfo( locationDll ).Parent.FullName;
			string[] ViewFolders = Directory.GetDirectories( Path.Combine( folderWebUI, "Views" ) );

			Dictionary<string, List<string>> ViewFolder_to_cshtmls = ViewFolders
				.ToDictionary(
					x => Path.GetFileName( x ),
					x => Directory.GetFiles( x ).Select( y => Path.GetFileNameWithoutExtension( y )).ToList( ) 
			);
			
			return ViewFolder_to_cshtmls;
		}

		public static Dictionary<string, List<string>> getController_to_Actions( )
		{
			List<Type> controllers = Assembly.GetExecutingAssembly( ).GetTypes( ).Where( x => x.BaseType == typeof( BaseController ) ).ToList( );

			Dictionary<string, List<string>> dict = controllers.ToDictionary(
				x => x.Name.Replace( "Controller", "" ),
				
				x => x
					.GetMethods( BindingFlags.Instance | BindingFlags.Public )
					.Where( y => y.ReturnType == typeof( ActionResult ) )
					.Select( z => z.Name )
					.ToList( )
			);

			return dict;
		}
	}
}