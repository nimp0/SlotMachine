using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public GameObject[] pictures = new GameObject[15];
    private List<Transform> sprites = new List<Transform>();
    public bool isRunning;
    public float timeRunning;
    public float speed;

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
		speed = 1f;
		isRunning = true;
		timeRunning = 10;

		float deltaTime = Time.deltaTime * speed;

		while(timeRunning > 0)
		{
			foreach(int index in Enumerable.Range(order * 5, 5))
			{
				Transform sprite = sprites[index];

				sprite.Translate(-Vector2.up);

				Vector2 startPos = new Vector2(sprites[index].position.x, sprites[index].transform.position.y);
				Vector2 endPos = new Vector2(sprites[index].transform.position.x, -20);
				float distance = Vector2.Distance(startPos, endPos);
				//float distance = -movementVector.y;
				//float currTimeRunning = timeRunning - deltaTime * 0.01f;
				//float currDistance = distance - (deltaTime * 0.01f);
				//t = currTimeRunning * deltaTime / currDistance;
				float t = timeRunning * deltaTime / distance;
				sprites[index].transform.position = Vector2.Lerp(startPos, endPos, t);
				//sprites[index].transform.Translate(movementVector * deltaTime * t);
			}
			Teleport();
			timeRunning -= deltaTime;
			yield return new WaitForSeconds(deltaTime * 0.01f);
		}
		//isRunning = false;
		RoundUpThePositionValue();
	}

    public void Teleport()
    {
        for (int j = 0; j < sprites.Count; j++)
        {
            if (sprites[j].transform.position.y <= -6f)
            {
                Vector2 currentPosition = sprites[j].transform.position;
                currentPosition.y = (currentPosition.y + 10f);
                sprites[j].transform.position = currentPosition;
            }
        }
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



