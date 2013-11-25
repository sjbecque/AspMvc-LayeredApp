using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Linq.Expressions;
using Expr = System.Linq.Expressions.Expression;
using Base.DLinq;
using System.Reflection;

namespace Base.General
{
	//Dit is een Pagination approach via Expression Trees;
	//Wellicht is dit te complex vergeleken met een pure-SQL oplossing of
	//"creeer en gebruik Stored Procedure"-oplossing;
	//Eventueel kan deze code worden vereenvoudigt door gebruik te maken van
	//SQL multiple result sets (zodat de TotalItemCount separaat kan worden geretouneerd
	//vanuit de query i.p.v. in een extra kolom) en een efficientere manier om de
	//Select uit te voeren. Desgewenst zou ook TotalWithoutFilters in een kolom worden kunnen worden opgevraagd.
	//Denk ook aan compiled queries

	public class Pagination
	{
		private static string PropertyNameTotal = "Total";

		public static PaginationOUT GetPageData<T>( IQueryable<T> InitialQuery, PaginationIN p )
		{
			int skip = p.MaxItemsOnPage * ( p.PageNumber - 1 );
			int take = p.MaxItemsOnPage;

			var query = InitialQuery;
			query = Pagination.GetWhere( InitialQuery, p.TableColumns );
			query = Pagination.GetOrderedQuery( query, p.TableColumns );
			var queryPaginated = query.Skip( skip ).Take( take );
			var iQueryable = Pagination.GetSelect( queryPaginated, p.TableColumns, query );
						
			var result = getPaginationOUT( iQueryable, p );

			return result;
		}
		private static PaginationOUT getPaginationOUT( IQueryable iQueryable, PaginationIN p_in )
		{
			//Convert the resulting records to an List<string[]>;

			var result = new PaginationOUT { MaxItemsOnPage = p_in.MaxItemsOnPage, PageNumber = p_in.PageNumber };
			var columnProperties = iQueryable.ElementType.GetProperties( ).Where( x => x.Name != PropertyNameTotal ).ToList( );
			result.Items = new List<string[]>( );

			foreach (var row in iQueryable)
			{
				if (!result.ItemTotalOnAllPages.HasValue)
				{
					result.ItemTotalOnAllPages = (int)iQueryable.ElementType.GetProperty( PropertyNameTotal ).GetValue( row, null );
				}

				var rowStr = new string[columnProperties.Count( )];
				for (int i = 0; i < columnProperties.Count( ); i++)
				{
					//Misschien nog conversie?
					var ColumnValue = columnProperties[i].GetValue( row, null );
					var str = ColumnValue != null ? ColumnValue.ToString( ) : string.Empty;
					rowStr[i] = str;
				}
				result.Items.Add( rowStr );
			}
			return result;
		}

		public static IOrderedQueryable<T> GetOrderedQuery<T>( IQueryable<T> Query, List<TableColumn> Columns )
		{
			var QueryExpr = Query.Expression;
			var type = typeof( T );
			var typeT = Expression.Parameter( type ); // T
			var MethodAsc = "OrderBy";
			var MethodDesc = "OrderByDescending";

			foreach (var column in Columns)
			{
				//Todo replace by Reflection.method
				var property = type.GetProperty( column.PropertyName ); // .SomeProperty
				var propertyAccess = Expression.MakeMemberAccess( typeT, property ); // T.SomeProperty
				var lambda = Expression.Lambda( propertyAccess, typeT ); // T => T.SomeProperty

				//This results in Queryable.MethodName( T => T.SomeProperty );
				QueryExpr = Expression.Call(
					typeof( Queryable ), //Static class containing extension method;
					( column.SortAscending ? MethodAsc : MethodDesc ),
					new Type[] { type, property.PropertyType }, //Generic parameters;
					QueryExpr, //Expression for the Query, the "this" argument in extension method;
					Expression.Quote( lambda ) ); //Putting in the argument for the lambda-expression as Expression;

				MethodAsc = "ThenBy";
				MethodDesc = "ThenByDescending";
			}

			return (IOrderedQueryable<T>)Query.Provider.CreateQuery( QueryExpr ); ;
		}

