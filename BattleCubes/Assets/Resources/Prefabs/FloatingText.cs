using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI popupText;

    void Start()
    {
        //popupText = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        gameObject.GetComponent<TweenController>().PopupNumber();
    }

    public void SetText(string num) {
        popupText.text = num;
    }

}
