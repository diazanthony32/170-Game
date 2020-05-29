using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInformation : MonoBehaviour
{

    public AudioClip[] cubeRotationSounds;
    AudioSource cubeAudioSource;


    // Start is called before the first frame update

    private void Start()
    {
		cubeAudioSource = GetComponent<AudioSource>();
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
			//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demons/Colors/" + PlayerPrefs.GetString("CubeColor") + "Body");
}

