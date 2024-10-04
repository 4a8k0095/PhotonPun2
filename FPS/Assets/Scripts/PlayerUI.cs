using UnityEngine;

namespace MultiplayerFPS
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform thrusterFuelFill;

        [SerializeField] private RectTransform healthFill;

        private Player player;
        private PlayerController playerController;

        private void Update()
        {
            SetFuelAmount(playerController.ThrusterFuelAmount);
            SetHealthAmount(player.GetCurrentHealthPercentage());
        }

        public void SetPlayer(Player _player)
        {
            player = _player;
            playerController = _player.GetComponent<PlayerController>();
        }

        private void SetHealthAmount(float _amount)
        {
            healthFill.localScale = new Vector3(1f, _amount, 1f);
        }

        private void SetFuelAmount(float _amount)
        {
            thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
        }
    }
}