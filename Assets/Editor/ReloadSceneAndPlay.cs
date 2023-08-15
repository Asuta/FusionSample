using UnityEditor;
using UnityEngine;

public class RefreshResources : Editor
{
    [MenuItem("Custom/Refresh Resources %'")]
    private static void Refresh()
    {
        // 刷新资源
        AssetDatabase.Refresh();
        // 编辑器运行
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}