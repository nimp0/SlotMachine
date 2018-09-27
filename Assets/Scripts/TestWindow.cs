using UnityEngine;
using UnityEditor;

public class TestWindow : EditorWindow {

    [MenuItem("Window/Example")]

    public static void ShowWindow()
    {
        GetWindow<TestWindow>("Example");
    }

	void OnGUI()
    {

    }
}
