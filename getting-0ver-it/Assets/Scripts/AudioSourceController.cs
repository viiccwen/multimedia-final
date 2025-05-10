using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSourceController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    private static AudioSourceController instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void setVolume()
    {
        audioSource.volume = musicSlider.value;
    }
}
