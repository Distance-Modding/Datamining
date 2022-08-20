using AkWWISE.IO.Interfaces;

namespace AkWWISE.SoundBank.Interfaces
{
	public interface IVisitor
	{
		void Visit(IReader reader);
	}
}
