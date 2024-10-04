using UnityEngine;
using Photon.Pun;

namespace MultiplayerFPS
{
    public class WeaponManager : MonoBehaviourPun
    {
        private string weaponLayerName = "Weapon";

        [SerializeField] private Transform weaponHolder;

        [SerializeField] private PlayerWeapon primaryWeapon;

        private PlayerWeapon currentWeapon;

        private WeaponVFX weaponVFX;

        private void Awake()
        {
            EquipWeapon(primaryWeapon);
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public WeaponVFX GetWeaponVFX()
        {
            return weaponVFX;
        }

        public void EquipWeapon(PlayerWeapon _weapon)
        {
            currentWeapon = _weapon;

            GameObject weaponInstance = Instantiate(_weapon.WeaponOBJ, weaponHolder.position, _weapon.WeaponOBJ.transform.rotation);
            weaponInstance.transform.SetParent(weaponHolder);

            weaponVFX = weaponInstance.GetComponent<WeaponVFX>();

            if (photonView.IsMine)
                SetWeaponLayer(weaponInstance);
        }

        private void SetWeaponLayer(GameObject _obj)
        {
            if (_obj == null)
                return;

            _obj.layer = LayerMask.NameToLayer(weaponLayerName);

            foreach (Transform _child in _obj.transform)
            {
                if (_child == null)
                    continue;

                SetWeaponLayer(_child.gameObject);
            }
        }
    }
}