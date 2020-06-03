using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInformation : MonoBehaviour
{
    public AudioClip[] cubeRotationSounds;
    AudioSource cubeAudioSource;

    //Explosion VFX
    bool explode = false;
    public GameObject explosion;
    bool startDissolve = false;
    float dissolveVal = 0.0f;
    float speed = 0.1f;

    private void Start()
    {
		cubeAudioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (startDissolve) {
            for (int i = 0; i < 6; i++) {
                dissolveVal += Time.deltaTime * speed;
                for (int j = 0; j < transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
                    transform.GetChild(i).GetComponent<MeshRenderer>().materials[0].SetFloat("_Fade", dissolveVal);
                    transform.GetChild(i).GetComponent<MeshRenderer>().materials[1].SetFloat("_Fade", dissolveVal);
                }
            }
        }
    }

    public void PlayRandomRotateSound() {
        int rand = Random.Range(0, cubeRotationSounds.Length);
        cubeAudioSource.PlayOneShot(cubeRotationSounds[rand]);
    }

	public void ReColorCube(string player, string theme, string color)
	{

		for (int i = 0; i < 6; i++)
		{
			//this.transform.GetChild(i).GetComponent<MeshRenderer>();

			for (int j = 0; j < this.transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length; j++) {
				this.transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/"+ theme + "/Colors/" + color + "/CUBE"));
				this.transform.GetChild(i).GetComponent<MeshRenderer>().materials[j].shader = Resources.Load<Material>("Themes/" + theme + "/Colors/" + color + "/CUBE").shader;
			}

		}
	}
    public void SpawnCubeExplosion(string posTag) {
        if (explosion != null && !explode) {
            explode = true;
            //print("explode!!");

            GameObject spawnPos = GameObject.FindGameObjectWithTag(posTag);

            GameObject explosionFX = Instantiate(explosion) as GameObject;
            explosionFX.transform.position = spawnPos.transform.position + new Vector3(0, 0.55f, 0);
            explosionFX.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            explosionFX.transform.SetParent(spawnPos.transform);

            Destroy(explosionFX, 10);

            StartCoroutine(WaitToDissolve());
        }
    }
    IEnumerator WaitToDissolve() {
        yield return new WaitForSeconds(4);
        startDissolve = true;
    }
    //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demons/Colors/" + PlayerPrefs.GetString("CubeColor") + "Body");
}

