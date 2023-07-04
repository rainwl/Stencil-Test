using UnityEngine;

public class PassClipPlaneData : MonoBehaviour
{
    public Transform clipPlane;
    public Material clipByPlaneMat;

    private void Update()
    {
        if (clipPlane == null || !clipByPlaneMat) return;
        clipByPlaneMat.SetVector("_PlanePos",clipPlane.position);
        clipByPlaneMat.SetVector("_PlaneNormal", clipPlane.up);
    }
}
