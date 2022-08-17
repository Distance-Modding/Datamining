using AkWWISE.Metadata.Soundbanks;
using Distance.Data;
using Distance.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using File = System.IO.File;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Distance.Services.Extractors
{
	public class WwiseXmlEventsExtractor : IExtractor<FileInfo>
	{
		public Dictionary<StringKey, List<SoundBanksInfo>> SoundBanksInfos { get; protected internal set; }

		public WwiseXmlEventsExtractor()
		{
			SoundBanksInfos = new Dictionary<StringKey, List<SoundBanksInfo>>();
		}

		public void LoadXml(FileInfo xmlFile)
		{
			if (!xmlFile.Exists)
			{
				return;
			}

			SoundBanksInfo bankInfo = null;
			try
			{
				bankInfo = Xml<SoundBanksInfo>.Deserialize(xmlFile);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return;
			}

			StringKey path = new StringKey(Path.GetFileNameWithoutExtension(xmlFile.FullName));
			Add(path, bankInfo);
		}

		public void Add(string path, SoundBanksInfo bankInfo) => Add(new StringKey(path), bankInfo);

		public void Add(StringKey path, SoundBanksInfo bankInfo)
		{
			if (bankInfo is null)
			{
				return;
			}

			if (!SoundBanksInfos.ContainsKey(path))
			{
				SoundBanksInfos.Add(path, new List<SoundBanksInfo>());
			}
			SoundBanksInfos[path].Add(bankInfo);

			Console.WriteLine("\tbank information added");
		}

		public void ExtractTo(FileInfo destination)
		{
			using (Stream stream = File.OpenWrite(destination.FullName))
			{
				IWorkbook workbook = new XSSFWorkbook();
				ISheet allEventsSheet = workbook.CreateSheet("All Soundbanks");

				List<WwiseEventItem> allEvents = new List<WwiseEventItem>();

				foreach (KeyValuePair<StringKey, List<SoundBanksInfo>> element in SoundBanksInfos)
				{
					StringKey path = element.Key;
					List<SoundBanksInfo> bankInfos = element.Value;

					List<WwiseEventItem> events = bankInfos
						.SelectManyNotNull(bankInfo => bankInfo?.SoundBanks)
						.SelectManyNotNull(soundBank => soundBank?.IncludedEvents)
						.Select(akEvent => new WwiseEventItem(akEvent))
						.DistinctByHashCode()
						.OrderBy(akEvent => akEvent.Event)
						.ThenBy(akEvent => akEvent.Path)
						.ToList();

					ISheet sheet = workbook.CreateSheet(path);
					FillWorkbookSheetWithData(sheet, events);

					allEvents.AddRange(events);
				}

				allEvents = allEvents
					.DistinctByHashCode()
					.OrderBy(akEvent => akEvent.Event)
					.ThenBy(akEvent => akEvent.Path)
					.ToList();

				FillWorkbookSheetWithData(allEventsSheet, allEvents);

				workbook.Write(stream);
			}
		}

		protected void FillWorkbookSheetWithData(ISheet sheet, IEnumerable<WwiseEventItem> events) 
		{
			IRow headerRow = sheet.CreateRow(0);
			headerRow.CreateCell(0).SetCellValue("WWISE Event name");
			headerRow.CreateCell(1).SetCellValue("Path");

			int index = 0;
			foreach (WwiseEventItem eventItem in events)
			{
				IRow row = sheet.CreateRow(++index);
				row.CreateCell(0).SetCellValue(eventItem.Event);
				row.CreateCell(1).SetCellValue(eventItem.Path);
			}
		}

		void IExtractor<FileInfo>.ExtractTo(FileInfo destination) => ExtractTo(destination);

		~WwiseXmlEventsExtractor()
		{
			foreach (List<SoundBanksInfo> item in SoundBanksInfos.Values)
			{
				item.Clear();
			}
			SoundBanksInfos.Clear();
		}
	}	
}
