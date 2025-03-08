using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainSceneChecker))]
public class MainSceneCheckerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainSceneChecker script = (MainSceneChecker)target;
        if (GUILayout.Button("�ε�� �� ������ ����"))
        {
            script.ClearMainSceneFlag();
        }
    }
}
