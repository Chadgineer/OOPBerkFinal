using DG.Tweening;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float followDuration = 0.3f; 
    public Vector3 offset = new Vector3(0, 0, -10); 
    public Ease easeType = Ease.OutCubic; 

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + offset;
        transform.DOMove(targetPosition, followDuration).SetEase(easeType);
    }
}