		public static IQueryable<T> GetWhere<T>( IQueryable<T> Query, List<TableColumn> Columns )
		{
			var QueryExpr = Query.Expression;

			foreach (var column in Columns.Where(x => !string.IsNullOrEmpty( x.FilterText ) ))
			{
				var Argument_This = QueryExpr;
				var Argument_LambdaWhere = GetWhereLambdaExpression<T>( column.PropertyName, column.FilterText );

				QueryExpr = Expression.Call(
					typeof( Queryable ), "Where", new Type[] { typeof( T ) }, //= Queryable.Where<T>
					new[] { Argument_This, Argument_LambdaWhere } );
			}
			
			return (IQueryable<T>)Query.Provider.CreateQuery( QueryExpr );
		}
		private static LambdaExpression GetWhereLambdaExpression<T>( string PropertyName, string SearchText )
		{
			//Result: (T x) => (TProperty_NonNullablePart)x.SomeProperty == (TProperty_NonNullablePart)"SomeText"

			var pi = typeof( T ).GetProperty( PropertyName );
			var typeT = Expr.Parameter( typeof( T ) );

			var left_AccessProperty = Expr.Property( typeT, pi ); //This determines the return type of the lambda (pi.PropertyType);

			var CompareType = Nullable.GetUnderlyingType( pi.PropertyType ) ?? pi.PropertyType;
			var SearchTextObj = Convert.ChangeType( SearchText, CompareType );
			var right_Constant = Expr.Constant( SearchTextObj );

			var body = Expr.Equal( Expr.Convert( left_AccessProperty, CompareType ), right_Constant );

			LambdaExpression result = Expr.Lambda( body, typeT );
			return result;
		}

		public static IQueryable GetSelect<T>( IQueryable<T> Query, List<TableColumn> columns, IQueryable<T> QueryNoFilters )
		{	
			//This ParameterExpression to be used in multiple place, otherwise you get error:
			//"The parameter 'TEntity' was not bound in the specified LINQ to Entities query expression."
			var paramT = Expr.Parameter( typeof( T ), "TEntity" );

			var AnonymousExpr = GetAnonymous( columns, QueryNoFilters, paramT );
			var LambdaSelect = Expr.Lambda( AnonymousExpr, new ParameterExpression[] { paramT } );

			//Call select on Query;
			var QueryExpr = Expr.Call(
				typeof( Queryable ), "Select", new Type[] { Query.ElementType, LambdaSelect.Body.Type }, //= Queryable.Where<T>
				Query.Expression, Expr.Quote( LambdaSelect ) ); //= arguments

			var result = Query.Provider.CreateQuery( QueryExpr );
			return result;
		}
		private static MemberInitExpression GetAnonymous<T>( List<TableColumn> columns, IQueryable<T> QueryNoFilters, ParameterExpression paramT )
		{
			//Get dynamic anonymous type with Dynamic Linq's Classfactory;
			var DynamicProperties = columns.Select( x => new DynamicProperty(
				x.PropertyName,
				typeof( T ).GetProperty( x.PropertyName ).PropertyType )
			).ToList( );

			//Add a column that will contain Total;
			DynamicProperties.Add( new DynamicProperty( PropertyNameTotal, typeof( int ) ) );

			var AnonymousType = ClassFactory.Instance.GetDynamicClass( DynamicProperties );

			List<MemberAssignment> mb = DynamicProperties.Where( x => x.Name != PropertyNameTotal ).Select( x =>
				Expr.Bind(
					AnonymousType.GetProperty( x.Name ),
					Expr.Property( paramT, typeof( T ).GetProperty( x.Name ) )
				)
			).ToList( );

			//For Total column: Total = QueryNoFilters.Count()
			mb.Add(
				Expr.Bind(
					AnonymousType.GetProperty( PropertyNameTotal ),
					Expr.Call( typeof( Queryable ), "Count", new Type[] { QueryNoFilters.ElementType }, QueryNoFilters.Expression )
				)
			);

			//Create: new { prop1 = x.prop1, prop2 = x.prop2, Total = QueryNoFilters.Count() }
			var AnonymousExpr = Expr.MemberInit( Expr.New( AnonymousType ), mb );
			return AnonymousExpr;
		}
	}



	public class TableColumn
	{
		public string PropertyName { get; set; }
		public bool SortAscending { get; set; }
		public string FilterText { get; set; }
		public string FilterBoundaryLeft { get; set; }
		public string FilterBoundaryRight { get; set; }

		//voor sql nog
		public string getWhere()
		{
			if (!string.IsNullOrEmpty( FilterText ))
			{
				return PropertyName + "=" + FilterText;
			}
			else if (!string.IsNullOrEmpty( FilterBoundaryLeft ) && !string.IsNullOrEmpty( FilterBoundaryRight ))
			{
				return null;
			}
			else return null;		
		}
	}



	public class PaginationBase
	{
		public int MaxItemsOnPage { get; set; }
		public int PageNumber { get; set; }
	}

	public class PaginationIN : PaginationBase
	{
		public List<TableColumn> TableColumns { get; set; }
	}
	public class PaginationOUT : PaginationBase
	{
		public List<string[]> Items { get; set; }
		//public int? ItemCountNoFilters { get; set; }
		public int? ItemTotalOnAllPages { get; set; }
		//public int PageCountWithFilters
		//{
		//    get
		//    {
		//        if (!ItemTotalOnAllPages.HasValue) return 0;
		//        return ( ItemTotalOnAllPages.Value / MaxItemsOnPage ) + 1;
		//    }
		//}
	}
}
