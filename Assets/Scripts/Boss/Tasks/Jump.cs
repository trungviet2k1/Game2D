using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class Jump : EnemyAction
{
    public float horizontalForce = 5f;
    public float maxDetectionRange = 10.0f;
    public float jumpForce = 20.0f;

    public float buildupTime;
    public float jumpTime;

    public string animationTriggerName;
    public bool shakeCameraOnLanding;
    public LayerMask playerLayer;
    public AudioClip concussionSound;
    public Hazard hazard;

    public bool HasLanded => hasLanded;
    private bool hasLanded;
    private bool isInRange;

    private Tween buildupTween;
    private Tween jumpTween;

    public override void OnStart()
    {
        if (player == null)
        {
            return;
        }

        Vector2 directionToPlayer = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, maxDetectionRange, playerLayer);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            buildupTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
            anim.SetTrigger(animationTriggerName);
        }
    }

    private void StartJump()
    {
        if (player != null)
        {
            var direction = player.transform.position.x < transform.position.x ? -1 : 1;
            rb2d.AddForce(new Vector2(horizontalForce * direction, jumpForce), ForceMode2D.Impulse);
        }

        jumpTween = DOVirtual.DelayedCall(jumpTime, () =>
        {
            hasLanded = true;
            if (shakeCameraOnLanding)
                CameraShake.Instance.ShakeCamera(0.2f);
            SoundManager.instance.PlaySound(concussionSound);

            if (hazard != null)
            {
                hazard.HasLanded = true;
            }
        }, false);
    }

    public override TaskStatus OnUpdate()
    {
        if (!hasLanded && player != null)
        {
            Vector2 directionToPlayer = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, maxDetectionRange, playerLayer);

            if (!isInRange)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
                {
                    isInRange = true;
                    buildupTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
                    anim.SetTrigger(animationTriggerName);
                }
                else
                {
                    isInRange = false;
                    buildupTween?.Kill();
                    jumpTween?.Kill();
                    hasLanded = false;
                }
            }
        }

        if (hasLanded)
        {
            hasLanded = false;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        isInRange = false;
        buildupTween?.Kill();
        jumpTween?.Kill();
        hasLanded = false;

        if (hazard != null)
        {
            hazard.HasLanded = false;
        }
    }
}