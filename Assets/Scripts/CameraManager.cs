using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    private float yOffset;

    void Start() {
        GameManager.OnCubeSpawned += MoveCamera;
        yOffset = transform.position.y;
    }
    private void OnDestroy() => GameManager.OnCubeSpawned -= MoveCamera;
    private void MoveCamera() {
        StopAllCoroutines();
        StartCoroutine(IMoveCamera()); }
    private IEnumerator IMoveCamera() {
        var targetPosition = new Vector3(transform.position.x, GameManager.Instance.CurrentCube.transform.position.y + yOffset, transform.position.z);
        while (transform.position != targetPosition) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * .75f);
            yield return 0;
        }
    }
}
