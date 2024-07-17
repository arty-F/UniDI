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
    }
}
