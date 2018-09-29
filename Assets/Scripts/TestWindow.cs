using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class TestWindow : EditorWindow
{
    Sprite sprite;
    IEnumerable<GameObject> filteredObjects;
    List<string> distinctiveNames;
    

    [MenuItem("Window/SpriteDetecter")]

    public static void ShowWindow()
    {
        GetWindow<TestWindow>("SpriteDetecter");
    }

    void OnGUI()
    {
        if (GUILayout.Button("CheckSprites"))
        {
            CheckMissingSprites();
        }

        if (GUILayout.Button("SetSprites"))
        {
            SetCorrectSprites();
        }
    }

    void CheckMissingSprites()
    {
        bool isEmpty = !Selection.gameObjects.Any();

        if (isEmpty)
        {
            Debug.Log("Select Game Objects!");
        }

        Func<GameObject, bool> predicate = obj =>
        {
            sprite = obj.GetComponent<SpriteRenderer>().sprite;
            bool result = sprite == null;
            return result;
        };

        filteredObjects = Selection.gameObjects.Where(predicate);

      
        distinctiveNames = filteredObjects.Select(r => r.name).Distinct().ToList();

        foreach (var name in distinctiveNames)
        {
            Debug.Log(name + " " + "has missing Sprite");
        }

        /*foreach (var item in filteredObjects)
        {
            Debug.Log(item.name + " " + "has missing Sprite");
        }*/
    }

    void SetCorrectSprites()
    {
        foreach (var filteredObject in filteredObjects)
        {
            sprite = filteredObject.GetComponent<SpriteRenderer>().sprite;
            sprite = Resources.Load<Sprite>("Pictures/background") as Sprite;
        }
    }
}

    
