using Cinemachine;
using Pixelplacement;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private float intensity;

    private void Start()
    {
        CinemachineCore.CameraCutEvent.AddListener(OnCameraCut);
    }

    private void OnDestroy()
    {
        CinemachineCore.CameraCutEvent.RemoveListener(OnCameraCut);
    }

    public void OnCameraCut(CinemachineBrain brain)
    {
        ShakeCamera(intensity);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ShakeCamera(float strength, float duration = 1.0f)
    {
        Tween.Shake(transform, transform.localPosition, new Vector3(strength, strength, 0), duration, 0);
    }
}
