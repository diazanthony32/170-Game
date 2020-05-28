using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFractured : MonoBehaviour
{
    public GameObject original;
    public GameObject fractured;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SpawnFracturedObj();
        }
    }

    public void SpawnFracturedObj() {
        //Destroy(original);
        DestroyImmediate(original, true);
        GameObject fractObj = Instantiate(fractured) as GameObject;
        fractObj.transform.GetChild(1).GetComponent<explode>().ExplodeObject();
    }
}
