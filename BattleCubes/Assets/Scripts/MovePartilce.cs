using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePartilce : MonoBehaviour
{
    bool movingPositive = false;
    float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.forward, 360, 30);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
