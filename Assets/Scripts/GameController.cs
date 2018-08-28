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
    public GameObject[] picturesL = new GameObject[5];
    public GameObject[] picturesM = new GameObject[5];
    public GameObject[] picturesR = new GameObject[5];
    public bool isRunning;
    public float t1;
    public float t2;
    public float t3;
    public float yEndPositionLeft;
    public float yEndPositionMid;
    public float yEndPositionRight;

    private List<Transform> spritesLeft = new List<Transform>();
    private List<Transform> spritesMid = new List<Transform>();
    private List<Transform> spritesRight = new List<Transform>();
    private float[] yStartPosition = new float[5] { 0, -2, -4, -6, -8 };

    public EndPositions<float> GetRandomValueOfYEndPos()
    {
        var initial = new List<EndPositions<float>>
        {
            new EndPositions<float> {Probability = 30 / 100.0, EndPosition = -100},
            new EndPositions<float> {Probability = 20 / 100.0, EndPosition = -102},
            new EndPositions<float> {Probability = 10 / 100.0, EndPosition = -104},
            new EndPositions<float> {Probability = 15 / 100.0, EndPosition = -106},
            new EndPositions<float> {Probability = 25 / 100.0, EndPosition = -108},
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
        for (int l = 0; l < picturesL.Length; l++)
        {
            spritesLeft.Add(picturesL[l].transform);
        }
        for (int m = 0; m < picturesM.Length; m++)
        {
            spritesMid.Add(picturesM[m].transform);
        }
        for (int r = 0; r < picturesR.Length; r++)
        {
            spritesRight.Add(picturesR[r].transform);
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
        t1 = 0;
        StartCoroutine("MoveReelsLeft");
        yield return new WaitForSeconds(1);
        t2 = 0;
        StartCoroutine("MoveReelsMid");
        yield return new WaitForSeconds(1);
        t3 = 0;
        StartCoroutine("MoveReelsRight");
    }

    /*public float CalculateT(float t)
    {
        float delta = TimeDeltaAsRandom();
        float c = 0.5f;
        if (t > c)
        {
            float tNext = Mathf.Sqrt((1 - t1) * (1 / c)) * delta/10;
            t += tNext;
        }
        else
        {
            t += delta/10;
        }
        return t;
    }*/
    public IEnumerator MoveReelsLeft()
    {
        yEndPositionLeft = GetRandomValueOfYEndPos().EndPosition;
        isRunning = true;

        while (t1 < 1)
        {
            float delta = TimeDeltaAsRandom();
            float c = 0.5f;
            if (t1 > c)
            {
                float tNext1 = Mathf.Sqrt((1 - t1) * (1 / c)) * delta / 2;
                t1 += tNext1;
            }
            else
            {
                t1 += delta / 2;
            }
            for (int l = 0; l < spritesLeft.Count; l++)
            {
                Vector3 startPos = new Vector3(spritesLeft[l].transform.position.x, yStartPosition[l], spritesLeft[l].transform.position.z);
                Vector3 endPos = new Vector3(spritesLeft[l].transform.position.x, yStartPosition[l] + yEndPositionLeft, spritesLeft[l].transform.position.z);
                spritesLeft[l].transform.position = Vector3.Lerp(startPos, endPos, t1);
                Vector3 fakePos = spritesLeft[l].transform.position;
                fakePos.y %= 10;
                spritesLeft[l].transform.position = fakePos;
            }
            yield return new WaitForSeconds(0.01f);
        }
        isRunning = false;
        RoundUpThePositionValueOfLeft();
    }

    public IEnumerator MoveReelsMid()
    {
        yEndPositionMid = GetRandomValueOfYEndPos().EndPosition;
        isRunning = true;
        while (t2 < 1)
        {
            float delta = TimeDeltaAsRandom();
            float c = 0.5f;
            if (t2 > c)
            {
                float tNext = Mathf.Sqrt((1 - t2) * (1 / c)) * delta / 2;
                t2 += tNext;
            }
            else
            {
                t2 += delta / 2;
            }
            for (int m = 0; m < spritesMid.Count; m++)
            {
                Vector3 startPos = new Vector3(spritesMid[m].transform.position.x, yStartPosition[m], spritesMid[m].transform.position.z);
                Vector3 endPos = new Vector3(spritesMid[m].transform.position.x, yStartPosition[m] + yEndPositionMid, spritesMid[m].transform.position.z);
                spritesMid[m].transform.position = Vector3.Lerp(startPos, endPos, t2);
                Vector3 fakePos = spritesMid[m].transform.position;
                fakePos.y %= 10;
                spritesMid[m].transform.position = fakePos;
            }
            yield return new WaitForSeconds(0.01f);
        }
        isRunning = false;
        RoundUpThePositionValueOfMid();
    }

    public IEnumerator MoveReelsRight()
    {
        yEndPositionRight = GetRandomValueOfYEndPos().EndPosition;
        isRunning = true;
        while (t3 < 1)
        {
            float delta = TimeDeltaAsRandom();
            float c = 0.5f;
            if (t3 > c)
            {
                float tNext = Mathf.Sqrt((1 - t3) * (1 / c)) * delta / 2;
                t3 += tNext;
            }
            else
            {
                t3+= delta / 2;
            }
            for (int r = 0; r < spritesRight.Count; r++)
            {
                Vector3 startPos = new Vector3(spritesRight[r].transform.position.x, yStartPosition[r], spritesRight[r].transform.position.z);
                Vector3 endPos = new Vector3(spritesRight[r].transform.position.x, yStartPosition[r] + yEndPositionRight, spritesRight[r].transform.position.z);
                spritesRight[r].transform.position = Vector3.Lerp(startPos, endPos, t3);
                Vector3 fakePos = spritesRight[r].transform.position;
                fakePos.y %= 10;
                spritesRight[r].transform.position = fakePos;
            }
            yield return new WaitForSeconds(0.01f);
        }
        isRunning = false;
        RoundUpThePositionValueOfRight();
    }

    private float TimeDeltaAsRandom()
    {
        return Random.Range(0.0111f, 0.01f);
    }

    public void RoundUpThePositionValueOfLeft()
    {
        for (int l = 0; l < spritesLeft.Count; l++)
        {
            float missalignedY = spritesLeft[l].transform.position.y;
            float alignedY = Mathf.RoundToInt(missalignedY);
            spritesLeft[l].transform.position = new Vector3(spritesLeft[l].transform.position.x, alignedY, spritesLeft[l].transform.position.z);
        }
    }

    public void RoundUpThePositionValueOfMid()
    {
        for (int m = 0; m < spritesMid.Count; m++)
        {
            float missalignedY = spritesMid[m].transform.position.y;
            float alignedY = Mathf.RoundToInt(missalignedY);
            spritesMid[m].transform.position = new Vector3(spritesMid[m].transform.position.x, alignedY, spritesMid[m].transform.position.z);
        }
    }

    public void RoundUpThePositionValueOfRight()
    {
        for (int r = 0; r < spritesRight.Count; r++)
        {
            float missalignedY = spritesRight[r].transform.position.y;
            float alignedY = Mathf.RoundToInt(missalignedY);
            spritesRight[r].transform.position = new Vector3(spritesRight[r].transform.position.x, alignedY, spritesRight[r].transform.position.z);
        }
    }
}



