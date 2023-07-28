using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneMenuWindow : EditorWindow
{
    private string[] sceneNames;
    private int selectedSceneIndex;
    private int previousSceneIndex = -1;
    


    [MenuItem("Window/Scene Menu")]
    public static void ShowWindow()
    {
        GetWindow<SceneMenuWindow>("Scene Menu");
    }

    private void OnGUI()
    {
        //EditorGUILayout.LabelField("Drag scene files here:");
        //EditorGUILayout.Space();


        selectedSceneIndex = EditorGUILayout.Popup("Scenes", selectedSceneIndex, sceneNames);

        if (selectedSceneIndex != previousSceneIndex)
        {
            previousSceneIndex = selectedSceneIndex;
            OpenScene(selectedSceneIndex);
        }

        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetLastRect();

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        string scenePath = AssetDatabase.GetAssetPath(draggedObject);

                        if (scenePath.EndsWith(".unity"))
                        {
                            AddScene(scenePath);
                        }
                    }
                }

                break;
        }

        EditorGUILayout.Space();

        //做一个按钮，点击后将选中的场景从sceneNames中删除（并不删除文件本身）
        if (GUILayout.Button("Delete"))
        {
            DeleteScene(selectedSceneIndex);
            // if (sceneNames != null && sceneNames.Length > 0)
            // {
            //     string[] newSceneNames = new string[sceneNames.Length - 1];
            //     int j = 0;
            //     for (int i = 0; i < sceneNames.Length; i++)
            //     {
            //         if (i != selectedSceneIndextoDelete)
            //         {
            //             newSceneNames[j] = sceneNames[i];
            //             j++;
            //         }
            //     }
            //     sceneNames = newSceneNames;
            // }

        }
    }

    private void AddScene(string scenePath)
    {
        string sceneName = Path.GetFileNameWithoutExtension(scenePath);

        if (sceneNames == null)
        {
            sceneNames = new string[1];
            sceneNames[0] = sceneName;
        }
        else
        {
            string[] newSceneNames = new string[sceneNames.Length + 1];
            sceneNames.CopyTo(newSceneNames, 0);
            newSceneNames[sceneNames.Length] = sceneName;
            sceneNames = newSceneNames;
        }
    }

    private void OpenScene(int sceneIndex)
    {
        if (sceneNames != null && sceneIndex >= 0 && sceneIndex < sceneNames.Length)
        {
            string scenePath = AssetDatabase.FindAssets(sceneNames[sceneIndex] + " t:SceneAsset")[0];
            EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(scenePath));
        }
    }

    private void DeleteScene(int sceneIndex)
    {
        if (sceneNames != null && sceneNames.Length > 0)
        {
            string[] newSceneNames = new string[sceneNames.Length - 1];
            int j = 0;
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (i != sceneIndex)
                {
                    newSceneNames[j] = sceneNames[i];
                    j++;
                }
            }
            sceneNames = newSceneNames;
        }
    }
}