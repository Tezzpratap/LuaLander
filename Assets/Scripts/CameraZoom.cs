using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
	[SerializeField] private CinemachineCamera cinemachineCamera;
	[SerializeField] private Transform playerTransform;

	public static CameraZoom Instance { get; private set; }

	private float targetOrthographicSize = 25f;
	private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

	private void Awake()
    {
        Instance = this;
		if (cinemachineCamera != null && playerTransform != null) { 
			cinemachineCamera.Follow = playerTransform; 
		}
	}

	private void Update(){
        float zoomSpeed = 5f;
		cinemachineCamera.Lens.OrthographicSize = 
            Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,targetOrthographicSize, Time.deltaTime * zoomSpeed);
	}

    public void SetTargetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }

    public void SetNormalOrthographicSize(){
        SetTargetOrthographicSize(NORMAL_ORTHOGRAPHIC_SIZE);
	}
}
