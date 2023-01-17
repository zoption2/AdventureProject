using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace TheGame
{
    public class NetworkPlayerView : NetworkBehaviour
    {
        [SerializeField] private GameObject _playerUi;
        [SerializeField] private GameObject _enemyUI;
        [SerializeField] private Slider _healthbar;
        [SerializeField] private Slider _enemyHealthbar;
        [SerializeField] private Button _doDamageButton;
        [SerializeField] private Button _getEnemyButton;
        [SerializeField] private Button _endTurnButton;

        private IPlayerController _mediator;

        private void OnEnable()
        {
            _doDamageButton.onClick.AddListener(DoDamage);
            _getEnemyButton.onClick.AddListener(SelectEnemy);
            _endTurnButton.onClick.AddListener(EndTurn);
            EnableEnemyHealthbar(false);
        }

        private void OnDisable()
        {
            _doDamageButton.onClick.RemoveListener(DoDamage);
            _getEnemyButton.onClick.RemoveListener(SelectEnemy);
            _endTurnButton.onClick.RemoveListener(EndTurn);
        }

        private void OnConnectedToServer()
        {
            _playerUi.SetActive(isLocalPlayer);
        }

        private void DoDamage()
        {

        }

        private void SelectEnemy()
        {
            EnableEnemyHealthbar(true);
        }

        private void EndTurn()
        {
            EnableEnemyHealthbar(false);
        }

        private void EnableEnemyHealthbar(bool isOn)
        {
            _enemyHealthbar.gameObject.SetActive(isOn);
        }
    }
}

