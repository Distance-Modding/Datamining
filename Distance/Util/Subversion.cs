using System;
using System.IO;
using System.Reflection;
using Distance.Services;

namespace Distance.Util
{
	public class Subversion
	{
		public static string GetVersion(FileInfo assemblyFile)
		{
			if (!assemblyFile.Exists)
			{
				throw new FileNotFoundException(assemblyFile.FullName);
			}

			string domainName = $"Reflection.{Guid.NewGuid():B}";
			AssemblyReflection reflection = new AssemblyReflection();

			try
			{
				reflection.LoadAssembly(assemblyFile.FullName, domainName);

				return reflection.Reflect(assemblyFile.FullName, assembly =>
				{
					Type t_SVNRevision = assembly.GetType("SVNRevision");
					FieldInfo f_number = t_SVNRevision.GetField("number_", BindingFlags.Public | BindingFlags.Static);

					return f_number.GetValue(null).ToString();
				});
			}
			finally
			{
				reflection.UnloadDomain(domainName);
			}
		}

		public static string GetVersion(DirectoryInfo gameBaseDirectory)
		{
			return GetVersion(Game.GetGameAssemblyPath(gameBaseDirectory));
		}
	}
}
