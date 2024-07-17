using UniDI.Settings;
using UnityEditor;
using UnityEngine;

namespace UniDI.Editor
{
    public class UniDIEditorWindow : EditorWindow
    {
        private static UniDISettings _settings;
        private const string WINDOW_NAME = "UniDI Settings";
        private const string COMMON_SETTINGS = "Common settings";
        private const string LOCAL_SCOPES_TIP = "Count of local scopes.\n\nclass1.Inject();\t\t//local scopes: 0\nclass2.Inject(52);\t\t" +
            "//local scopes:1\nclass3.Inject(52);\t\t//local scopes:1\nclass3.Inject(981);\t//local scopes:2";
        private const string INSTANCES_COUNT_SETTINGS = "Injected instances count";
        private const string GAME_INSTANCES_TIP = "Count of global scope instances injected with Lifetime.Game parameter.\n\n" +
            "class1.Inject();\t\t\t//instances:1\nclass2.Inject(55);\t\t\t//instances:1\nclass3.Inject(Lifetime.Game);\t//instances:2";
        private const string SCENE_INSTANCES_TIP = "Count of global scope instances injected with Lifetime.Scene parameter.\n\n" +
            "class1.Inject();\t\t\t//instances:0\nclass2.Inject(Lifetime.Scene);\t//instances:1";
        private const string GAME_LOCAL_INSTANCES_TIP = "Count of local scope instances injected with Lifetime.Game parameter for each scope.\n\n" +
            "class1.Inject();\t\t\t\t//inst:0\nclass2.Inject(55);\t\t\t\t//inst:1\nclass3.Inject(489, Lifetime.Game);\t//inst:2";
        private const string SCENE_LOCAL_INSTANCES_TIP = "Count of local scope instances injected with Lifetime.Scene parameter for each scope.\n\n" +
            "class1.Inject(Lifetime.Scene);\t\t//inst:0\nclass2.Inject(55);\t\t\t\t//inst:0\nclass3.Inject(489, Lifetime.Scene);\t//inst:1";
        private const string INJECTION_USAGE_COUNT_SETTINGS = "Injection usage count";
        private const string INJECTED_FIELDS_TIP = "Count of global and local scope fields marked as [Inject].";
        private const string INJECTED_PROPERTIES_TIP = "Count of global and local scope properties marked as [Inject].";
        private const string INJECTED_METHODS_TIP = "Count of global scope methods marked as [Inject].";
        private const string INJECTED_LOCAL_METHODS_TIP = "Count of local scope methods marked as [Inject] for each scope.";

        [MenuItem("Tools/UniDI/Settings")]
        public static void Open()
        {
            var settings = GetSettings();
            GetWindow<UniDIEditorWindow>(WINDOW_NAME);
        }

        private void OnGUI()
        {
            GUILayout.Label(COMMON_SETTINGS, EditorStyles.boldLabel);
            _settings.LocalScopes = EditorGUILayout.IntField(new GUIContent(nameof(_settings.LocalScopes), LOCAL_SCOPES_TIP), _settings.LocalScopes);
            EditorGUILayout.Space();
            GUILayout.Label(INSTANCES_COUNT_SETTINGS, EditorStyles.boldLabel);
            _settings.GameInstances = EditorGUILayout.IntField(new GUIContent(nameof(_settings.GameInstances), GAME_INSTANCES_TIP), _settings.GameInstances);
            _settings.GameLocalInstances = EditorGUILayout.IntField(new GUIContent(nameof(_settings.GameLocalInstances), GAME_LOCAL_INSTANCES_TIP), _settings.GameLocalInstances);
            _settings.SceneInstances = EditorGUILayout.IntField(new GUIContent(nameof(_settings.SceneInstances), SCENE_INSTANCES_TIP), _settings.SceneInstances);
            _settings.SceneLocalInstances = EditorGUILayout.IntField(new GUIContent(nameof(_settings.SceneLocalInstances), SCENE_LOCAL_INSTANCES_TIP), _settings.SceneLocalInstances);
            GUILayout.Label(INJECTION_USAGE_COUNT_SETTINGS, EditorStyles.boldLabel);
            EditorGUILayout.Space();
            _settings.InjectedFields = EditorGUILayout.IntField(new GUIContent(nameof(_settings.InjectedFields), INJECTED_FIELDS_TIP), _settings.InjectedFields);
            _settings.InjectedProperties = EditorGUILayout.IntField(new GUIContent(nameof(_settings.InjectedProperties), INJECTED_PROPERTIES_TIP), _settings.InjectedProperties);
            _settings.InjectedMethods = EditorGUILayout.IntField(new GUIContent(nameof(_settings.InjectedMethods), INJECTED_METHODS_TIP), _settings.InjectedMethods);
            _settings.InjectedLocalMethods = EditorGUILayout.IntField(new GUIContent(nameof(_settings.InjectedLocalMethods), INJECTED_LOCAL_METHODS_TIP), _settings.InjectedLocalMethods);
        }

        private static UniDISettings GetSettings()
        {
            if (_settings != null)
            {
                return _settings;
            }

            _settings = Resources.Load<UniDISettings>(nameof(UniDISettings));
            if (_settings == null)
            {
                throw new UniDIException("Can't load settings. Please try to reimport package.");
            }

            return _settings;
        }
    }
}
