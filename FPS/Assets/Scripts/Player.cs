using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MultiplayerFPS
{
    public class Player : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private int maxHealth = 100;
        private int currentHealth;
        public float GetCurrentHealthPercentage()
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            return (float)currentHealth / maxHealth;
        }

        private bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
            protected set { isDead = value; }
        }

        private Collider col;

        [SerializeField] private List<Behaviour> disableOnDeath;
        [SerializeField] private List<GameObject> disableGameObjectsOnDeath;

        [SerializeField] private GameObject deathVFXPrefab;
        [SerializeField] private GameObject respawnVFXPrefab;

        public void Setup()
        {
            photonView.RPC("PunRPC_SetUp", RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void PunRPC_SetUp()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Debug.Log($"{transform.name} 執行");

            GameObject _respawnVFX = Instantiate(respawnVFXPrefab, transform.position, Quaternion.identity);
            Destroy(_respawnVFX, 3f);

            isDead = false;

            currentHealth = maxHealth;

            for (int i = 0; i < disableOnDeath.Count; i++)
            {
                disableOnDeath[i].enabled = true;
            }

            for (int i = 0; i < disableGameObjectsOnDeath.Count; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(true);
            }

            col = GetComponent<Collider>();
            if(col != null)
            {
                col.enabled = true;
            }

            if (photonView.IsMine)
            {
                GetComponent<PlayerSetup>().PlayerUIInstance.SetActive(true);

                GameMaster.Instace.SetSceneCameraActive(false);
            }
        }

        public void TakeDamage(int _amount)
        {
            if (isDead)
                return;

            currentHealth -= _amount;
            Debug.Log($"{transform.name} 受到 {_amount} 點傷害，剩餘血量 {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            Debug.Log($"{transform.name} 死亡");

            GameObject _deathVFX = Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(_deathVFX, 3f);

            for (int i = 0; i < disableOnDeath.Count; i++)
            {
                disableOnDeath[i].enabled = false;
            }

            for (int i = 0; i < disableGameObjectsOnDeath.Count; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(false);
            }

            if (col != null)
            {
                col.enabled = false;
            }

            if (photonView.IsMine)
            {
                GetComponent<PlayerSetup>().PlayerUIInstance.SetActive(false);

                GameMaster.Instace.SetSceneCameraActive(true);
            }

            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(3f);
            Debug.Log($"{transform.name} 重生");
            SetDefaults();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // 為玩家本人的狀態，將狀態更新給其他玩家
                stream.SendNext(currentHealth);
            }
            else
            {
                // 非為玩家本人的狀態，單純接收更新的資料
                currentHealth = (int)stream.ReceiveNext();
            }
        }
    }
}