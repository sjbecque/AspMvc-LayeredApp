using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.General
{
	public static class New
	{
		public static List<T> List<T>( params T[] ListMembers )
		{
			//Note: helper for compactness in MVC views for example;
			return new List<T>( ListMembers );
		}
	}
}
