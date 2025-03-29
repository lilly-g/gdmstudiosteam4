using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private PlayerController playerController;
    private AudioManager audioManager;

    void Start(){
        playerController = GetComponent<PlayerController>();
        audioManager = GameObject.FindWithTag("Audio Manager").GetComponent<AudioManager>();
    }

    void Update(){

    }
}
