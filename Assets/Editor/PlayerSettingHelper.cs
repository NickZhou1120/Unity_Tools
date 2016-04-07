using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerSettingHelper : Editor
{
    [InitializeOnLoad]
    public class KeyStoreHelper
    {
        static string m_key_store_path = "Assets//Editor//Test.keystore";
        static string m_key_alias_name = "test";
        static string m_key_store_pass = "test";
        static string m_key_alias_pass = "test";
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
