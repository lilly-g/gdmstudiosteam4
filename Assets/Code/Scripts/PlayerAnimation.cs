using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private string currentState;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    
    void Update(){
        UpdateAnimation();
    }

    private void SetAnimation(string state){
        if (state.Equals(currentState)){
            return;
        }

        animator.Play(state);
    }

    private void UpdateAnimation(){
        if (playerController.FrameInput.x == 0){
            SetAnimation("MCIdle");
        }
        else if (playerController.FrameInput.x > 0){
            SetAnimation("MCRun");
            spriteRenderer.flipX = false;
        }
        else{
            SetAnimation("MCRun");
            spriteRenderer.flipX = true;
        }
    }
}
