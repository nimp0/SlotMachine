using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndPositions<T>
{
    public double Probability { get; set; }
    public T EndPosition { get; set; }
}

public class GameController : MonoBehaviour
{
    public GameObject[] pictures = new GameObject[15];
    public List<Transform> sprites = new List<Transform>();
    private float[] startPositionsOfSpritesY = new float[15] { 0, -2, -4, -6, -8, 0, -2, -4, -6, -8, 0, -2, -4, -6, -8 };
    public bool isRunning;
    public float t;
    public float yEndPosition;

    public EndPositions<float> GetRandomValueOfYEndPos()
    {
        var initial = new List<EndPositions<float>>
        {
            new EndPositions<float> {Probability = 30 / 100.0, EndPosition = -100},
            new EndPositions<float> {Probability = 25 / 100.0, EndPosition = -102},
            new EndPositions<float> {Probability = 20 / 100.0, EndPosition = -104},
            new EndPositions<float> {Probability = 15 / 100.0, EndPosition = -106},
            new EndPositions<float> {Probability = 10 / 100.0, EndPosition = -108},
        };

        var converted = new List<EndPositions<float>>(initial.Count);
        var sum = 0.0;
        foreach (var item in initial.Take(initial.Count - 1))
        {
            sum += item.Probability;
            converted.Add(new EndPositions<float> { Probability = sum, EndPosition = item.EndPosition });
        }
        converted.Add(new EndPositions<float> { Probability = 1.0, EndPosition = initial.Last().EndPosition });

        var random = new System.Random();
        var probability = random.NextDouble();
        var selected = converted.SkipWhile(i => i.Probability < probability).First();

        return selected;
    }

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
            yEndPosition = GetRandomValueOfYEndPos().EndPosition;
            StartCoroutine("Movement");
        }
    }

    public IEnumerator Movement()
    {
        t = 0;
        StartCoroutine(MoveReels(0));
        yield return new WaitForSeconds(1);
        StartCoroutine(MoveReels(1));
        yield return new WaitForSeconds(1);
        StartCoroutine(MoveReels(2));
    }

    public IEnumerator MoveReels(int order)
    {
        isRunning = true;
        while (t < 1)
        {
            float delta = TimeDeltaAsRandom();
            float c = 0.5f;
            if (t > c)
            {
                float t1 = Mathf.Sqrt((1 - t) * (1 / c)) * delta / 10;
                t += t1;
            }
            else
            {
                t += delta / 10;
            }
            foreach (int index in Enumerable.Range(order * 5, 5))
            {
                Vector2 startPos = new Vector2(sprites[index].transform.position.x, startPositionsOfSpritesY[index]);
                Vector2 endPos = new Vector2(sprites[index].transform.position.x, startPositionsOfSpritesY[index] + yEndPosition);
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

    private float TimeDeltaAsRandom()
    {
        return Random.Range(0.0111f, 0.01f);
    }

    public void RoundUpThePositionValue()
    {
        for (int j = 0; j < sprites.Count; j++)
        {
            float missalignedY = sprites[j].transform.position.y;
            float alignedY = Mathf.RoundToInt(missalignedY);
            sprites[j].transform.position = new Vector3(sprites[j].transform.position.x, alignedY, sprites[j].transform.position.z);
        }
    }
}



