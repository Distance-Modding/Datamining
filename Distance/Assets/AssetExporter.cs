using System.IO;
using System.Text;
using AssetStudio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Distance.Assets
{
	public static class AssetExporter
	{
		public static void ExportSprite(FileInfo exportFile, Sprite m_Sprite)
		{
			using (Image<Bgra32> image = m_Sprite.GetImage())
			{
				using (FileStream fileStream = File.OpenWrite(exportFile.FullName))
				{
					image.WriteToStream(fileStream, ImageFormat.Png);
				}
			}
		}

		public static void ExportTexture2D(FileInfo exportFile, Texture2D m_Texture2D)
		{
			using (Image<Bgra32> image = m_Texture2D.ConvertToImage(true))
			{
				using (FileStream fileStream = File.OpenWrite(exportFile.FullName))
				{
					image.WriteToStream(fileStream, ImageFormat.Png);
				}
			}
		}

		public static void ExportTextAsset(FileInfo exportFile, TextAsset m_TextAsset)
		{
			File.WriteAllBytes(exportFile.FullName, m_TextAsset.m_Script);
		}

		public static void ExportFont(FileInfo exportFile, Font m_Font)
		{
			File.WriteAllBytes(exportFile.FullName, m_Font.m_FontData);
		}

		public static void ExportAudioClip(FileInfo exportFile, AudioClip m_AudioClip)
		{
			AudioClipConverter converter = new AudioClipConverter(m_AudioClip);
			if (converter.IsSupport)
			{
				File.WriteAllBytes(exportFile.FullName, converter.ConvertToWav());
			}
			else
			{
				File.WriteAllBytes(exportFile.FullName, m_AudioClip.m_AudioData.GetData());
			}
		}

		public static void ExportVideoClip(FileInfo exportFile, VideoClip m_VideoClip)
		{
			m_VideoClip.m_VideoData.WriteData(exportFile.FullName);
		}

		public static void ExportMovieTexture(FileInfo exportFile, MovieTexture m_MovieTexture)
		{
			File.WriteAllBytes(exportFile.FullName, m_MovieTexture.m_MovieData);
		}

		public static void ExportMesh(FileInfo exportFile, Mesh m_Mesh)
		{
            if (m_Mesh.m_VertexCount <= 0)
			{
                return;
			}
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("g " + m_Mesh.m_Name);
            #region Vertices
            if (m_Mesh.m_Vertices == null || m_Mesh.m_Vertices.Length == 0)
            {
                return;
            }
            int c = 3;
            if (m_Mesh.m_Vertices.Length == m_Mesh.m_VertexCount * 4)
            {
                c = 4;
            }
            for (int v = 0; v < m_Mesh.m_VertexCount; v++)
            {
                sb.AppendFormat("v {0} {1} {2}\r\n", -m_Mesh.m_Vertices[v * c], m_Mesh.m_Vertices[v * c + 1], m_Mesh.m_Vertices[v * c + 2]);
            }
            #endregion

            #region UV
            if (m_Mesh.m_UV0?.Length > 0)
            {
                c = 4;
                if (m_Mesh.m_UV0.Length == m_Mesh.m_VertexCount * 2)
                {
                    c = 2;
                }
                else if (m_Mesh.m_UV0.Length == m_Mesh.m_VertexCount * 3)
                {
                    c = 3;
                }
                for (int v = 0; v < m_Mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vt {0} {1}\r\n", m_Mesh.m_UV0[v * c], m_Mesh.m_UV0[v * c + 1]);
                }
            }
            #endregion

            #region Normals
            if (m_Mesh.m_Normals?.Length > 0)
            {
                if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 3)
                {
                    c = 3;
                }
                else if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 4)
                {
                    c = 4;
                }
                for (int v = 0; v < m_Mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vn {0} {1} {2}\r\n", -m_Mesh.m_Normals[v * c], m_Mesh.m_Normals[v * c + 1], m_Mesh.m_Normals[v * c + 2]);
                }
            }
            #endregion

            #region Face
            int sum = 0;
            for (var i = 0; i < m_Mesh.m_SubMeshes.Length; i++)
            {
                sb.AppendLine($"g {m_Mesh.m_Name}_{i}");
                int indexCount = (int)m_Mesh.m_SubMeshes[i].indexCount;
                var end = sum + indexCount / 3;
                for (int f = sum; f < end; f++)
                {
                    sb.AppendFormat("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\r\n", m_Mesh.m_Indices[f * 3 + 2] + 1, m_Mesh.m_Indices[f * 3 + 1] + 1, m_Mesh.m_Indices[f * 3] + 1);
                }
                sum = end;
            }
            #endregion

            sb.Replace("NaN", "0");
            File.WriteAllText(exportFile.FullName, sb.ToString());
        }
	}
}
