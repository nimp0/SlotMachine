using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public GameObject[] pictures = new GameObject[15];
    public List<Transform> sprites = new List<Transform>();
    private float[] startPositionsOfSpritesY = new float[15] { 0, -2, -4, -6, -8, 0, -2, -4, -6, -8, 0, -2, -4, -6, -8 };
    public bool isRunning;
    public float t;

    public void Awake()
    {
        for (int i = 0; i < pictures.Length; i++)
        {
            sprites.Add(pictures[i].transform);
        }
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
        StartCoroutine(MoveReels(0));
        yield return new WaitForSeconds(1f);
        StartCoroutine(MoveReels(1));
        yield return new WaitForSeconds(1f);
        StartCoroutine(MoveReels(2));
        yield return new WaitForSeconds(1f);
	}
   
	public IEnumerator MoveReels(int order)
	{
		isRunning = true;
        t = 0;

        while (t < 1)
		{
            float c = 0.5f;
            if (t > c)
            {
                float t1 = Mathf.Sqrt((1 - t) * (1 / c)) * Time.deltaTime / 10;
                t += t1;
            }
            else
            {
                t += Time.deltaTime / 10;
            }

            foreach (int index in Enumerable.Range(order * 5, 5))
            {
                Vector2 startPos = new Vector2(sprites[index].position.x, startPositionsOfSpritesY[index]);
                Vector2 endPos = new Vector2(sprites[index].position.x, startPositionsOfSpritesY[index] - 100);
                sprites[index].transform.position = Vector2.Lerp(startPos, endPos, t);

                Vector2 fakePos = sprites[index].transform.position;
                fakePos.y %= 10;
                sprites[index].transform.position = fakePos;
            }
			yield return new WaitForSeconds(0.01f);
		}
		isRunning = false;
		RoundUpThePositionValue();
	}

    public void RoundUpThePositionValue()
    {
        for (int j = 0; j < sprites.Count; j++)
        {
            float missalignedY = sprites[j].transform.position.y;
            float alignedY = Mathf.RoundToInt(missalignedY);
            if (alignedY % 2 != 0)
            {
                sprites[j].transform.position = new Vector3(sprites[j].transform.position.x, alignedY - 1, sprites[j].transform.position.z);
            }

            else
            {
                sprites[j].transform.position = new Vector3(sprites[j].transform.position.x, alignedY, sprites[j].transform.position.z);
            }
        }
    }
}



