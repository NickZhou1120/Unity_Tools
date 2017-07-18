using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;

public static class TextureSeprateAlpha
{
    //是否分离alpha
    public static bool WetherSeparate = false;

    /// <summary>
    /// 设置Texture格式
    /// </summary>
    /// <param name="texture"></param>
    public static void SetTextureToARGB(Texture texture)
    {
        if (texture == null)
            return;
        string assetpath = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(assetpath);
        if (importer != null)
        {
			importer.SetPlatformTextureSettings("Android", 4096, TextureImporterFormat.ARGB32,100,true);
			importer.SetPlatformTextureSettings("iphone", 4096, TextureImporterFormat.ARGB32,100,true);
        }
        AssetDatabase.ImportAsset(assetpath, ImportAssetOptions.ForceUpdate);


    }


    /// <summary>
    /// 分离alpha
    /// </summary>
    /// <param name="srcTexture"></param>
    public static void SeparateTexture(Texture src, out Texture rgb, out Texture a, bool alphaHalfSize = true)
    {
        rgb = null;
        a = null;

        if (src == null)
            return;
        string srcPath = AssetDatabase.GetAssetPath(src);
        string dirName = Path.GetDirectoryName(srcPath);
        string fileName = Path.GetFileNameWithoutExtension(srcPath);
        string extName = Path.GetExtension(srcPath);
        string alphaPath = string.Format("{0}/{1}_alpha{2}", dirName, fileName, extName);

        AssetDatabase.DeleteAsset(alphaPath);
        AssetDatabase.CopyAsset(srcPath, alphaPath);
        AssetDatabase.Refresh();

        int rgbSize = Mathf.Max(src.width, src.height, 32);
        int alphaSize = alphaHalfSize ? Mathf.Max(rgbSize / 2, 32) : rgbSize;

        SetTextureSetting(srcPath, rgbSize, TextureImporterFormat.ETC_RGB4, TextureImporterFormat.PVRTC_RGB4);
        SetTextureSetting(alphaPath, alphaSize, TextureImporterFormat.Alpha8, TextureImporterFormat.Alpha8);

    }

    /// <summary>
    /// 设置图片格式
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="maxSize"></param>
    /// <param name="androidFormat"></param>
    /// <param name="iosFormat"></param>
    static void SetTextureSetting(string assetPath, int maxSize, TextureImporterFormat androidFormat, TextureImporterFormat iosFormat)
    {
        var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
        {
            importer.textureType = TextureImporterType.Default;
            importer.npotScale = TextureImporterNPOTScale.ToNearest;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Bilinear;
            importer.anisoLevel = 0;
            importer.SetPlatformTextureSettings("Android", maxSize, androidFormat, 100,true);
            importer.SetPlatformTextureSettings("iPhone", maxSize, iosFormat, 100,true);
        }
        AssetDatabase.ImportAsset(assetPath);
    }
}
