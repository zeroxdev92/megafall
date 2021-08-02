using System.Collections.Generic;
using System.Xml;
using Application.Scripts.Model;
using UnityEngine;

namespace Application.Scripts.Views.Managers
{
    public class LanguageManager : MonoBehaviour {

        public static LanguageManager instance;

        private Dictionary<string, string> stringsDict;

        void Awake()
        {
            if (instance != null)
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }

            if (string.IsNullOrEmpty(PlayerPrefs.GetString(Constants.Language.LANGUAGE)))
            {
                PlayerPrefs.SetString(Constants.Language.LANGUAGE, Constants.Language.LANG_ENG_KEY);
            }
        }

        void Start()
        {
            if (stringsDict == null)
            {
                ChangeLanguage(PlayerPrefs.GetString(Constants.Language.LANGUAGE));
            }
        }
    
        public void ChangeLanguage(string lang)
        {
            stringsDict = new Dictionary<string, string>();

            TextAsset stringsTA = Resources.Load<TextAsset>(string.Format("Strings/{0}/strings", lang));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(stringsTA.text);

        
            foreach (XmlNode node in doc.SelectNodes("strings/string"))
            {
                stringsDict.Add(node.Attributes["name"].Value, node.InnerXml);
            }

            PlayerPrefs.SetString(Constants.Language.LANGUAGE, lang);
        }

        public string GetText(string key)
        {
            if (!stringsDict.ContainsKey(key))
            {
                Debug.LogError(string.Format("String with key {0} not found!", key));
                return string.Empty;
            }

            return stringsDict[key];
        }
    }
}
