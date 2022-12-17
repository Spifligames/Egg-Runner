
using UnityEngine;
using UnityEditor;

public class BigBrainWindow : EditorWindow
{
    Color color;
    Object obj1, obj2;
    GameObject object1;
    GameObject object2;

    [MenuItem("Window/BigBrainWindow")]
    public static void ShowWindow()
    {
        GetWindow<BigBrainWindow>("BigBrainWindow");
    }
    void OnGUI()
    {
        GUILayout.Label("Time for some BigBrain moves^^", EditorStyles.boldLabel);

        GUILayout.Space(20);

        //Colorize description and button
        GUILayout.Label("Colorize all selected Objects", EditorStyles.whiteLabel);

        color = EditorGUILayout.ColorField("Color", color);

        if (GUILayout.Button("Colorize!"))
        {
            ColorizeSelected();
        }

        //Copy and Paste rotation and Position
        GUILayout.Label("Position you want to be copied", EditorStyles.whiteLabel);

        obj1 = EditorGUILayout.ObjectField(obj1, typeof(GameObject), true);
        object1 = obj1 as GameObject;

        GUILayout.Label("GameObject to be adjusted", EditorStyles.whiteLabel);
        obj2 = EditorGUILayout.ObjectField(obj2, typeof(GameObject), true);
        object2 = obj2 as GameObject;
        if (GUILayout.Button("Copy Position + rotation"))
        {
            if (object1 != null && object2 != null)
            {
                object2.transform.position = object1.transform.position;
                object2.transform.rotation = object1.transform.rotation;
            }
            else Debug.LogWarning("ONE OR MORE OBJECTS IS MISSING!");
        }

        GUILayout.Space(15);

        //Brackeys help me!!!
        if (GUILayout.Button("Brackeys help!!!")) 
        {
            Application.OpenURL("https://www.youtube.com/user/Brackeys");
        }
    }
    void ColorizeSelected()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial.color = color;
                //renderer.material.color = color;
            }
        }
    }
}
