using UnityEngine;
using Photon.Pun;

namespace MultiplayerFPS
{
    [RequireComponent(typeof(WeaponManager))]
    public class PlayerShoot : MonoBehaviourPun
    {
        [SerializeField] private Camera cam;

        private WeaponManager weaponManager;
        private PlayerWeapon currentWeapon;

        private const string TARGET_TAG = "Player";
        [SerializeField] private LayerMask targetMask;

        private void Start()
        {
            if (cam == null)
            {
                Debug.LogError("沒有 Camera");
                this.enabled = false;
            }

            weaponManager = GetComponent<WeaponManager>();
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            currentWeapon = weaponManager.GetCurrentWeapon();
            if (currentWeapon.FireRate <= 0f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.FireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }
        }

        private void Shoot()
        {
            photonView.RPC("PunRPC_PlayMuzzleFlash", RpcTarget.All);

            RaycastHit _hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.Range, targetMask))
            {
                if (_hit.collider.tag == TARGET_TAG)
                {
                    Debug.Log("擊中:" + _hit.collider.name);
                    string _hitTargetName = _hit.collider.name;
                    photonView.RPC("PunRPC_Shoot", RpcTarget.All, _hitTargetName, currentWeapon.Damage);
                }

                photonView.RPC("PunRPC_InstantiateHitEffect", RpcTarget.All, _hit.point, _hit.normal);
            }
        }

        [PunRPC]
        private void PunRPC_Shoot(string _targetName, int _damage)
        {
            Debug.Log($"{transform.name} 擊中 {_targetName} !!，{_targetName} 遭受 {_damage} 點傷害");
            GameMaster.Instace.GetPlayer(_targetName).TakeDamage(_damage);
        }

        [PunRPC]
        private void PunRPC_PlayMuzzleFlash()
        {
            weaponManager.GetWeaponVFX().MuzzleFlash.Play();
        }

        [PunRPC]
        private void PunRPC_InstantiateHitEffect(Vector3 _pos, Vector3 _normal)
        {
            GameObject _hitEffect = Instantiate(weaponManager.GetWeaponVFX().HitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
            Destroy(_hitEffect, 1f);
        }
    }
}