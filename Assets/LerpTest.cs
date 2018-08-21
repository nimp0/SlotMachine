using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
	public Transform A;
	public Transform B;
	public float t = 0;

	private void Update()
	{
		float c = .5f;
		if(t > c)
		{
			float t1=Mathf.Sqrt((1-t)*(1/ c)) *Time.deltaTime/10;
			t += t1;
		}
		else
		{
			t += Time.deltaTime/10;
		}


		if(t > 1)
		{
			t = 0;
		}

		transform.position = Vector3.Lerp(A.position, B.position, t)+ Vector3.up*10;

	}
}

