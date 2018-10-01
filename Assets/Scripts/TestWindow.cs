using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class TestWindow : EditorWindow
{
    private Sprite spriteToRender;
    private IEnumerable<GameObject> filteredObjects;
    private List<string> distinctiveNames;
    private Sprite loadedSprite;

    [MenuItem("Window/SpriteDetecter")]

    public static void ShowWindow()
    {
        GetWindow<TestWindow>("SpriteDetecter");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("CheckSprites"))
        {
            CheckMissingSprites();
        }

        if (GUILayout.Button("SetSprites"))
        {
            foreach (var item in Selection.gameObjects)
            {
                loadedSprite = Resources.Load("Sprites/" + item.name, typeof(Sprite)) as Sprite;
                SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sprite = loadedSprite;
                }
            }
            //Selection.gameObjects.Where(o => o.GetComponent<SpriteRenderer>() != null).ToList().ForEach(o => o.GetComponent<SpriteRenderer>().sprite = loadedSprite);
        }

        if (GUILayout.Button("DeleteSprites"))
        {
            Selection.gameObjects.Where(o => o.GetComponent<SpriteRenderer>() != null).ToList().ForEach(o => o.GetComponent<SpriteRenderer>().sprite = null);
        }
    }

    private void CheckMissingSprites()
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
            if (spriteToRender != null)
            {
                Debug.Log("All sprites are given");
            }
            return result;
        };

        filteredObjects = Selection.gameObjects.Where(predicate);
        distinctiveNames = filteredObjects.Select(o => o.name).Distinct().ToList();

        foreach (var distinctiveName in distinctiveNames)
        {
            Debug.Log(distinctiveName + " " + "has missing Sprite");
        }
    }

    
        
    
    

}

    
