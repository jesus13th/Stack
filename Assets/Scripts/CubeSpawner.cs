using UnityEngine;

public enum MoveDirection { X, Z }
public class CubeSpawner : MonoBehaviour {
    [SerializeField] private MovingCube pCube;
    [SerializeField] private MoveDirection moveDirection;

    public void SpawnCube() {
        var cube = Instantiate(pCube);
        if(GameManager.Instance.LastCube && GameManager.Instance.LastCube.gameObject != GameObject.Find("StartCube")) {
            cube.transform.position = new Vector3(
                moveDirection == MoveDirection.X ? transform.position.x : GameManager.Instance.LastCube.transform.position.x, 
                GameManager.Instance.LastCube.transform.position.y + pCube.transform.localScale.y, 
                moveDirection == MoveDirection.Z ? transform.position.z : GameManager.Instance.LastCube.transform.position.z
                );
        } else {
            cube.transform.position = transform.position;
        }
        cube.moveDirection = moveDirection;
    }
}