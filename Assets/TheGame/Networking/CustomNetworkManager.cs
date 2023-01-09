using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheGame;
using System;

namespace Mirror
{
    public class CustomNetworkManager : NetworkManager
    {
        [SerializeField] private MatchController _matchController;
        private bool _isHostStarted;
        private bool _isClientConnected;

        public bool IsHostStarted => _isHostStarted;
        public bool IsClientConnected => _isClientConnected;

        public event Action OnSceneChanged;
        public event Action OnHostStarted;
        public event Action OnClientConnected;
        public event Action OnClientFailed;

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
            OnSceneChanged?.Invoke();
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            _isHostStarted = true;
            OnHostStarted?.Invoke();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }

        public override void OnClientNotReady()
        {
            base.OnClientNotReady();
            OnClientFailed?.Invoke();
        }

        public void InitMatch(Match match)
        {
            _matchController.Init(match);
        }
    }
}

