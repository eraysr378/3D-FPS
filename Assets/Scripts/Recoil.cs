
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private PlayerGun playerGun;
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private bool isScopeEnabled;

    void Start()
    {
        playerGun = GetComponentInParent<PlayerGun>();
    }

    void Update()
    {
       if(playerGun.GetGun() == null)
        {
            return;
        }
        isScopeEnabled = playerGun.IsScopeEnabled();
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, playerGun.GetGun().GetReturnSpeed() * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, playerGun.GetGun().GetSnappiness() * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void RecoilFire()
    {
        if(isScopeEnabled)
        {
            targetRotation += new Vector3(playerGun.GetGun().GetScopedRecoil().x, Random.Range(-playerGun.GetGun().GetScopedRecoil().y, playerGun.GetGun().GetScopedRecoil().y), Random.Range(-playerGun.GetGun().GetScopedRecoil().z, playerGun.GetGun().GetScopedRecoil().z));
        }
        else
        {
            targetRotation += new Vector3(playerGun.GetGun().GetHipfireRecoil().x, Random.Range(-playerGun.GetGun().GetHipfireRecoil().y, playerGun.GetGun().GetHipfireRecoil().y), Random.Range(-playerGun.GetGun().GetHipfireRecoil().z, playerGun.GetGun().GetHipfireRecoil().z));
        }
    }
}
