#if UNITY_EDITOR
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

        public static bool button1IsValid = false;
        public static bool button2IsValid = false;

        public static bool error = false;

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

            #region Info

            GUILayout.Label("Status: " + status);

            GUILayout.Space(10);

            GUILayout.Label("Current Project: " + ERPC.projectName);
            GUILayout.Label("Current Scene: " + ERPC.sceneName);

            GUILayout.Space(20);

            GUILayout.Label("Editor Application ID");
            ERPC.applicationID = GUILayout.TextField(ERPC.applicationID);

            GUILayout.Space(10);

            #endregion

            #region Details/State

            customDetailsState = EditorGUILayout.ToggleLeft("Custom Details/State", customDetailsState);

            if (customDetailsState)
            {
                GUILayout.Label("Details");
                ERPC.details = GUILayout.TextField(ERPC.details);

                GUILayout.Label("State");
                ERPC.state = GUILayout.TextField(ERPC.state);
            }

            GUILayout.Space(10);

            #endregion

            #region Images

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

            #endregion

            #region Buttons

            CheckButtons();

            if (button1IsValid) GUILayout.Label("Button 1", EditorStyles.boldLabel);
            else GUILayout.Label("Button", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Text", "(e.g. \"Itch.io\" or \"Steam\")"), GUILayout.MaxWidth(30f));
            ERPC.button1Text = GUILayout.TextField(ERPC.button1Text, GUILayout.MaxWidth(120f));

            GUILayout.Label(new GUIContent("URL", "(e.g. Itch.io or Steam URL)"), GUILayout.MaxWidth(30f));
            ERPC.button1Url = GUILayout.TextField(ERPC.button1Url, GUILayout.MaxWidth(120f));
            GUILayout.EndHorizontal();

            ButtonErrors(ERPC.button1Text, ERPC.button1Url);

            GUILayout.Space(10);

            if (button1IsValid)
            {
                GUILayout.Label("Button 2", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Text", "(e.g. \"GitHub\")"), GUILayout.MaxWidth(30f));
                ERPC.button2Text = GUILayout.TextField(ERPC.button2Text, GUILayout.MaxWidth(120f));

                GUILayout.Label(new GUIContent("URL", "(e.g. GitHub URL)"), GUILayout.MaxWidth(30f));
                ERPC.button2Url = GUILayout.TextField(ERPC.button2Url, GUILayout.MaxWidth(120f));
                GUILayout.EndHorizontal();

                ButtonErrors(ERPC.button1Text, ERPC.button1Url);
            }

            GUILayout.Space(20);

            #endregion

            ERPC.resetOnSceneChange = EditorGUILayout.ToggleLeft("Reset timestap on scene change", ERPC.resetOnSceneChange);

            GUILayout.Space(10);

            if (EditorGUI.EndChangeCheck())
            {
                ERPC.lastEdit = EditorApplication.timeSinceStartup;
                status = "Updating...";
            }

            if (GUILayout.Button("Update Presence"))
            {
                ERPC.UpdateActivity();

                status = "Up to date";

                ERPC.lastEdit = 0f;
            }

            GUILayout.EndVertical();
        }

        public bool ButtonIsValid(string buttonText, string buttonUrl) => ButtonUrlIsValid(buttonUrl) && !string.IsNullOrEmpty(buttonText);

        public void CheckButtons()
        {
            button1IsValid = ButtonIsValid(ERPC.button1Text, ERPC.button1Url);
            if (button1IsValid)
            {
                button2IsValid = ButtonIsValid(ERPC.button2Text, ERPC.button2Url);
            }
            else
            {
                button2IsValid = false;
            } 
        }

        private void ButtonErrors(string buttonText, string buttonUrl)
        {
            GUI.contentColor = Color.red;

            if (!ButtonUrlIsValid(buttonUrl) && !string.IsNullOrEmpty(buttonText))
            {
                GUILayout.Label("URL is not valid!", EditorStyles.boldLabel);
            }
            else if (string.IsNullOrEmpty(buttonText) && !string.IsNullOrEmpty(buttonUrl))
            {
                GUILayout.Label("Button text cannot be empty!", EditorStyles.boldLabel);
            }

            GUI.contentColor = Color.white;
        }

        private bool ButtonUrlIsValid(string url) => Uri.TryCreate(url, UriKind.Absolute, out Uri result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
#endif
