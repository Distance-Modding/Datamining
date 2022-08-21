using System;
using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Interfaces;

namespace AkWWISE.SoundBank.Util
{
	public static class VisitorUtil
	{
		public static T Visit<T>(IReader reader) where T : IVisitor, new()
		{
			T result = new T();
			result.Visit(reader);
			return result;
		}

		public static T Visit<T>(IReader reader, params object[] ctorArgs) where T : IVisitor
		{
			T result = (T)Activator.CreateInstance(typeof(T), ctorArgs);
			result.Visit(reader);
			return result;
		}
	}
}
