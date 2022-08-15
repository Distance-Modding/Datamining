using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static Distance.Util.FileSystem;

namespace Distance.Services
{
	public class DirectoryMerger
	{
		public DirectoryInfo Source { get; protected internal set; }

		public string SubFolder { get; protected internal set; }

		protected readonly HashSet<FilePath> paths;

		public DirectoryMerger(DirectoryInfo source, string subFolder = null)
		{
			Source = source;
			SubFolder = subFolder;
			paths = new HashSet<FilePath>();
			ProcessDirectoryStructure();
		}

		protected void ProcessDirectoryStructure()
		{
			foreach (DirectoryInfo subdir in Source.GetDirectories())
			{
				Console.WriteLine($"Indexing\t\"{subdir.Name}\"...");

				ProcessDirectoryStructure(DirectoryWithSubfolder(subdir));
			}
		}

		protected void ProcessDirectoryStructure(DirectoryInfo directory)
		{
			if (!directory.Exists)
			{
				return;
			}

			foreach (FileInfo file in directory.GetFiles("*", SearchOption.AllDirectories))
			{
				paths.Add(new FilePath(file.FullName.Substring(directory.FullName.Length + 1)));
				Console.Title = $"{paths.Count} files indexed";
			}
		}

		public void MergeTo(DirectoryInfo destination)
		{
			DirectoryInfo destinationRoot = DirectoryWithSubfolder(destination);
			Dictionary<string, FileInfo> toCopy = new Dictionary<string, FileInfo>();

			FileInfo GetDestinationFile(string filePath, string hash, FileInfo sourceFile)
			{
				FileInfo tempFile = new FileInfo(Path.Combine(destinationRoot.FullName, filePath));
				return new FileInfo(Path.Combine(tempFile.Directory.FullName, $"{Path.GetFileNameWithoutExtension(tempFile.Name)}.{hash}{tempFile.Extension}"));
			}

			DirectoryInfo[] sourceDirectories = Source.GetDirectories()
				.Select(DirectoryWithSubfolder)
				.Where(dir => dir.Exists)
				.ToArray();

			int index = 0;
			foreach (string filePath in paths)
			{
				Console.Title = $"Merging \"{filePath}\" ({++index}/{paths.Count})";

				Console.WriteLine($"Processing file\t\"{filePath}\"");
				toCopy.Clear();
				foreach (DirectoryInfo sourceRoot in sourceDirectories)
				{
					if (!sourceRoot.Exists)
					{
						continue;
					}

					FileInfo sourceFile = new FileInfo(Path.Combine(sourceRoot.FullName, filePath));
					if (sourceFile.Exists)
					{
						string hash = ComputeHash(sourceFile);
						if (!toCopy.ContainsKey(hash))
						{
							if (GetDestinationFile(filePath, hash, sourceFile).Exists)
							{
								Console.WriteLine($"\tSkipping...");
								goto endLoopFilePathMerge;
							}
							Console.WriteLine($"\tFound hash\t\"{hash}\"");
							toCopy.Add(hash, sourceFile);
						}
					}
				}

				Console.WriteLine($"\t{toCopy.Count} version(s) of file\t\"{filePath}\" found");

				foreach (KeyValuePair<string, FileInfo> item in toCopy)
				{
					string hash = item.Key;
					FileInfo sourceFile = item.Value;
					try
					{
						FileInfo destinationFile = GetDestinationFile(filePath, hash, sourceFile);
						if (!destinationFile.Directory.Exists)
						{
							destinationFile.Directory.Create();
						}

						sourceFile.CopyTo(destinationFile.FullName, true);
						Console.WriteLine($"\tCopied \"{sourceFile.FullName}\" to \"{destinationFile.FullName}\"");
					}
					catch (Exception anyException)
					{
						Console.WriteLine($"\tError while copying \"{sourceFile.FullName}\" ({hash})... {anyException}");
					}
				}
				endLoopFilePathMerge:;
			}
		}

		protected DirectoryInfo DirectoryWithSubfolder(DirectoryInfo baseDir)
			=> string.IsNullOrEmpty(SubFolder)
			? baseDir
			: new DirectoryInfo(Path.Combine(baseDir.FullName, SubFolder));

		protected string ComputeHash(FileInfo file)
		{
			if (!file.Exists)
			{
				return null;
			}

			using (MD5 hashAlgorithm = MD5.Create())
			{
				using (FileStream stream = File.OpenRead(file.FullName))
				{
					byte[] hash = hashAlgorithm.ComputeHash(stream);
					return Convert.ToBase64String(hash).ToLower().RemoveIllegalPathChars().RemoveUnwantedChars();
				}
			}
		}

		~DirectoryMerger()
		{
			paths.Clear();
		}

		protected struct FilePath
		{
			private readonly string path;

			public FilePath(string path)
			{
				this.path = path;
			}

			public override bool Equals(object obj)
			{
				return obj is FilePath other
					&& path.ToLower() == other.path.ToLower();
			}

			public override int GetHashCode()
			{
				return -1757656154 + EqualityComparer<string>.Default.GetHashCode(path.ToLower());
			}

			public override string ToString() => this;

			public static implicit operator string(FilePath instance) => instance.path;
		}
	}
}
