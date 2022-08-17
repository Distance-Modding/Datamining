using System;
using System.IO;
using System.Text;
using System.Xml;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;

namespace Distance.Serializer
{
	public class Xml<T> where T : new()
	{
		public static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings()
		{
			Indent = true,
			IndentChars = "\t"
		};

		public static T Deserialize(FileInfo file)
		{
			return Deserialize(File.OpenRead(file.FullName));
		}

		public static T Deserialize(string xml)
		{
			using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
			{
				return Deserialize(stream);
			}
		}

		public static T Deserialize(Stream stream)
		{
			try
			{
				return GetSerializer().Deserialize<T>(stream);
			}
			catch (InvalidOperationException)
			{
				return default;
			}
		}

		public static string Serialize(T data)
		{
			return GetSerializer()
				.Serialize(WriterSettings, data);
		}

		internal static IExtendedXmlSerializer GetSerializer()
			=> new ConfigurationContainer()
				.UseAutoFormatting()
				.UseOptimizedNamespaces()
				.EnableImplicitTyping(typeof(T))
				.Create();

		public static void Serialize(T data, FileInfo file)
		{
			File.WriteAllText(file.FullName, Serialize(data));
		}
	}
}
