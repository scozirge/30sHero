using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetBundleTool
{
    static string BuildPath = @"";
    public const string RESOURCE_PATH = "Assets/Resources/";


    [MenuItem("Assets/BuildSelectedTargetsToAssetBundles")]
    static void BuildSelectedTargets()
    {
        Object selectedObj = Selection.activeObject;
        Object[] selectedObjs = Selection.objects;
        Selection.activeObject = null;
        Selection.objects = new Object[0];
        int totalCount = selectedObjs.Length;
        int nowCount = 0;
        foreach (var resAsset in selectedObjs)
        {
            Selection.activeObject = resAsset;
            Selection.objects = new Object[] { resAsset };
            nowCount++;
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (EditorUtility.DisplayCancelableProgressBar("BuildAssetBundles", assetPath, (float)nowCount / (float)totalCount))
            {
                CleanSelectionLoadAsset();

                Selection.activeObject = selectedObj;
                Selection.objects = selectedObjs;
                EditorUtility.ClearProgressBar();
                return;
            }
            bool success = false;
            success = BuildAssetBundle(BuildTarget.StandaloneWindows64);
            CleanSelectionLoadAsset();

            if (!success)
            {
                Selection.activeObject = selectedObj;
                Selection.objects = selectedObjs;
                EditorUtility.ClearProgressBar();
                return;
            }
        }

        CleanSelectionLoadAsset();
        Selection.activeObject = selectedObj;
        Selection.objects = selectedObjs;
        EditorUtility.ClearProgressBar();
    }
    static bool BuildAssetBundle(BuildTarget _buildTarget)
    {
        //Path
        string assetBundlePath = Application.dataPath;
        BuildPath = assetBundlePath + "/../AssetBundles/" + _buildTarget.ToString() + "/";
        //BundleName
        var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string assetBundleName = "";
        string[] assetBundlePathArray = string.Copy(assetPath).Replace(RESOURCE_PATH, "").Split('/');
        for (int j = 0; j < assetBundlePathArray.Length - 1; j++)
        {
            if (j > 0)
            {
                assetBundleName += "/";
            }
            assetBundleName += assetBundlePathArray[j];
        }
        if (assetBundlePathArray.Length - 1 > 0)
        {
            assetBundleName += "/";
        }
        assetBundleName += Selection.activeObject.name;
        //AssetNames
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (selection.Length == 0)
        {
            Debug.LogWarning("沒有選取輸出AssetBundle物件");
            return false;
        }

        string[] assetNames = new string[selection.Length];
        for (int i = 0; i < selection.Length; i++)
        {
            Object obj = selection[i];

            string path = AssetDatabase.GetAssetPath(obj);
            assetNames[i] = path;
            AssetDatabase.SetLabels(obj, new string[1] { path });
        }


        //Build
        AssetBundleBuild assetBuild = new AssetBundleBuild();
        assetBuild.assetBundleName = assetBundleName;
        assetBuild.assetNames = assetNames;
        Debug.Log("assetBundleName=" + assetBundleName);
        for (int i = 0; i < assetNames.Length; i++)
        {
            Debug.Log(string.Format("assetNames[{0}]={1}", i, assetNames[i]));
        }
        BuildPipeline.BuildAssetBundles(BuildPath, new AssetBundleBuild[1] { assetBuild }, BuildAssetBundleOptions.ForceRebuildAssetBundle, _buildTarget);
        return true;
    }
    static void CleanSelectionLoadAsset()
    {
        Selection.activeObject = null;
        Selection.objects = new Object[0];

        AssetDatabase.SaveAssets();
        EditorUtility.UnloadUnusedAssetsImmediate();
    }

}