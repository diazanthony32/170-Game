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

    bool changed = false;

    void Awake()
    {
        int rand = Random.Range(0, infoTextArray.Length);
        TextMeshProComponent.text = infoTextArray[rand];

        m_textRectTransform = TextMeshProComponent.GetComponent<RectTransform>();

        m_cloneTextObject = Instantiate(TextMeshProComponent) as TextMeshProUGUI;
        RectTransform cloneRectTransform = m_cloneTextObject.GetComponent<RectTransform>();
        cloneRectTransform.SetParent(m_textRectTransform);
        cloneRectTransform.anchorMin = new Vector2(0f, 0f);
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

            /*
            //print(m_textRectTransform.localPosition.x);
            if (m_textRectTransform.localPosition.x <= -m_textRectTransform.sizeDelta.x + 100 && !changed) 
            {
                print("new text");
                int rand = Random.Range(0, infoTextArray.Length);
                TextMeshProComponent.text = m_cloneTextObject.text;
                yield return new WaitForSeconds(0.05f);

                m_cloneTextObject.text = infoTextArray[rand];
                
                RectTransform cloneRectTransform = m_cloneTextObject.GetComponent<RectTransform>();
                cloneRectTransform.SetParent(m_textRectTransform);
                cloneRectTransform.anchorMin = new Vector2(0f, 0f);
                cloneRectTransform.localPosition = new Vector3(TextMeshProComponent.preferredWidth, 0, cloneRectTransform.position.z);
                cloneRectTransform.localScale = new Vector3(1, 1, 1);

                changed = true;
                
            }
            else if(m_textRectTransform.localPosition.x >= -20 && changed){
                print("reset");
                changed = false;
            }
            */

            //print(m_textRectTransform.sizeDelta.x);
            yield return null;
        }
    }

    //int rand = Random.Range(0, infoTextArray.Length);
    //TextMeshProComponent.text = infoTextArray[rand];

    // Update is called once per frame
    //void Update()
    //{
    //int rand = Random.Range(0, infoTextArray.Length);
    //scrollText.text = (infoTextArray[rand]);
    //scrollText.gameObject.GetComponent<TweenController>().ScrollText(scrollSpeed);
    //}
}
