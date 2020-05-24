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
}
