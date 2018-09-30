using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class TestWindow : EditorWindow
{
    Sprite spriteToRender;
    IEnumerable<GameObject> filteredObjects;
    List<string> distinctiveNames;
    Sprite loadedSprite;
    new string name;


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

        loadedSprite = Resources.Load("Sprites/" + name, typeof(Sprite)) as Sprite;

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
            spriteToRender = obj.GetComponent<SpriteRenderer>().sprite;
            bool result = spriteToRender == null;
            return result;
        };

        filteredObjects = Selection.gameObjects.Where(predicate);
        distinctiveNames = filteredObjects.Select(o => o.name).Distinct().ToList();

        foreach (var name in distinctiveNames)
        {
            Debug.Log(name + " " + "has missing Sprite");
        }
    }
   
    void SetCorrectSprites()
    {
        filteredObjects.Where(o => o.GetComponent <SpriteRenderer>() != null).ToList().ForEach(o => o.GetComponent<SpriteRenderer>().sprite = loadedSprite);
        /*foreach (var item in filteredObjects)
        {
            name = item.name;
            SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = loadedSprite;
            }
        }*/
    }
    
        
    
    

}

    
