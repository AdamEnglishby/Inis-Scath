using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{

    [SerializeField] private float amount;
    [SerializeField] private Camera camera;

    private void LateUpdate()
    {
        Vector3 cameraPosition = this.camera.transform.position;
        var ourTransform = this.transform;
        ourTransform.position = new Vector3(
            cameraPosition.x / amount,
            cameraPosition.y / amount,
            ourTransform.position.z
        );
    }
}
