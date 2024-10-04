using UnityEngine;

namespace MultiplayerFPS
{
    public class WeaponVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem muzzleFlash;
        public ParticleSystem MuzzleFlash
        {
            get { return muzzleFlash; }
        }

        [SerializeField] private GameObject hitEffectPrefab;
        public GameObject HitEffectPrefab
        {
            get { return hitEffectPrefab; }
        }
    }
}