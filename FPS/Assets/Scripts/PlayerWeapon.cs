using UnityEngine;

namespace MultiplayerFPS
{
    [System.Serializable]
    public class PlayerWeapon
    {
        [SerializeField] private int damage = 10;
        public int Damage
        {
            get { return damage; }
        }

        [SerializeField] private float range = 100f;
        public float Range
        {
            get { return range; }
        }

        [SerializeField] private float fireRate = 0f;
        public float FireRate
        {
            get { return fireRate; }
        }

        [SerializeField] private GameObject weaponOBJ;
        public GameObject WeaponOBJ
        {
            get { return weaponOBJ; }
        }
    }
}