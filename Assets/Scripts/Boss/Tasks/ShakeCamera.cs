using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Cinemachine;

public class ShakeCamera : EnemyAction
{
    public float intensity;

    public override TaskStatus OnUpdate()
    {
        CameraShake.Instance.ShakeCamera(intensity);
        return TaskStatus.Success;
    }
}