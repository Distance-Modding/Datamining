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
	}
}
