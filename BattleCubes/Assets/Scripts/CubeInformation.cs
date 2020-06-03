using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInformation : MonoBehaviour {
    public AudioClip[] cubeRotationSounds;
    AudioSource cubeAudioSource;

    //Explosion VFX
    bool explode = false;
    public GameObject explosion;
    bool startDissolve = false;
    float dissolveVal = 0.0f;
    [SerializeField] float speed = 0.1f;

    //Impact effect
    bool impacted = false;
    float impactSpeed = 50f;
    float interval = 1;
    //float sv = 1;


    private void Start() {
        cubeAudioSource = GetComponent<AudioSource>();
        //SpawnCubeExplosion();
    }

    private void Update() {
        Dissolve();

        //if (Input.GetMouseButtonUp(0)) { StartCoroutine(impactLenght()); }
        ShowImpact();
    }

    public void PlayRandomRotateSound() {
        int rand = Random.Range(0, cubeRotationSounds.Length);
        cubeAudioSource.PlayOneShot(cubeRotationSounds[rand]);
    }

    public void ReColorCube(string player, string theme, string color) {

        for (int i = 0; i < 6; i++) {
            //this.transform.GetChild(i).GetComponent<MeshRenderer>();

            for (int j = 0; j < this.transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
                this.transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/" + theme + "/Colors/" + color + "/CUBE"));
                this.transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].shader = Resources.Load<Material>("Themes/" + theme + "/Colors/" + color + "/CUBE").shader;
            }

        }
    }
    public void SpawnCubeExplosion() {
        if (explosion != null && !explode) {
            explode = true;
            //print("explode!!");

            GameObject explosionFX = Instantiate(explosion) as GameObject;
            explosionFX.transform.position = transform.position + new Vector3(0, 0.55f, 0);
            explosionFX.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            explosionFX.transform.SetParent(transform);

            Destroy(explosionFX, 10);

            StartCoroutine(WaitToDissolve());
        }
    }
    void Dissolve() {
        if (startDissolve) {
            for (int i = 0; i < 6; i++) {
                dissolveVal += Time.deltaTime * speed;
                for (int j = 0; j < transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
                    transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].SetFloat("_Fade", dissolveVal);
                }
            }
        }
    }
    void ShowImpact() {
        if (impacted) {
            if (interval >= 1) {
                interval = 0;
                print("change color!!");
                Color impactColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);

                for (int i = 0; i < 6; i++) {
                    for (int j = 0; j < transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
                        transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].SetColor("Color_774AE0F8", impactColor);
                    }
                }
            }
            interval += impactSpeed * Time.deltaTime;
        }
    }
    IEnumerator WaitToDissolve() {
        yield return new WaitForSeconds(4);
        startDissolve = true;
        for (int i = 0; i < transform.childCount - 1; i++) {
            for (int j = 0; j < transform.GetChild(i).childCount; j++) {
                transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }
    }
    IEnumerator impactLenght() {
        impacted = true;
        yield return new WaitForSeconds(0.35f);
        impacted = false;

        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
                transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].SetColor("Color_774AE0F8", Color.black);
            }
        }
        //sv = 1;
    }

}

