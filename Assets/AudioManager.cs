using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource MusicMenu;
    [SerializeField] private AudioSource MusicLayer1;
    [SerializeField] private AudioSource MusicLayer2;
    [SerializeField] private AudioSource MusicLayer3;
    [SerializeField] private AudioSource AbilityPickup;
    [SerializeField] private AudioSource Death;
    [SerializeField] private AudioSource Footsteps;
    [SerializeField] private AudioSource Grapple;
    [SerializeField] private AudioSource Jump;
    [SerializeField] private AudioSource Dash;

    public AudioClip layer1;
    public AudioClip layer2;
    public AudioClip layer3;
    public AudioClip layer4;
    public AudioClip layer5;

    public AudioClip footsteps1;
    public AudioClip footsteps2;
    public AudioClip footsteps3;

    private Scene currentScene;
    private AudioClip musicToPlay;

    public AudioManager manager;

    // Music remains persistent between levels 
    // (This obviously does not work if the player does not start at level 1. Needs to be fixed)
    void Awake(){
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        MusicLayer1.clip = layer1;
        MusicLayer1.Play();
        MusicLayer1.loop = true;
    }

    void Update(){
        if (currentScene != SceneManager.GetActiveScene()){
            currentScene = SceneManager.GetActiveScene();
            
            if (currentScene.buildIndex == 0){
                musicToPlay = layer1;
            }
            else if (currentScene.buildIndex < 6){
                musicToPlay = layer2;
            }
            else if (currentScene.buildIndex < 12){
                musicToPlay = layer3;
            }
            else{
                musicToPlay = layer4;
            }

            if (musicToPlay != MusicLayer1.clip){
                MusicLayer1.clip = musicToPlay;
                MusicLayer1.Play();
                MusicLayer1.loop = true;
            }   
        }
    }

    public void playAbility(){
        AbilityPickup.Play();
    }

    public void playDeath(){
        Death.Play();
    }

    public void playFootsteps(){
        if (!Footsteps.isPlaying){
            if (Footsteps.clip == footsteps1){
                Footsteps.clip = footsteps2;
            }
            else if (Footsteps.clip == footsteps2){
                Footsteps.clip = footsteps3;
            }
            else{
                Footsteps.clip = footsteps1;
            }
            Footsteps.Play();
        }
    }

    public void playGrapple(){
        Grapple.Play();
    }

    public void playJump(){
        Jump.Play();
    }

    public void playDash(){
        Dash.Play();
    }
}
