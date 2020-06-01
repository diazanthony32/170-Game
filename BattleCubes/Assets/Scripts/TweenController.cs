using System.Collections;
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

	RotateCube rotateCube = null;

	void Start()
	{
		if (gameObject.GetComponent<RotateCube>()) {
			rotateCube = gameObject.GetComponent<RotateCube>();
		}

		if (showFloating) {
			Float();
		}
		if (pulse) {
			PulseHighlight();
		}
		if (notify) {
			Notify();
		}
	}

	public void PopInUI()
	{

		if (gameObject.transform.localScale != new Vector3(0, 0, 0))
		{
			LeanTween.alpha(gameObject, 0, 0);
			LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0);
		}

		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		LeanTween.alpha(gameObject, 1, 0.025f).setDelay(delayTime);
		LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.05f);
	}

	public void PopInUIInfo(infoMenu infoMenu)
	{

		if (gameObject.transform.localScale != new Vector3(0, 0, 0))
		{
			LeanTween.alpha(gameObject, 0, 0);
			LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0);
		}

		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		LeanTween.alpha(gameObject, 1, 0.025f).setDelay(delayTime);
		LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.2f).setOnComplete(infoMenu.Pause);
	}

	public void HideUI()
	{
		LeanTween.alpha(gameObject, 0, 0.025f);
		LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.05f).setOnComplete(DisableGameObject);
	}

	public void Float()
	{
		LeanTween.moveY(gameObject, 0.1f, 4.0f).setEaseInOutSine().setLoopPingPong().setDelay(delayTime);
	}

	public void Pulse()
	{
		gameObject.GetComponent<Image>().color = Color.yellow;
		LeanTween.scale(gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.75f).setEaseInOutSine().setLoopPingPong();
	}

	public void PulseHighlight()
	{
		gameObject.GetComponent<Image>().color = Color.yellow;
		LeanTween.scale(gameObject, new Vector3(1.10f, 1.10f, 1.10f), 0.75f).setEaseInOutSine().setLoopPingPong();
	}

	public void Highlight()
	{
		LeanTween.cancel(gameObject);
		gameObject.GetComponent<Image>().color = Color.yellow;
		//LeanTween.scale(gameObject, new Vector3(1.10f, 1.10f, 1.10f), 0.5f).setEaseInOutSine().setLoopPingPong();
	}

	public void CancelPulseHighlight() {
		LeanTween.cancel(gameObject);
		gameObject.GetComponent<Image>().color = Color.gray;
		LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.0f).setOnComplete(plsStopTween);
	}

	public void CancelHighlight()
	{
		LeanTween.cancel(gameObject);
		//gameObject.GetComponent<Image>().color = Color.gray;

		var tempColor = gameObject.GetComponent<Image>().color;
		tempColor.a = 0f;
		gameObject.GetComponent<Image>().color = tempColor;

		LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.0f);
	}

	public void Notify()
	{

		//TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();

		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0);


		//LeanTween.alpha(gameObject, 1, 1f);
		LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.4f);

		LeanTween.scale(gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.5f).setEaseInOutSine().setLoopPingPong().setDelay(0.41f);

		//LeanTween.alpha(gameObject, 0, 1f).setDelay(2);
		LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.4f).setOnComplete(DisableGameObject).setDelay(3);

	}

	public void PopupNumber(){
		LeanTween.moveLocalY(gameObject.transform.GetChild(0).gameObject, 100.0f, 1.0f).setEaseInOutSine();
		LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0.0f, 0.5f).setEaseInOutSine().setDelay(1.0f);
		LeanTween.moveLocalY(gameObject.transform.GetChild(0).gameObject, -50.0f, 1.0f).setEaseInOutSine().setDelay(1.0f).setOnComplete(DestroyGameObject);
	}

	public void ScrollText(float time)
	{
		LeanTween.moveLocalX(gameObject.transform.GetChild(0).gameObject, 100.0f, time).setEaseInOutSine();
		LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0.0f, time/2).setEaseInOutSine().setDelay(time);
		LeanTween.moveLocalX(gameObject.transform.GetChild(0).gameObject, -50.0f, time).setEaseInOutSine().setDelay(time);
	}

	public void PulseTargets()
	{
		//gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
		LeanTween.cancel(gameObject);

		var tempColor = gameObject.GetComponent<MeshRenderer>().material.color;
		tempColor.a = 0.2f;
		gameObject.GetComponent<MeshRenderer>().material.color = tempColor;

		LeanTween.scale(gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEaseInOutSine().setLoopPingPong();
	}

	public void HighlightPlacementTargets()
	{
		//gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
		LeanTween.cancel(gameObject);

		var tempColor = gameObject.GetComponent<MeshRenderer>().material.color;
		tempColor.a = 0.2f;
		gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;

		LeanTween.scale(gameObject, new Vector3(0.0003f, 1f, 0.0003f), 0.5f).setEaseInOutSine().setLoopPingPong();
	}

	public void ResetPlacementTargets()
	{
		//gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
		LeanTween.cancel(gameObject);

		//tempColor = gameObject.GetComponent<MeshRenderer>().material.color;
		//tempColor.a = 0.2f;
		gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

		LeanTween.scale(gameObject, new Vector3(0.000323f, 1f, 0.000323f), 0.0f);
	}

	public void StopPulseTargets()
	{
		LeanTween.cancel(gameObject);

		var tempColor = gameObject.GetComponent<MeshRenderer>().material.color;
		tempColor.a = 0f;
		gameObject.GetComponent<MeshRenderer>().material.color = tempColor;

		LeanTween.scale(gameObject, new Vector3(.9f, .9f, .9f), 0.0f).setOnComplete(plsStopTween);
	}

	public void Rotate(Vector3 rotDir) {
        //LeanTween.rotateAroundLocal(gameObject, rotDir, 90, 1);
        //print(rotDir);
        LeanTween.rotateAround(gameObject, rotDir, 90, 0.6f).setEaseInOutSine();
		gameObject.GetComponent<CubeInformation>().PlayRandomRotateSound();

	}
	//public void RotateAndStore(Vector3 rotDir) {
	//    LeanTween.rotateAround(gameObject, rotDir, 90, 0.6f).setEaseInOutSine().setOnComplete(PushToStack);
	//}
	public void RotateBack(Quaternion rotation) {
		
		if (gameObject.transform.rotation.eulerAngles != rotation.eulerAngles)
		{
			gameObject.GetComponent<CubeInformation>().PlayRandomRotateSound();

		}

		LeanTween.rotateLocal(gameObject, rotation.eulerAngles, 0.6f).setEaseInOutSine();
		//gameObject.GetComponent<CubeInformation>().PlayRandomRotateSound();
	}
    //public void PushToStack() {
    //    rotateCube.PushToPlannedStack(transform.rotation);
    //}

    public void StopTween()
	{
		LeanTween.cancel(gameObject);
	}

	public void slideXOver(float x){
		LeanTween.moveX(gameObject, gameObject.transform.position.x+x, 0.5f).setEaseInOutSine();
	}

	public void slideEnemyUp(){
		LeanTween.moveY(gameObject, 0f, 2f).setEaseInOutSine().setOnComplete(ResetFloat);
	}

	void ResetFloat()
	{
		LeanTween.cancel(gameObject);
		Float();
	}

	void plsStopTween()
	{
		LeanTween.cancel(gameObject);
		//gameObject.SetActive(false);
	}

	void DisableGameObject()
	{
		LeanTween.cancel(gameObject);
		gameObject.SetActive(false);
	}

	void DestroyGameObject()
	{
		LeanTween.cancel(gameObject);
		Destroy(gameObject);
	}
}

