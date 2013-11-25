using LayeredApp.DAL.Repository;
using Base.General;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace LayeredApp.zTryoutConsole
{
	class TestDAL
	{
		static void Main( string[] args )
		{
			var listColumnProperties = new List<string> { "bb", "cc" };
			var listColumnSearchText = new List<string> { "text1" };

			var list = new List<AA> { new AA { bb = "text1" }, new AA { cc = "text2" } };
			//list.OrderBy(x => x.aa).ThenBy(x => x.bb);
			//Expression<Func<AA, string>> expr = x => x.aa;
			//list.Select( x => new { x.aa, x.bb } );
			
			PropertyInfo pi = typeof(AA).GetProperty( "aa" );

			//Where
			//list.Where()

			
			

			//Orderby
			var lambda = Reflection.GetPropertyAccessLambdaExpression<AA>( pi );
			//var qq = Expression.Lambda( lambda ).Compile();
			//list.OrderBy( qq );
			//list.OrderBy(x => x.aa).ThenBy(x => x.bb);

			MethodInfo OrderBy = typeof( Enumerable ).GetMethods( ).Single( x => x.Name == "OrderBy" && x.GetParameters( ).Count( ) == 2 ); //TODO use GetMethod to find it without the integer;



			//Select


			var x2 = Reflection.InvokeGenericExtensionMethod<IOrderedEnumerable<AA>>( list, OrderBy, new[] { typeof( AA ), pi.PropertyType }, new object[] { lambda.Compile( ) } );
			var xx2 = x2.ToList( );

		}



	}

	class AA {
		public string aa { get;set; }
		public string bb { get; set; }
		public string cc { get; set; }
	}

	public static class extensions
	{
		public static IQueryable<T> OrderBy<T>( this IQueryable<T> source, string ordering, params object[] values )
		{
			var type = typeof( T );
			var property = type.GetProperty( ordering );
			var parameter = Expression.Parameter( type, "p" );
			var propertyAccess = Expression.MakeMemberAccess( parameter, property );
			var orderByExp = Expression.Lambda( propertyAccess, parameter );

			MethodCallExpression resultExp = Expression.Call( 
				typeof( Queryable ), 
				"OrderBy", 
				new Type[] { type, property.PropertyType },
				source.Expression, 
				Expression.Quote( orderByExp ) );

			return source.Provider.CreateQuery<T>( resultExp );
		}

		//public static IQueryable OrderBy( this IQueryable query, string ordering, params object[] values )
		//{
		//    Expression queryExpr = query.Expression;

		//    IEnumerable<DynamicOrdering> orderings = parser.ParseOrdering( );
		//    var parameters = new ParameterExpression[] { Expression.Parameter(query.ElementType) };
		//    string methodAsc = "OrderBy";
		//    string methodDesc = "OrderByDescending";

		//    foreach (DynamicOrdering o in orderings)
		//    {
		//        queryExpr = Expression.Call(
		//            typeof( Queryable ),
		//            o.Ascending ? methodAsc : methodDesc,
		//            new Type[] { query.ElementType, o.Selector.Type },
		//            queryExpr,
		//            Expression.Quote( Expression.Lambda( o.Selector, parameters ) )
		//        );
		//        methodAsc = "ThenBy";
		//        methodDesc = "ThenByDescending";
		//    }
		//    return query.Provider.CreateQuery( queryExpr );
		//}
	}
}
