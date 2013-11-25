using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Base.Reflection
{
	public static class Reflection
	{
		//This class resides in its own namespace, to keep intellisense-popups clean;

		#region Overig
		/// <summary>
		/// Retrieves an object's property by string;
		/// 
		/// Example: SomeObject.getPropertyValue("SomeProperty")
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static T getPropertyValue<T>( this object obj, string propertyName )
		{
			return (T)(obj.GetType( ).GetProperty( propertyName ).GetValue( obj, null ));
		}


		public static void copyProperties( object source, object target )
		{
			BindingFlags bindingflags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			PropertyInfo[] propertiesSource = source.GetType( ).GetProperties( bindingflags );

			foreach (PropertyInfo pi in propertiesSource)
			{
				var propertyValue = pi.GetValue( source, null );
				var piTarget = target.GetType( ).GetProperty( pi.Name );
				piTarget.SetValue( target, propertyValue, null );
			}
		}
		#endregion

		#region Expression Trees
		public static LambdaExpression GetPropertyAccessLambdaExpression<T>( PropertyInfo pi )
		{
			//todo T kun je uit pi halen 

			//Example: var result = GetPropertyAccessLambdaExpression<AA>( SomePropertyInfo )
			//Example: var result = GetPropertyAccessLambdaExpression( typeof(AA), "SomeProperty" ) //todo weg???
			//Result: (AA x) => x.SomeProperty
			//Use Expression.Lambda<Func<AA, SomeType>>( result ) to convert to expression tree and .Compile to typed lambda;

			ParameterExpression typeT = Expression.Parameter( typeof( T ) );
			MemberExpression AccessProperty = Expression.Property( typeT, pi ); //This determines the return type of the lambda (pi.PropertyType);

			return Expression.Lambda( AccessProperty, typeT );
		}

		public static LambdaExpression GetPropertyAccessLambdaExpression<T>( string propertyName )
		{
			//Example: var result = GetPropertyAccessLambdaExpression<AA>( SomePropertyInfo )
			//Example: var result = GetPropertyAccessLambdaExpression( typeof(AA), "SomeProperty" ) //todo weg???
			//Result: (AA x) => x.SomeProperty
			//Use Expression.Lambda<Func<AA, SomeType>>( result ) to convert to expression tree and .Compile to typed lambda;
						
			ParameterExpression TypeIngoing = Expression.Parameter( typeof( T ) );
			return Expression.Lambda( GetPropertyAccess<T>( propertyName ), TypeIngoing );
		}

		public static MemberExpression GetPropertyAccess<T>( string propertyName )
		{
			//Example: var result = GetPropertyAccessLambdaExpression<AA>( SomePropertyInfo )
			//Example: var result = GetPropertyAccessLambdaExpression( typeof(AA), "SomeProperty" ) //todo weg???

			ParameterExpression typeT = Expression.Parameter( typeof( T ), "" );
			PropertyInfo pi = typeof( T ).GetProperty( propertyName );
			return Expression.Property( typeT, pi );
		}

		//Semi-trivial helpers;
		public static TResult InvokeGenericExtensionMethod<TResult>( object Target, MethodInfo method, Type[] GenericParameters, object[] MethodParameters )
		{
			return InvokeGenericStaticMethod<TResult>( method, GenericParameters, new List<object> { Target }.Union( MethodParameters ).ToArray( ) );
		}
		public static TResult InvokeGenericStaticMethod<TResult>( MethodInfo method, Type[] GenericParameters, object[] MethodParameters )
		{
			//Note: lastige details, vandaar aparte method;
			return (TResult)method.MakeGenericMethod( GenericParameters ).Invoke( null, MethodParameters );
		} 

		#endregion

		#region Attributes

		public static bool hasAttribute<T>( this Type type ) where T : Attribute
		{
			return type.GetCustomAttributes( typeof( T ), true ).Any( );
		}

		public static IEnumerable<Type> getTypesWithAttribute<T>( IEnumerable<Type> types ) where T : Attribute
		{
			foreach (Type type in types)
			{
				if (type.GetCustomAttributes( typeof( T ), true ).Any( ))
				{
					yield return type;
				}
			}
		}

		public static TAttribute getAttribute<TAttribute>( Type type ) where TAttribute : Attribute
		{
			return type
				.GetCustomAttributes( typeof( TAttribute ), false )
				.OfType<TAttribute>( )
				.SingleOrDefault( );
		}
		#endregion
	}

	public static class X 
	{
		//Helper class, named X for brevity;
		public static string name<T>( Expression<Func<T, object>> expr )
		{
			//Usage: string s = X.getName<AA>(x => x.someProp)

			UnaryExpression unary = expr.Body as UnaryExpression;
			MemberExpression mExpr = expr.Body as MemberExpression;
			if (unary != null) mExpr = unary.Operand as MemberExpression;
			return mExpr.Member.Name;
		}	
		//public static string name( Type type, Expression<Func<T, object>> expr )
		//{
		//    //Usage: string s = X.getName<AA>(x => x.someProp)

		//    UnaryExpression unary = expr.Body as UnaryExpression;
		//    MemberExpression mExpr = expr.Body as MemberExpression;
		//    if (unary != null) mExpr = unary.Operand as MemberExpression;
		//    return mExpr.Member.Name;
		//}	
	}
}
