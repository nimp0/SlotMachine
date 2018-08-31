
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndPositions1<T>
{
    public double Probability { get; set; }
    public T EndPosition { get; set; }
}

public class LerpTest : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public float t = 0;
    public float x = 0;
    public float y;
    Vector2 cubePos;
    public GameObject cube;
    //private int[] endPositions = new int[3] { -102, -104, -106 };
    private int index;
    Vector3 endPos1;
    
    //public List<int> endPositions = new List<int>() { -102, -104, -106 };
    



    public EndPositions1<float> GetRandomValueOfYEndPos() 
    {

        var initial = new List<EndPositions1<float>>
        {
            new EndPositions1<float> {Probability = 30 / 100.0, EndPosition = -100},
            new EndPositions1<float> {Probability = 25 / 100.0, EndPosition = -102},
            new EndPositions1<float> {Probability = 20 / 100.0, EndPosition = -104},
            new EndPositions1<float> {Probability = 15 / 100.0, EndPosition = -106},
            new EndPositions1<float> {Probability = 10 / 100.0, EndPosition = -108},
        };

        var converted = new List<EndPositions1<float>>(initial.Count);
        var sum = 0.0;
        foreach (var item in initial.Take(initial.Count - 1))
        {
            sum += item.Probability;
            converted.Add(new EndPositions1<float> { Probability = sum, EndPosition = item.EndPosition });
        }
        converted.Add(new EndPositions1<float> { Probability = 1.0, EndPosition = initial.Last().EndPosition });

        var random = new System.Random();
        var probability = random.NextDouble();
        var selected = converted.SkipWhile(i => i.Probability < probability).First();

        return selected;
    }

    public void Awake()
    {
        float yEndPosition1 = GetRandomValueOfYEndPos().EndPosition;
        endPos1 = new Vector3(0, yEndPosition1, 0);
    }


    private void Update()
    {
        float currPosOfY = cube.transform.position.y;
        float c = .5f;
        float offset = 10f;
        

        //float offset = 10;

        if (t > c)
        {
            float t1 = Mathf.Sqrt((1 - t) * (1 / c)) * Time.deltaTime / 10;
            t += t1;
        }
       
        else
        {
            t += Time.deltaTime / 10;
        }

        if (t > 1)
        {
            t = 1;
        }
        //cube.transform.position = Vector3.Lerp(new Vector3(0,0,0), new Vector3(0,10,0), y);


        if (t >= 1 && x < 1)
        {
            x += 0.01f;
        }

        if (x >= 1)
        {
            x = 1;
        }
        /*if (y <= 0)
        {
            y = 0;
        }
        y = Mathf.Sin(x * Mathf.PI);


        Debug.Log(x);
        Debug.Log(y);
        cube.transform.position = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -10, 0), y);
        Vector3 position = cube.transform.position;*/
        cube.transform.position = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -10, 0), t);
        cube.transform.position += new Vector3(0, -1, 0) * Mathf.Sin(x * (Mathf.PI * 2));


        //cube.transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 10), y);

        /* cubePos = cube.transform.position;
         cubePos.y %= 10;
         cube.transform.position = cubePos;*/
    }

   
}

