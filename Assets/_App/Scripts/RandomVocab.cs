using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomVocab : MonoBehaviour
{

	public GameObject rainbowPlane;

	void Start()
	{
		TextMeshPro mText = rainbowPlane.GetComponent<TextMeshPro>();


		Debug.Log("greenPlane " + mText);

	}

	// Update is called once per frame
	void Update()
	{
		TextMeshPro mText = rainbowPlane.GetComponent<TextMeshPro>();


		//Debug.Log("greenPlane ");

		mText.text = "Another word " + Random.Range(1, 100);

	}
}
