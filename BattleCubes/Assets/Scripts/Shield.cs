using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    UnitInformation unitInfo;
    bool shieldOn = false;
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
    public void ChangeMaterial() {
        if (shieldOn) {
            shieldOn = false;
            StartCoroutine(CheckVulnerability());
        }
    }
    public IEnumerator CheckVulnerability() {
        yield return new WaitForSeconds(0.1f);

        if (unitInfo) {
            Color tmpColor;

            if (unitInfo.CheckIfVulnerable()) {
                if (shieldOn) {
                    unitInfo.unitRenderer.materials[0].SetColor("Color_E68DA174", Color.black);

                    shieldOn = false;
                }
            }
            else {
                if (!shieldOn) {
                    //Get the particle system main, because other wise unity wont let u change it directly
                    GameObject particle = Instantiate(unitInfo.shieldParticle);
                    ParticleSystem ps = particle.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule main = ps.main;

                    //Get color and transform it to HSV to adjust its saturation
                    tmpColor = unitInfo.unitRenderer.materials[0].GetColor("Color_EC96F719");
                    Color.RGBToHSV(tmpColor, out float h, out float s, out float v);
                    tmpColor = Color.HSVToRGB(Mathf.Min(h, 1), Mathf.Min(s, 0.75f), Mathf.Min(v, 1));
                    main.startColor = tmpColor;

                    //Positioning the particle system
                    particle.transform.position = transform.position;
                    particle.transform.rotation = transform.rotation;
                    particle.transform.SetParent(transform);
                    Destroy(particle, 1.1f);

                    //Fade in the shield
                    tmpColor = Color.black;
                    for (int i = 0; i < 10; i++) {
                        tmpColor.r += 0.1f;
                        tmpColor.g += 0.1f;
                        tmpColor.b += 0.1f;

                        unitInfo.unitRenderer.materials[0].SetColor("Color_E68DA174", tmpColor);
                        yield return new WaitForSeconds(0.1f);
                    }
                    shieldOn = true;
                }
            }
        }
        else {
            print("Unit info script not found");
        }
    }
}
