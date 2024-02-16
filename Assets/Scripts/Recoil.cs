
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private PlayerWeapon playerWeapon;
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private bool isScopeEnabled;

    void Start()
    {
        playerWeapon = GetComponentInParent<PlayerWeapon>();
    }

    void Update()
    {
        Gun gun = playerWeapon.GetWeapon()?.GetComponent<Gun>();
        if (gun == null)
        {
            return;
        }
        isScopeEnabled = gun.IsScopeEnabled();
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gun.GetReturnSpeed() * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gun.GetSnappiness() * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void RecoilFire()
    {
        Gun gun = playerWeapon.GetWeapon().GetComponent<Gun>();

        if (isScopeEnabled)
        {
            targetRotation += new Vector3(gun.GetScopedRecoil().x, Random.Range(-gun.GetScopedRecoil().y, gun.GetScopedRecoil().y), Random.Range(-gun.GetScopedRecoil().z, gun.GetScopedRecoil().z));
        }
        else
        {
            targetRotation += new Vector3(gun.GetHipfireRecoil().x, Random.Range(-gun.GetHipfireRecoil().y, gun.GetHipfireRecoil().y), Random.Range(-gun.GetHipfireRecoil().z, gun.GetHipfireRecoil().z));
        }
    }
}
