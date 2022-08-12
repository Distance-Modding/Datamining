using System.Collections.Generic;
using System.Linq;
using AssetStudio;
using Distance.Data;

namespace Distance.Services
{
	public class AssetInfoDatabase
	{
		private readonly AssetsManager assetsManager;

        public string ProductName { get; protected internal set; }

        public List<GameAsset> Assets { get; protected internal set; } = new List<GameAsset>();

		public AssetInfoDatabase(AssetsManager assetsManager)
		{
			this.assetsManager = assetsManager;
            BuildAssetData();
        }

		public static AssetInfoDatabase From(AssetsManager assetsManager) => new AssetInfoDatabase(assetsManager);

		protected void BuildAssetData()
		{
			int objectCount = assetsManager.assetsFileList.Sum(x => x.Objects.Count);

			List<(PPtr<Object>, string)> containers = new List<(PPtr<Object>, string)>();
			Dictionary<Object, GameAsset> objectAssetItemDic = new Dictionary<Object, GameAsset>(objectCount);
            
			foreach (SerializedFile assetsFile in assetsManager.assetsFileList)
			{
				foreach (Object assetObj in assetsFile.Objects) {
					GameAsset asset = new GameAsset(assetObj);
                    Assets.Add(asset);
                    objectAssetItemDic.Add(assetObj, asset);

                    switch (assetObj)
                    {
                        case GameObject m_GameObject:
                            asset.Name = m_GameObject.m_Name;
                            break;
                        case Texture2D m_Texture2D:
                            if (!string.IsNullOrEmpty(m_Texture2D.m_StreamData?.path))
                            {
                                asset.FullSize = assetObj.byteSize + m_Texture2D.m_StreamData.size;
                            }
                            asset.Name = m_Texture2D.m_Name;
                            break;
                        case AudioClip m_AudioClip:
                            if (!string.IsNullOrEmpty(m_AudioClip.m_Source))
                            {
                                asset.FullSize = assetObj.byteSize + m_AudioClip.m_Size;
                            }
                            asset.Name = m_AudioClip.m_Name;
                            break;
                        case VideoClip m_VideoClip:
                            if (!string.IsNullOrEmpty(m_VideoClip.m_OriginalPath))
                            {
                                asset.FullSize = assetObj.byteSize + m_VideoClip.m_ExternalResources.m_Size;
                            }
                            asset.Name = m_VideoClip.m_Name;
                            break;
                        case Shader m_Shader:
                            asset.Name = m_Shader.m_ParsedForm?.m_Name ?? m_Shader.m_Name;
                            break;
                        case Mesh _:
                        case TextAsset _:
                        case AnimationClip _:
                        case Font _:
                        case MovieTexture _:
                        case Sprite _:
                            asset.Name = ((NamedObject)asset).m_Name;
                            break;
                        case Animator m_Animator:
                            if (m_Animator.m_GameObject.TryGet(out var gameObject))
                            {
                                asset.Name = gameObject.m_Name;
                            }
                            break;
                        case MonoBehaviour m_MonoBehaviour:
                            if (m_MonoBehaviour.m_Name == "" && m_MonoBehaviour.m_Script.TryGet(out var m_Script))
                            {
                                asset.Name = m_Script.m_ClassName;
                            }
                            else
                            {
                                asset.Name = m_MonoBehaviour.m_Name;
                            }
                            break;
                        case PlayerSettings m_PlayerSettings:
                            ProductName = m_PlayerSettings.productName;
                            break;
                        case AssetBundle m_AssetBundle:
                            foreach (var m_Container in m_AssetBundle.m_Container)
                            {
                                var preloadIndex = m_Container.Value.preloadIndex;
                                var preloadSize = m_Container.Value.preloadSize;
                                var preloadEnd = preloadIndex + preloadSize;
                                for (int k = preloadIndex; k < preloadEnd; k++)
                                {
                                    containers.Add((m_AssetBundle.m_PreloadTable[k], m_Container.Key));
                                }
                            }
                            asset.Name = m_AssetBundle.m_Name;
                            break;
                        case ResourceManager m_ResourceManager:
                            foreach (var m_Container in m_ResourceManager.m_Container)
                            {
                                containers.Add((m_Container.Value, m_Container.Key));
                            }
                            break;
                        case NamedObject m_NamedObject:
                            asset.Name = m_NamedObject.m_Name;
                            break;
                    }
                }
			}

            foreach ((PPtr<Object> pptr, string container) in containers)
            {
                if (pptr.TryGet(out Object obj))
                {
                    objectAssetItemDic[obj].Container = container;
                }
            }

            containers.Clear();
            objectAssetItemDic.Clear();
        }

		~AssetInfoDatabase()
		{
            Assets.Clear();
		}
	}
}
