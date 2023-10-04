using Cinemachine;
using StarterAssets;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

    private StarterAssetsInputs startAssetsInputs;
    private ThirdPersonController thirdPersonController;

    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;
    private Animator animator;

    private void Awake()
    {
        startAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 999f, aimColliderMask))
        {
            mouseWorldPosition = hitInfo.point;
        }

        if (startAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f  ));

            thirdPersonController.SetNewSensitivity(aimSensitivity);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetNewSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 20f));
        }

        if (startAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - bulletSpawnPos.position).normalized;
            Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
            startAssetsInputs.shoot = false;
        }

    }
}
