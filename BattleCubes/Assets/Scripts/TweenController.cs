﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TweenController : MonoBehaviour
{

	[SerializeField] bool showFloating = false;
	[SerializeField] float delayTime = 0.025f;
	[SerializeField] bool pulse = false;

	[SerializeField] bool notify = false;

	void Start()
    {
        if(showFloating){
        	Float();
        }
        if(pulse){
        	PulseHighlight();
        }
        if(notify){
        	Notify();
        }
    }

	public void PopInUI()
	{
		if(!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		if(gameObject.transform.localScale != new Vector3(0, 0, 0))
		{
			LeanTween.alpha(gameObject, 0, 0);
			LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0);
		}

		LeanTween.alpha(gameObject, 1, 0.025f).setDelay(delayTime);
		LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.05f);
	}

	public void HideUI()
	{
		LeanTween.alpha(gameObject, 0, 0.025f);
		LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.05f).setOnComplete(DisableGameObject);
	}

	public void Float()
	{
		LeanTween.moveY(gameObject, 0.125f, 2.5f).setEaseInOutSine().setLoopPingPong().setDelay(delayTime);
	}

	public void Pulse()
	{
		LeanTween.scale(gameObject, new Vector3(1.10f, 1.10f, 1.10f), 0.5f).setEaseInOutSine().setLoopPingPong();
	}

	public void PulseHighlight()
	{
		gameObject.GetComponent<Image>().color = Color.yellow;
		LeanTween.scale(gameObject, new Vector3(1.10f, 1.10f, 1.10f), 0.5f).setEaseInOutSine().setLoopPingPong();
	}

	public void Notify()
	{

		//TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();

		if(!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0);
		

		//LeanTween.alpha(gameObject, 1, 1f);
		LeanTween.scale(gameObject, new Vector3(1.0f,1.0f,1.0f), 0.4f);

		LeanTween.scale(gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.5f).setEaseInOutSine().setLoopPingPong().setDelay(0.41f);

		//LeanTween.alpha(gameObject, 0, 1f).setDelay(2);
		LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.4f).setOnComplete(DisableGameObject).setDelay(3);

	}

	public void StopTween()
	{
		LeanTween.cancel(gameObject);
	}



	void DisableGameObject()
	{
		LeanTween.cancel(gameObject);
		gameObject.SetActive(false);
	}
}
