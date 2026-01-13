using UnityEngine;

public class FuelPour : MonoBehaviour
{
    public ParticleSystem fuelStream;
    public Camera mainCam;
    public LayerMask groundLayer;
    public LayerMask holeLayer;
    public float pourSpeed = 20f;

    private bool isPouring = false;
    private GameManager gameManager;

    void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        MovePourPoint();

        if (Input.GetMouseButtonDown(0))
            StartPour();

        if (Input.GetMouseButtonUp(0))
            StopPour();
    }

    void MovePourPoint()
    {
        // マウス → Raycast
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 pos = hit.point + Vector3.up * 1.2f;
            transform.position = pos;

            transform.rotation = Quaternion.LookRotation(Vector3.down);
        }
    }

    void StartPour()
    {
        isPouring = true;
        Debug.Log("StartPour called");
        fuelStream.Play();
    }

    void StopPour()
    {
        isPouring = false;
        fuelStream.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("EngineHole"))
        {
            gameManager.AddFuel(pourSpeed * Time.deltaTime);
        }
    }
}
