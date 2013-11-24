using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Base.General
{
	public static class ExtensionMethods
	{
		public static T ParseEnum<T>( this Enum someEnum, string value )
		{
			return (T)Enum.Parse( typeof( T ), value, true );
		}

		public static int? toIntNullable( this string str )
		{ 
			int temp = 0;
			bool success = int.TryParse( str, out temp );
			return success ? new int?(temp) : null;
		}

		public static V ValueOrDefault<T, V>( this T obj, Func<T, V> func, V defaultValue )
		{
			//Usage:
			//var result = SomeReferenceType.ValueOrDefault( x => x.SomeSubType.SomeProperty, "someDefaultValue" );

			return ( obj != null ) ? func( obj ) : defaultValue;
		}

		#region Nullable<bool>
		public static bool TrueOrNull( this bool? nullableBool )
		{
			return nullableBool.HasValue ? nullableBool.Value : true;
		}
		public static bool FalseOrNull( this bool? nullableBool )
		{
			return nullableBool.HasValue ? !nullableBool.Value : true;
		}
		public static bool IsTrue( this bool? nullableBool )
		{
			return nullableBool.HasValue ? nullableBool.Value : false;
		}
		public static bool IsFalse( this bool? nullableBool )
		{
			return nullableBool.HasValue ? !nullableBool.Value : false;
		}
		#endregion
	}
}
