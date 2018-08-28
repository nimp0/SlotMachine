
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
        float c = .5f;
        
        if (t > c)
        {
            float t1 = Mathf.Sqrt((1 - t) * (1 / c)) * Time.deltaTime / 10;
            t += t1;
        }
        else
        {
            t += Time.deltaTime / 10;
        }

       
 
         cube.transform.position = Vector3.Lerp(new Vector3(0,0,0), endPos1, t);
       

        cubePos = cube.transform.position;
        cubePos.y %= 10;
        cube.transform.position = cubePos;
    }
}

