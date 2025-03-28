using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioSource audioSource3;
    [SerializeField] private AudioSource audioSource4;
    [SerializeField] private AudioSource audioSource5;

    public AudioClip layer1;
    public AudioClip layer2;
    public AudioClip layer3;
    public AudioClip layer4;
    public AudioClip layer5;

    public AudioManager manager;

    // Music remains persistent between levels 
    // (This obviously does not work if the player does not start at level 1. Needs to be fixed)
    void Awake(){
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        // How do I play the music only when there is something placed in the variable?
        if (layer1 != null){
            audioSource1.clip = layer1;
            audioSource1.Play();
            audioSource1.loop = true;
        }
        if (layer2 != null){
            audioSource2.clip = layer2;
            audioSource2.Play();
            audioSource2.loop = true;
        }
        if (layer3 != null){
            audioSource3.clip = layer3;
            audioSource3.Play();
            audioSource3.loop = true;

        }
        if (layer4 != null)
        {
            audioSource4.clip = layer4;
            audioSource4.Play();
            audioSource4.loop = true;
        }
        if (layer5 != null)
        {
            audioSource5.clip = layer5;
            audioSource5.Play();
            audioSource5.loop = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
