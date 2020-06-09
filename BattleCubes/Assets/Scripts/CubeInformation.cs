using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CubeInformation : MonoBehaviour {
    public AudioClip[] cubeRotationSounds;
    AudioSource cubeAudioSource;
    public AudioClip cubeExplosion;


    //Explosion VFX
    bool explode = false;
    public GameObject explosion;
    bool startDissolve = false;
    float dissolveVal = 0.0f;
    [SerializeField] float speed = 0.085f;

    //Impact effect
    Color originalColor;
    bool impacted = false;
    bool negativeColor = true;


    private void Start() {
        cubeAudioSource = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            originalColor = transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].GetColor("Color_76507EF6");

        }
        //originalColor = transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].GetColor("Color_76507EF6");
        //SpawnCubeExplosion();
    }

    private void Update() {
        Dissolve();

        //if (Input.GetMouseButtonUp(0)) { impacted = true; }
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
        originalColor = transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].GetColor("Color_76507EF6");
    }
    public void SpawnCubeExplosion() {
        if (explosion != null && !explode) {
            explode = true;
            //print("explode!!");

            GameObject explosionFX = Instantiate(explosion) as GameObject;
            explosionFX.transform.position = transform.position + new Vector3(0, 0.55f, 0);
            explosionFX.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            explosionFX.transform.SetParent(transform);

            cubeAudioSource.PlayOneShot(cubeExplosion);

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
            StartCoroutine(FlashCube());
        }
    }
    IEnumerator FlashCube() {
        impacted = false;
        for (int i = 0; i < 6; i++) {
            Color impactColor = originalColor;
            if (negativeColor) {
                impactColor *= 10;
                negativeColor = false;
            }
            else {
                impactColor = originalColor;
                negativeColor = true;
            }

            for (int j = 0; j < 6; j++) {
                for (int k = 0; k < transform.GetChild(j).GetComponent<MeshRenderer>().materials.Length; k++) {
                    transform.GetChild(j).GetComponent<MeshRenderer>().materials[k].SetColor("Color_76507EF6", impactColor);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
                transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].SetColor("Color_76507EF6", originalColor);
            }
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
    public void StartImpact() {
        impacted = true;
    }
    //public IEnumerator StartImpact() {
    //    impacted = true;
    //    yield return new WaitForSeconds(0.35f);
    //    //impacted = false;

    //    for (int i = 0; i < 6; i++) {
    //        for (int j = 0; j < transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
    //            //transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].SetColor("Color_774AE0F8", Color.black);
    //            transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].SetColor("Color_76507EF6", originalColor);
    //        }
    //    }
    //}
}

