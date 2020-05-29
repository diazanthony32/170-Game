using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFractured : MonoBehaviour
{
    public GameObject explosion;
    public Vector3 explosionOffset;
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
        if (explosion != null) {
            GameObject explosionFX = Instantiate(explosion) as GameObject;
            Destroy(explosionFX, 3);
        }

        Destroy(original);
        GameObject fractObj = Instantiate(fractured) as GameObject;
        fractObj.transform.GetChild(1).GetComponent<explode>().ExplodeObject();
    }
}
