#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using DiscordRPC;

namespace ERPC
{
    [InitializeOnLoad]
    public class ERPC
    {
        public static DiscordRpcClient Client { get; private set; }

        public static string projectName;
        public static string sceneName;

        public static string applicationID = "821629735229980702";

        public static string details;
        public static string state;
        public static string largeImageKey;
        public static string largeImageText;
        public static string smallImageKey;
        public static string smallImageText;

        public static DateTime start;

        public static bool resetOnSceneChange = true;

        public static DiscordPipe targetPipe = DiscordPipe.FirstAvailable;

        public static double lastEdit;

        /// <summary>
        /// All possible pipes discord can be found on.
        /// </summary>
        public enum DiscordPipe
        {
            FirstAvailable = -1,
            Pipe0 = 0,
            Pipe1 = 1,
            Pipe2 = 2,
            Pipe3 = 3,
            Pipe4 = 4,
            Pipe5 = 5,
            Pipe6 = 6,
            Pipe7 = 7,
            Pipe8 = 8,
            Pipe9 = 9
        }

        static ERPC()
        {
            projectName = Application.productName;
            sceneName = SceneManager.GetActiveScene().name;

            EditorApplication.update += Update;
            SceneManager.sceneLoaded += SceneLoaded;

            start = DateTime.UtcNow;

            if (!EditorApplication.isPlaying == false && Client == null) Initialize();
            ERPCSettings.GetSettings();
        }

        static void Update()
        {
            if (!EditorApplication.isPlaying && Client == null) Initialize();
            if (EditorApplication.isPlaying && Client != null) Deinitialize();

            projectName = Application.productName;
            sceneName = SceneManager.GetActiveScene().name;

            if (EditorApplication.timeSinceStartup >= lastEdit + 5 && EditorApplication.timeSinceStartup <= lastEdit + 10)
            {
                UpdateActivity();
                ERPCWindow.status = "Up to date";

                lastEdit = 0f;
            }
        }

        static void SceneLoaded(Scene scene = new Scene(), LoadSceneMode mode = new LoadSceneMode())
        {
            UpdateActivity();

            if (resetOnSceneChange)
            {
                start = DateTime.UtcNow;
            }
        }

        public static void Initialize()
        {
            Debug.Log("[ERP] Creating Client");
            Client = new DiscordRpcClient(
                applicationID,                                  //The Discord Application ID            
                pipe: (int)targetPipe,                          //The target pipe to connect too
                client: new DiscordRPC.Unity.UnityNamedPipe()   //The client for the pipe to use. Unity MUST use a NativeNamedPipeClient since its managed client is broken.
            );
            Client.Initialize();                            //Connects the client
            Debug.Log("[ERP] Client Initialized");

            UpdateActivity();
        }

        public static void Deinitialize()
        {
            Debug.LogError("[ERP] Disposing Discord IPC Client...");
            Client.Dispose();
            Client = null;
            Debug.Log("[ERP] Finished Disconnecting");
        }

        public static void UpdateActivity()
        {
            if (!ERPCWindow.customDetailsState)
            {
                details = projectName;
                state = sceneName;
            }

            Client.SetPresence(new RichPresence()
            {
                Details = details,
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = largeImageKey,
                    LargeImageText = largeImageText,
                    SmallImageKey = smallImageKey,
                    SmallImageText = smallImageText
                },
                Timestamps = new Timestamps
                {
                    Start = start
                }
            });
            Debug.Log("[ERP] Updated Presence");

            ERPCSettings.SaveSettings();
        }
    }
}
#endif
