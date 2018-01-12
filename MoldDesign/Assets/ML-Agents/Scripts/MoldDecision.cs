using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldDecision : MonoBehaviour, Decision{
    float[] randActs;
    int actSize;

	public float[] Decide (List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
	{
        actSize = gameObject.GetComponent<Brain>().brainParameters.actionSize;
        randActs = new float[actSize];

        for(int i = 0; i < actSize; i++)
        {
            randActs[i] = Random.Range(-1f,1f);
        }
        //return default(float[]);
        return randActs;

	}


	public float[] MakeMemory (List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
	{
		return default(float[]);
		
	}
}
