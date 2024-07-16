namespace UniDI.Settings
{
    internal class UniDISettings
    {
        private int _localScopes = 16;
        private int _gameInstances = 256;
        private int _sceneInstances = 64;
        private int _gameLocalInstances = 128;
        private int _sceneLocalInstances = 32;
        private int _injectedFields = 128;
        private int _injectedProperties = 128;
        private int _injectedMethods = 128;
        private int _injectedLocalMethods = 32;

        internal int LocalScopes => _localScopes;
        internal int GameInstances => _gameInstances;
        internal int SceneInstances => _sceneInstances;
        internal int GameLocalInstances => _gameLocalInstances;
        internal int SceneLocalInstances => _sceneLocalInstances;
        internal int InjectedFields => _injectedFields;
        internal int InjectedProperties => _injectedProperties;
        internal int InjectedMethods => _injectedMethods;
        internal int InjectedLocalMethods => _injectedLocalMethods;

        internal static UniDISettings GetInstance() => new UniDISettings();
    }
}
