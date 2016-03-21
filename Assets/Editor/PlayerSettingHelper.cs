using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerSettingHelper : Editor
{
    [InitializeOnLoad]
    public class KeyStoreHelper
    {
        static string m_key_store_path = "Assets//Editor//denachina.keystore";
        static string m_key_alias_name = "dena";
        static string m_key_store_pass = "denadena01";
        static string m_key_alias_pass = "denadena01";
#if UNITY_ANDROID
        static keyStoreHelper()
        {
            string path = Path.GetFullPath(m_key_store_path);
            PlayerSettings.Android.keystoreName = path;
            PlayerSettings.Android.keyaliasName = m_key_alias_name;
            PlayerSettings.Android.keystorePass = m_key_store_pass;
            PlayerSettings.Android.keyaliasPass = m_key_alias_pass;
        }
#endif
    }
}
