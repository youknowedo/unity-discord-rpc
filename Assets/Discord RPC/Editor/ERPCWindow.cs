#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ERPC
{
    public class ERPCWindow : EditorWindow
    {
        private static ERPCWindow _window;
        public static bool customDetailsState = false;

        public static string status = "Up to Date";

        public GUIStyle styling = new GUIStyle();

        [MenuItem("Window/Editor Rich Presence")]
        private static void Init()
        {
            _window = (ERPCWindow)GetWindow(typeof(ERPCWindow), false, "Editor Rich Presence");
            _window.Show();
        }
        private void OnGUI()
        {
            styling.margin = new RectOffset(10, 10, 10, 10);

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginVertical(styling);

            GUILayout.Label("Status: " + status);

            GUILayout.Space(20);

            GUILayout.Label("Current Project: " + ERPC.projectName);
            GUILayout.Label("Current Scene: " + ERPC.sceneName);

            GUILayout.Space(20);

            GUILayout.Label("Editor Application ID");
            ERPC.applicationID = GUILayout.TextField(ERPC.applicationID);

            GUILayout.Space(10);

            customDetailsState = EditorGUILayout.ToggleLeft("Custom Details/State", customDetailsState);

            GUILayout.Space(10);

            if (customDetailsState)
            {
                GUILayout.Label("Details");
                ERPC.details = GUILayout.TextField(ERPC.details);

                GUILayout.Label("State");
                ERPC.state = GUILayout.TextField(ERPC.state);
            }

            GUILayout.Label("Large Image", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Key", "(e.g. Game Icon)"), GUILayout.MaxWidth(30f));
            ERPC.largeImageKey = GUILayout.TextField(ERPC.largeImageKey, GUILayout.MaxWidth(120f));

            GUILayout.Label(new GUIContent("Text", "(e.g. Game Name)"), GUILayout.MaxWidth(30f));
            ERPC.largeImageText = GUILayout.TextField(ERPC.largeImageText, GUILayout.MaxWidth(120f));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Small Image", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Key", "(e.g. Unity Logo)"), GUILayout.MaxWidth(30f));
            ERPC.smallImageKey = GUILayout.TextField(ERPC.smallImageKey, GUILayout.MaxWidth(120f));

            GUILayout.Label(new GUIContent("Text", "(e.g. \"Made with Unity\")"), GUILayout.MaxWidth(30f));
            ERPC.smallImageText = GUILayout.TextField(ERPC.smallImageText, GUILayout.MaxWidth(120f));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            ERPC.resetOnSceneChange = EditorGUILayout.ToggleLeft("Reset timestap on scene change", ERPC.resetOnSceneChange);

            if (EditorGUI.EndChangeCheck())
            {
                ERPC.lastEdit = EditorApplication.timeSinceStartup;
                status = "Updating...";
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Update Presence"))
            {
                ERPC.UpdateActivity();
            }

            GUILayout.EndVertical();
        }

        private bool ToggleButton(string trueText, string falseText, ref bool value)
        {
            if (value && GUILayout.Button(trueText))
            {
                value = false;
                return true;
            }
            else if (!value && GUILayout.Button(falseText))
            {
                value = true;
                return true;
            }
            return false;
        }
    }
}
#endif
