using UnityEngine;

public class MovingCube : MonoBehaviour {
    public MoveDirection moveDirection { get; set; }
    [SerializeField] private float moveSpeed = 2;

    public void OnEnable() {
        GameManager.Instance.LastCube ??= GameObject.Find("StartCube").GetComponent<MovingCube>();
        GameManager.Instance.CurrentCube = this;
        GetComponent<MeshRenderer>().material.color = GameManager.Instance.ColorRandom;

        if (GameManager.Instance.LastCube != null && GameManager.Instance.CurrentCube != null)
            transform.localScale = new Vector3(GameManager.Instance.LastCube.transform.localScale.x, transform.localScale.y, GameManager.Instance.LastCube.transform.localScale.z);
    }
    private void Update() {
        if (!GameManager.Instance.isGameOver) {
            transform.position += (moveDirection == MoveDirection.X ? transform.right : transform.forward) * Time.deltaTime * moveSpeed;
            if (Mathf.Abs(moveDirection == MoveDirection.X ? transform.position.x : transform.position.z) > 4.0f)
                Lose();
        }
    }

    public void Stop() {
        if (GameManager.Instance.CurrentCube != GameManager.Instance.LastCube) {
            float hangOver = GetHangOver;
            var max = moveDirection == MoveDirection.X ? GameManager.Instance.LastCube.transform.localScale.x : GameManager.Instance.LastCube.transform.localScale.z;

            if (Mathf.Abs(hangOver) >= max)
                Lose();
            if (GameManager.Instance.LastCube != null && GameManager.Instance.CurrentCube != null && !GameManager.Instance.isGameOver) {
                SplitCube(hangOver);
                Debug.LogError($"asd{GameManager.Instance.isGameOver}");
            }

            GameManager.Instance.LastCube = this;
            this.enabled = false;
        }
    }
    private void Lose() {
        GameManager.Instance.isGameOver = true;
        GameManager.Instance.ShowPanelGame();
        GameManager.Instance.audioBackground.Stop();
    }
    private void SplitCube(float hangOver) {
        var direction = moveDirection == MoveDirection.X ? Vector3.right : Vector3.forward;
        float newSize = GetValueByDirection(GameManager.Instance.LastCube.transform.localScale, direction) - Mathf.Abs(hangOver);
        float fallingBlockSize = GetValueByDirection(GameManager.Instance.LastCube.transform.localScale, direction) - newSize;
        float newPosition = GetValueByDirection(GameManager.Instance.LastCube.transform.position, direction) + (hangOver / 2);

        transform.localScale = ReplaceVector(transform.localScale, newSize);
        transform.position = ReplaceVector(transform.position, newPosition);

        float cubeEdge = GetValueByDirection(transform.position, direction) + (newSize / 2f * Mathf.Sign(hangOver));
        float fallingBlockPosition = cubeEdge + fallingBlockSize / 2f * Mathf.Sign(hangOver);

        SpawnDropCube(fallingBlockPosition, fallingBlockSize);
    }
    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize) {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = ReplaceVector(transform.localScale, fallingBlockSize);
        cube.transform.position = ReplaceVector(transform.position, fallingBlockPosition);
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        Destroy(cube, 2.0f);
        GameManager.OnCubeSpawned();
    }
    private float GetHangOver => moveDirection == MoveDirection.X ? transform.position.x - GameManager.Instance.LastCube.transform.position.x : transform.position.z - GameManager.Instance.LastCube.transform.position.z;
    private float GetValueByDirection(Vector3 v, Vector3 direction) => maxValue(Vector3.Scale(v, direction));
    private float maxValue(Vector3 v1) => moveDirection == MoveDirection.X ? v1.x : v1.z;
    private Vector3 ReplaceVector(Vector3 v1, float newSize) => new Vector3(moveDirection == MoveDirection.X ? newSize : v1.x, v1.y, moveDirection == MoveDirection.Z ? newSize : v1.z);
}