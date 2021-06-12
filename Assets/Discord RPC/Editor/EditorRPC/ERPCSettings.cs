#if UNITY_EDITOR && UNITY_STANDALONE
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace ERPC
{
    [Serializable]
    public class ERPCSettings
    {
        private static string path = Directory.GetCurrentDirectory() + "/.ERPC";
        public string details;
        public string state;
        public string largeImageKey = "ab";
        public string largeImageText = "Arthur Blue";
        public string smallImageKey = "unity";
        public string smallImageText = "Made with Unity";
        public string firstButtonLabel = "Itch.io";
        public string firstButtonUrl = "https://tarsier.itch.io/";
        public string secondButtonLabel = "GitHub";
        public string secondButtonUrl = "https://github.com/fenwikk/unity-discord-rpc/";
        public bool resetOnSceneChange = false;
        public bool autoUpdate = true;
        public bool enableOnStartup = false;

        public ERPCSettings() { }

        public ERPCSettings(string details, string state, string largeImageKey, string largeImageText, string smallImageKey, string smallImageText,
            string firstButtonLabel, string firstButtonUrl, string secondButtonLabel, string secondButtonUrl, bool resetOnSceneChange, bool autoUpdate, bool enableOnStartup)
        {
            this.details = details;
            this.state = state;
            this.largeImageKey = largeImageKey;
            this.largeImageText = largeImageText;
            this.smallImageKey = smallImageKey;
            this.smallImageText = smallImageText;
            this.resetOnSceneChange = resetOnSceneChange;
            this.firstButtonLabel = firstButtonLabel;
            this.firstButtonUrl = firstButtonUrl;
            this.secondButtonLabel = secondButtonLabel;
            this.secondButtonUrl = secondButtonUrl;
            this.autoUpdate = autoUpdate;
            this.enableOnStartup = enableOnStartup;
        }

        public static void GetSettings()
        {
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ERPCSettings));
                FileStream stream = new FileStream(path, FileMode.Open);
                ERPCSettings settings = serializer.Deserialize(stream) as ERPCSettings;
                ApplySettings(settings);
                stream.Close();
            }
        }

        private static void ApplySettings(ERPCSettings settings)
        {
            ERPC.details = settings.details;
            ERPC.state = settings.state;
            ERPC.largeImageKey = settings.largeImageKey;
            ERPC.largeImageText = settings.largeImageText;
            ERPC.smallImageKey = settings.smallImageKey;
            ERPC.smallImageText = settings.smallImageText;
            ERPC.firstButtonLabel = settings.firstButtonLabel;
            ERPC.firstButtonUrl = settings.firstButtonUrl;
            ERPC.secondButtonLabel = settings.secondButtonLabel;
            ERPC.secondButtonUrl = settings.secondButtonUrl;
            ERPC.autoUpdate = settings.autoUpdate;
            ERPC.enableOnStartup = settings.enableOnStartup;
        }

        public static void SaveSettings()
        {
            ERPCSettings settings = new ERPCSettings(ERPC.details, ERPC.state, ERPC.largeImageKey, ERPC.largeImageText, ERPC.smallImageKey, ERPC.smallImageText, 
                ERPC.firstButtonLabel, ERPC.firstButtonUrl, ERPC.secondButtonLabel, ERPC.secondButtonUrl, ERPC.resetOnSceneChange, ERPC.autoUpdate, ERPC.enableOnStartup);

            XmlSerializer serializer = new XmlSerializer(typeof(ERPCSettings));
            var stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, settings);
            stream.Close();
        }
    }
}
#endif