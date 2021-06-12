#if UNITY_EDITOR && UNITY_STANDALONE
using System;
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

        public static bool firstButtonIsValid = false;
        public static bool secondButtonIsValid = false;

        public static bool error = false;

        public static bool canUpdate { get; set; }

        public GUIStyle heading;

        [MenuItem("Window/Editor Rich Presence")]
        private static void Init()
        {
            _window = (ERPCWindow)GetWindow(typeof(ERPCWindow), false, "Editor Rich Presence");
            _window.Show();
        }

        public void OnGUI()
        {
#region Toolbar

            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));


            ERPC.enableOnStartup = GUILayout.Toggle(ERPC.enableOnStartup, "Enable on Startup", "ToolbarButton");

            EditorGUI.BeginChangeCheck();

            GUILayout.FlexibleSpace();
            
            ERPC.autoUpdate = GUILayout.Toggle(ERPC.autoUpdate, "Auto Update", "ToolbarButton");

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(!canUpdate);
            if (GUILayout.Button("Update", "ToolbarButton"))
            {
                ERPC.UpdateActivity();

                canUpdate = false;

                ERPC.lastEdit = 0f;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

#endregion

            GUILayout.BeginVertical();

#region Info

            GUILayout.Label("Editor Application ID");
            ERPC.applicationID = GUILayout.TextField(ERPC.applicationID);

            GUILayout.Space(10);

#endregion

#region Details/State

            customDetailsState = EditorGUILayout.ToggleLeft("Custom Details/State", customDetailsState);

            EditorGUI.BeginDisabledGroup(!customDetailsState);
            GUILayout.Label("Details");
            ERPC.details = GUILayout.TextField(ERPC.details);

            GUILayout.Label("State");
            ERPC.state = GUILayout.TextField(ERPC.state);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(20);

#endregion

            ERPC.resetOnSceneChange = EditorGUILayout.ToggleLeft("Reset timestap on scene change", ERPC.resetOnSceneChange);

#region Images

            GUILayout.Label("Large Image", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Key", "(e.g. Game Icon)"), GUILayout.MaxWidth(30f));
            ERPC.largeImageKey = GUILayout.TextField(ERPC.largeImageKey, GUILayout.MaxWidth(120f));

            GUILayout.Label(new GUIContent("Text", "(e.g. Game Name)"), GUILayout.MaxWidth(30f));
            ERPC.largeImageText = GUILayout.TextField(ERPC.largeImageText);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Small Image", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Key", "(e.g. Unity Logo)"), GUILayout.MaxWidth(30f));
            ERPC.smallImageKey = GUILayout.TextField(ERPC.smallImageKey, GUILayout.MaxWidth(120f));

            GUILayout.Label(new GUIContent("Text", "(e.g. \"Made with Unity\")"), GUILayout.MaxWidth(30f));
            ERPC.smallImageText = GUILayout.TextField(ERPC.smallImageText);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

#endregion

#region Buttons

            CheckButtons();

            if (firstButtonIsValid) GUILayout.Label("Button 1", EditorStyles.boldLabel);
            else GUILayout.Label("Button", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Text", "(e.g. \"Itch.io\" or \"Steam\")"), GUILayout.MaxWidth(30f));
            ERPC.firstButtonLabel = GUILayout.TextField(ERPC.firstButtonLabel, GUILayout.MaxWidth(120f));

            GUILayout.Label(new GUIContent("URL", "(e.g. Itch.io or Steam URL)"), GUILayout.MaxWidth(30f));
            ERPC.firstButtonUrl = GUILayout.TextField(ERPC.firstButtonUrl);
            GUILayout.EndHorizontal();

            ButtonErrors(ERPC.firstButtonLabel, ERPC.firstButtonUrl);

            GUILayout.Space(10);

            if (firstButtonIsValid)
            {
                GUILayout.Label("Button 2", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Text", "(e.g. \"GitHub\")"), GUILayout.MaxWidth(30f));
                ERPC.secondButtonLabel = GUILayout.TextField(ERPC.secondButtonLabel, GUILayout.MaxWidth(120f));

                GUILayout.Label(new GUIContent("URL", "(e.g. GitHub URL)"), GUILayout.MaxWidth(30f));
                ERPC.secondButtonUrl = GUILayout.TextField(ERPC.secondButtonUrl);
                GUILayout.EndHorizontal();

                ButtonErrors(ERPC.firstButtonLabel, ERPC.firstButtonUrl);
            }

            GUILayout.Space(20);

#endregion

            if (ERPC.enabled)
            {
                ERPC.enabled = GUILayout.Toggle(ERPC.enabled, "Shutdown", "Button");
            }
            else
            {
                ERPC.enabled = GUILayout.Toggle(ERPC.enabled, "Start", "Button");
            }

            if (EditorGUI.EndChangeCheck())
            {
                ERPC.lastEdit = EditorApplication.timeSinceStartup;
                canUpdate = true;
            }

            GUILayout.EndVertical();
        }

        public bool ButtonIsValid(string buttonLabel, string buttonUrl) => ButtonUrlIsValid(buttonUrl) && !string.IsNullOrEmpty(buttonLabel);

        public void CheckButtons()
        {
            firstButtonIsValid = ButtonIsValid(ERPC.firstButtonLabel, ERPC.firstButtonUrl);
            if (firstButtonIsValid)
            {
                secondButtonIsValid = ButtonIsValid(ERPC.secondButtonLabel, ERPC.secondButtonUrl);
            }
            else
            {
                secondButtonIsValid = false;
            }
        }

        private void ButtonErrors(string buttonLabel, string buttonUrl)
        {
            GUI.contentColor = Color.red;

            if (!ButtonUrlIsValid(buttonUrl) && !string.IsNullOrEmpty(buttonLabel))
            {
                GUILayout.Label("URL is not valid!", EditorStyles.boldLabel);
                canUpdate = false;
            }
            else if (string.IsNullOrEmpty(buttonLabel) && !string.IsNullOrEmpty(buttonUrl))
            {
                GUILayout.Label("Button text cannot be empty!", EditorStyles.boldLabel);
                canUpdate = false;
            }

            GUI.contentColor = Color.white;
        }

        private bool ButtonUrlIsValid(string url) => Uri.TryCreate(url, UriKind.Absolute, out Uri result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
#endif
