using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSourceController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    private static AudioSource audioSource;

    void Awake()
    {
        if (audioSource != null && audioSource != this.GetComponent<AudioSource>())
        {
            Destroy(this.gameObject);
            return;
        }
        
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        audioSource.Play();
    }

    public void setVolume()
    {
        audioSource.volume = musicSlider.value;
    }
}
