using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LayeredApp.DAL.General
{
	public class Pagination
	{
		public static string GetQuery( string entity, int ItemsPerPage, int ActivePageNumber, List<TableColumn> list )
		{
			//todo escape sql injection
			//sqlcommand gebruiken?

			string properties = string.Join( ",", list.Select( x => x.PropertyName.Safe( ) ) );
			string orderBy = "ORDER BY " + string.Join( ",", list.Select( x => x.PropertyName.Safe( ) + ( !x.SortAscending ? " desc" : "" ) ) );
			string where = string.Join( " and ", list.Select( x => x.where.Safe( ) ).Where( x => x != null ) );


			//string subquery = string.Format( "SELECT {0},", properties );
			//subquery += string.Format( "ROW_NUMBER() OVER ({0}) as RowNr,", orderBy );
			//subquery += string.Format( "(((ROW_NUMBER() OVER ({0})) - 1) / {1}) as PageNr,", orderBy, ItemsPerPage );
			//subquery += string.Format( "COUNT(*) OVER () AS CountTotal ", orderBy );
			//subquery += " FROM " + entity;
			//if (!string.IsNullOrEmpty( where )) subquery += " WHERE " + where;

			//var query = string.Format( "SELECT {0}, RowNr, PageNr, CountTotal FROM ({1}) AS temp", properties, subquery );


			string query = string.Format( @"
				SELECT {0}, ItemCountAllPages FROM (
				
					SELECT 
						{0},
						((((ROW_NUMBER() OVER ({1})) - 1) / {2}) + 1) as PageNr,
						COUNT(*) OVER () AS ItemCountAllPages 
					FROM {3}
					{4}

				) AS temp
				WHERE PageNr = {5}"
				, properties, orderBy, ItemsPerPage, entity, ( !string.IsNullOrEmpty( where ) ? "WHERE " + where : "" ), ActivePageNumber );
			
			query = query.Replace( "\n", " " ).Replace( "\r", String.Empty ).Replace( "\t", String.Empty );

			return query;
		}

		public static List<object[]> GetSqlResult( IDbConnection db, string query )
		{
			if (db.State != ConnectionState.Open)
			{
				db.Open( );
			}

			IDbCommand cmd = db.CreateCommand( );
			cmd.CommandText = query;

			var reader = cmd.ExecuteReader( );
			var result = new List<object[]>( );
			while (reader.Read( ))
			{
				var arr = new object[reader.FieldCount - 1]; //ItemCountAllPages-kolom niet meenemen, dus -1;

				var t = reader.GetValue( reader.FieldCount - 1 );
				reader.GetValues( arr );
				result.Add( arr );
			}

			return result;
		}
	}

	public class TableColumn
	{
		public string PropertyName { get; set; }
		public bool SortAscending { get; set; }
		public string SearchText { get; set; }
		public string RangeLeft { get; set; }
		public string RangeRight { get; set; }

		public string where
		{
			get
			{
				if (!string.IsNullOrEmpty( SearchText ))
				{
					return PropertyName + "=" + SearchText;
				}
				else if (!string.IsNullOrEmpty( RangeLeft ) && !string.IsNullOrEmpty( RangeRight ))
				{
					return null;
				}
				else return null;
			}
		}
	}
	
	public class PaginationResult
	{
		//public int ItemCountNoFilters { get; set; }
		public int ItemsPerPage { get; set; }
		public int ActivePageNumber { get; set; }
		public int PageCount { get; set; }
		public int BoundaryLeft { get; set; }
		public int BoundaryRight { get; set; }
		public List<object[]> Items { get; set; }

		public PaginationResult( int ItemsPerPage, int ActivePageNumber )
		{
			this.Items = new List<object[]>( );

			this.ItemsPerPage = ItemsPerPage;
			this.ActivePageNumber = ActivePageNumber;
		}
	}

	public static class ExtensionsSQL
	{
		public static string Safe( this string str )
		{
			return str;
			//

			if (str == null) return str;
			return str.Replace( "'", "''" );
		}
	}

}
