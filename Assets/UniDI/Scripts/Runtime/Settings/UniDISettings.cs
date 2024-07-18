#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UniDI.Settings
{
    internal class UniDISettings : ScriptableObject
    {
        public int LocalScopes = 16;
        public int GameInstances = 256;
        public int SceneInstances = 64;
        public int GameLocalInstances = 128;
        public int SceneLocalInstances = 32;
        public int InjectedFields = 128;
        public int InjectedProperties = 128;
        public int InjectedMethods = 128;
        public int InjectedLocalMethods = 32;
        public bool ShowReinjectLog = true;

        internal static UniDISettings GetSettings()
        {
            var settings = Resources.Load<UniDISettings>(nameof(UniDISettings));

#if UNITY_EDITOR
            if (settings == null)
            {
                var isResourcesFolderExist = AssetDatabase.IsValidFolder("Assets/Resources");
                if (!isResourcesFolderExist)
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                AssetDatabase.CreateAsset(CreateInstance(nameof(UniDISettings)), $"Assets/Resources/{nameof(UniDISettings)}.asset");
                settings = Resources.Load<UniDISettings>(nameof(UniDISettings));
            }
#endif

            if (settings == null)
            {

                throw new UniDIException("Can't load settings. Please try to reimport package.");
            }

            return settings;
        }
    }
}
