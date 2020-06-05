using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    UnitInformation unitInfo;
    //bool vulnerable;
    void Start()
    {
        unitInfo = GetComponent<UnitInformation>();

        StartCoroutine(CheckVulnerability());
    }

    void Update()
    {
        //if (vulnerable) {
        //    CheckVulnerability();
        //}
        //if (unitInfo) {
        //    if (unitInfo.CheckIfVulnerable()) {
        //        unitInfo.unitRenderer.materials[0].SetColor("Color_E68DA174", Color.black);
        //        //print("I AM VULNERABLE!!");
        //    }
        //    else {
        //        unitInfo.unitRenderer.materials[0].SetColor("Color_E68DA174", Color.white);
        //        //print("I AM INVULNERABLE!!");
        //    }
        //}
        //else {
        //    print("Unit info script not found");
        //}
    }
    //public void CheckVulnerable() {

    //}
    public IEnumerator CheckVulnerability() {
        yield return new WaitForSeconds(0.1f);

        if (unitInfo) {
            if (unitInfo.CheckIfVulnerable()) {
                unitInfo.unitRenderer.materials[0].SetColor("Color_E68DA174", Color.black);
                print("I AM VULNERABLE!!");
            }
            else {
                unitInfo.unitRenderer.materials[0].SetColor("Color_E68DA174", Color.white);
                print("I AM INVULNERABLE!!");
            }
        }
        else {
            print("Unit info script not found");
        }
    }
}
