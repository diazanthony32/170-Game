using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextScrolling : MonoBehaviour
{ 

    public string[] infoTextArray;

    public TextMeshProUGUI TextMeshProComponent;
    public float scrollSpeed = 10;

    private TextMeshProUGUI m_cloneTextObject;

    private RectTransform m_textRectTransform;
    private string sourceText;
    private string tempText;

    // Use this for initialization
    void Awake()
    {

        int rand = Random.Range(0, infoTextArray.Length);
        TextMeshProComponent.text = infoTextArray[rand];

        m_textRectTransform = TextMeshProComponent.GetComponent<RectTransform>();

        m_cloneTextObject = Instantiate(TextMeshProComponent) as TextMeshProUGUI;
        RectTransform cloneRectTransform = m_cloneTextObject.GetComponent<RectTransform>();
        cloneRectTransform.SetParent(m_textRectTransform);
        cloneRectTransform.anchorMin = new Vector2(1, 0.5f);
        cloneRectTransform.localPosition = new Vector3(TextMeshProComponent.preferredWidth, 0, cloneRectTransform.position.z);
        cloneRectTransform.localScale = new Vector3(1, 1, 1);

        m_cloneTextObject.text = TextMeshProComponent.text;

    }

    private IEnumerator Start()
    {

        float width = TextMeshProComponent.preferredWidth;
        Vector3 startPosition = m_textRectTransform.localPosition;

        float scrollPosition = 0;

        while (true)
        {
            m_textRectTransform.localPosition = new Vector3(-scrollPosition % width, startPosition.y, startPosition.z);
            scrollPosition += scrollSpeed * 20 * Time.deltaTime;
            //print(scrollPosition);
            yield return null;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        //int rand = Random.Range(0, infoTextArray.Length);
        //scrollText.text = (infoTextArray[rand]);
        //scrollText.gameObject.GetComponent<TweenController>().ScrollText(scrollSpeed);
    //}
}
