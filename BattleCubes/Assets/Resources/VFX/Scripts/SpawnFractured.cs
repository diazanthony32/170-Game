using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFractured : MonoBehaviour
{
    public GameObject explosion;
    public Vector3 explosionOffset;
    public GameObject original;
    public GameObject fractured;
    public GameObject hellCube;

    float val = 0.0f;
    bool start = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SpawnFracturedObj();
            StartCoroutine(Disolve());
        }

        if (start && val < 1.0f) {
            val += 0.005f;

            for (int i = 6; i < 12; i++) {
                hellCube.transform.GetChild(i).GetComponent<MeshRenderer>().materials[0].SetFloat("_Fade", val);
                hellCube.transform.GetChild(i).GetComponent<MeshRenderer>().materials[1].SetFloat("_Fade", val);
            }
        }
    }

    IEnumerator Disolve() {
        yield return new WaitForSeconds(4);
        start = true;
    }

    public void SpawnFracturedObj() {

        if (explosion != null) {
            GameObject explosionFX = Instantiate(explosion) as GameObject;
            Destroy(explosionFX, 10);
        }

        //Destroy(original);
        //GameObject fractObj = Instantiate(fractured) as GameObject;
        //fractObj.transform.GetChild(1).GetComponent<explode>().ExplodeObject();
    }
}
