using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private string currentState;
    private Vector3 originalScale;
    private bool isSquashing;

    void Start(){
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    
    void Update(){
        UpdateAnimation();
        if (playerController.justHitGround){
            Debug.Log("hit the ground");
            isSquashing = true;
        }
        SquashandStretch();
    }

    private void SetAnimation(string state){
        if (state.Equals(currentState)){
            return;
        }

        animator.Play(state);
    }

    private void UpdateAnimation(){
        if (playerController.isWallSliding){
            SetAnimation("MCWall");
        }
        else if (!playerController._grounded){
            SetAnimation("MCJump");
        }
        else if (playerController.FrameInput.x == 0){
            SetAnimation("MCIdle");
        }
        else if (!playerController.isWallSliding){
            SetAnimation("MCRun");
        }
        else{
            SetAnimation("MCIdle");
        }

        if (playerController.facingRight){
            spriteRenderer.flipX = false;
        }
        else{
            spriteRenderer.flipX = true;
        }
    }

    private void SquashandStretch(){
        if (playerController._frameVelocity.y > 0){
            isSquashing = false;
            transform.localScale = new Vector3(originalScale.x, Mathf.MoveTowards(transform.localScale.y, originalScale.y * 1.2f, Time.fixedDeltaTime), 1);
        }
        else if (isSquashing){
            if (transform.localScale.y <= originalScale.y * 0.8f){
                isSquashing = false;
            }
            else{
                transform.localScale = new Vector3(originalScale.x, Mathf.MoveTowards(transform.localScale.y, originalScale.y * 0.8f, Time.fixedDeltaTime), 1);
            }
        }
        else{
            transform.localScale = new Vector3(originalScale.x, Mathf.MoveTowards(transform.localScale.y, originalScale.y, Time.fixedDeltaTime), 1);
        }
    }
}
