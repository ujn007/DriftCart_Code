using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainSceneChecker))]
public class MainSceneCheckerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainSceneChecker script = (MainSceneChecker)target;
        if (GUILayout.Button("로드된 씬 데이터 삭제"))
        {
            script.ClearMainSceneFlag();
        }
    }
}
