using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<List<Transform>> sprites;
    public GameObject loadedSprite;
    public float[] tParameters;
    public float[] xParameters;
    public bool isRunning;

    public int multiplier;
    
    int amountOfRows = 3;
    int amountOfSprites = 15;
    int amountOfLoadedSprites = 5;
    int[][] probabilities;

    private float[] yStartPositions;
    private float yEndPosition;

    public void Awake()
    {
        //amountOfSprites *= multiplier;
        amountOfRows *= multiplier;

        if (multiplier>1)
        {
            amountOfRows--;
        }
        
        probabilities = new int[amountOfRows][];

        for (int i = 0; i < amountOfRows; i++)
        {
            probabilities[i] = new int[amountOfLoadedSprites];
        }

        for (int i = 0; i < amountOfRows; i++)
        {
            probabilities[i][0] = 1 * multiplier;
            probabilities[i][1] = 2 * multiplier;
            probabilities[i][2] = 3 * multiplier;
            probabilities[i][3] = 4 * multiplier;
            probabilities[i][4] = 5 * multiplier;
        }

        yStartPositions = new float[amountOfSprites];

        for (int i = 0; i < amountOfRows; i++)
        {
            tParameters = new float[amountOfRows];
            xParameters = new float[amountOfRows];
        }

        SetSprites();
    }

    public void StartMoveCoroutine()
    {
        if (isRunning)
        {
            return;
        }

        else
        {
            StartCoroutine("Movement");
        }
    }

    public IEnumerator Movement()
    {
        for (int i = 0; i < amountOfRows; i++)
        {
            tParameters[i] = 0;
            xParameters[i] = 0;
            StartCoroutine(MoveReelByIndex(i));
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MoveReelByIndex(int index)
    {
        isRunning = true;

        List<Transform> row = sprites[index];
        Transform sprite = row[index];
        yEndPosition = -(Random.Range(0, amountOfSprites) * 3 + amountOfSprites * 20);
        float t = tParameters[index];
        float x = xParameters[index];

        while (t < 1 || x < 1)
        {
            float speed = .5f;
            float delta = 0.01f;
            float c1 = 0.5f;
           
            if (t > c1)
            {
                float tNext = Mathf.Sqrt((1 - t) * (1 / c1)) * delta * speed;
                t += tNext;
            }
            else if (t < c1)
            {
                float tNext = Mathf.Sqrt((0.01f + t) * (1 / c1)) * delta * speed * speed;
                t += tNext;
            }
            else
            {
                t += delta * speed;
            }

            if (t > 1)
            {
                t = 1;
            }

            if (t > 0.95f && x < 1 )
            {
                float xNext = Mathf.Sqrt(1 - x) * delta * 2;
                x += xNext;
            }
            if (x >= 1)
            {
                x = 1;
            }

            for (int i = 0; i < row.Count; i++)
            {
                Vector3 startPos = new Vector3(row[i].position.x, yStartPositions[i], row[i].position.z);
                Vector3 endPos = new Vector3(row[i].position.x, yStartPositions[i] + yEndPosition, row[i].position.z);
                row[i].position = Vector3.Lerp(startPos, endPos, t);
                row[i].position += Vector3.down * 3 * Mathf.Sin(x * Mathf.PI);
                Vector3 fakePos = row[i].position;
                fakePos.y %= amountOfSprites * 3;
                row[i].position = fakePos;
            }
            tParameters[index] = t;
            xParameters[index] = x;
            yield return new WaitForSeconds(0.01f);
        }
        tParameters[index] = t;
        xParameters[index] = x;
        isRunning = false;
        //RoundUpThePositionValueOfRows();
    }

    /*void RoundUpThePositionValueOfRows()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            List<Transform> row = sprites[i];

            for (int j = 0; j < row.Count; j++)
            {
                float missalignedY = row[j].position.y;
                float alignedY = Mathf.RoundToInt(missalignedY);
                //alignedY += alignedY % 2;
                row[j].position = new Vector3(row[j].position.x, alignedY, row[j].position.z);
            }
        }
    }*/

    void SetSprites()
    {
        List<GameObject> spritesFromResources = new List<GameObject>();

        for (int i = 0; i < amountOfLoadedSprites; i++)
        {
            loadedSprite = Resources.Load("Prefabs/" + i.ToString()) as GameObject;
            spritesFromResources.Add(loadedSprite);
        }

        sprites = new List<List<Transform>>();

        for (int i = 0; i < amountOfRows; i++)
        {
            List<Transform> row = new List<Transform>();

            for (int j = 0; j < amountOfSprites; j++)
            {
                int r;

                do
                {
                    r = Random.Range(0, amountOfLoadedSprites);
                }
                while (!CheckIfIndexIsAllowedForAdding(r, i));

                Transform spriteInstanceInScene = Instantiate(spritesFromResources[r]).transform;
                row.Add(spriteInstanceInScene);
            }

            sprites.Add(row);
        }

        for (int i = 0; i < amountOfRows; i++)
        {
            List<Transform> row = sprites[i];

            for (int j = 0; j < amountOfSprites; j++)
            {
                Transform sprite = row[j];
                sprite.position = new Vector3(i * 2, -j * 3, 10);
                yStartPositions[j] = sprite.position.y;
            }
        }
    }

    bool CheckIfIndexIsAllowedForAdding(int index, int rowIndex)
    {
        if (probabilities[rowIndex][index] == 0)
        {
            return false;
        }
        else
        {
            probabilities[rowIndex][index]--;
            return true;
        }
    }
}



