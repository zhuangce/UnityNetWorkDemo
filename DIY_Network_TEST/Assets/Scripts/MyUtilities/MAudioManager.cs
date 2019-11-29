using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAudioManager : MonoSingleton<MAudioManager> {

    private AudioSource source;

    private void Start()
    {
       Inital();
    }

    // Use this for initialization
    public void Inital () {
        if (source==null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
        source.playOnAwake = false ;
        source.spatialBlend = 1;
        source.loop = false;
        
	}
	
    public void PlaySoundEffectByPath(string path , float volume =1,ulong delay  = 0 )
    {
        source.volume = volume;
        AudioClip ac = Resources.Load<AudioClip>("SE/"+ path);
        source.clip = ac;
        if (source.isPlaying)
            source.Stop();
        source.Play(delay);


    }


	// Update is called once per frame
	void Update () {
		
	}
}
