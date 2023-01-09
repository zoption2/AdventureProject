using UnityEngine;
using System.Linq;
using System;
using Mirror;
using Cysharp.Threading.Tasks;


namespace TheGame
{
    public interface IMatchService
    {
        Match GetMatch(string id);
        void FindMatch();
        void EndMatch(string id);
    }

    public class NetworkMatchService : NetworkBehaviour, IMatchService
    {
        [SerializeField] private CustomNetworkManager _networkManager;
        [SerializeField] private string _playSceneName ="";

        [SerializeField] private GameObject _findButton;
        [SerializeField] private GameObject _hostStartedText;
        [SerializeField] private GameObject _waitForPlayerText;

        private Matchmaker _matchmaker;

        public void EndMatch(string id)
        {
            throw new NotImplementedException();
        }

        public void FindMatch()
        {
            throw new NotImplementedException();
        }

        public Match GetMatch(string id)
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            _matchmaker = new Matchmaker(_networkManager);
        }




        [Serializable]
        private class Matchmaker
        {
            private const int kIdLength = 7;
            private const float kLowLevelRange = 0.5f;
            private const string kDedicatedServerName = "dedicatedserver";

            private readonly SyncList<Match> _networkMatches = new();
            private readonly SyncHashSet<Match> _availableMatches = new();
            private CustomNetworkManager _networkManager;

            public Matchmaker(CustomNetworkManager networkManager)
            {
                _networkManager = networkManager;
            }

            public Match CreateMatch()
            {
                var matchId = GetRandomMatchID();
                var match = new Match(matchId);
                _networkMatches.Add(match);
                return match;
            }

            public bool TryGetMatch(string id, out Match match)
            {
                if (_availableMatches.Any(x => x.ID == id))
                {
                    match = _availableMatches.First(x => x.ID == id);
                    return true;
                }
                else
                {
                    match = null;
                    return false;
                }
            }

            public void LaunchServer()
            {
                SetNetworkAddress(kDedicatedServerName);
                _networkManager.StartServer();
            }

            [Command]
            public void FindMatch(IPlayerMediator playerMediator)
            {
                if (TryFindMatchInPowerRange(playerMediator.Model.StatsGetter.TotalPower, kLowLevelRange, out Match match))
                {
                    if (completionSource.TrySetResult(match))
                    {
                        return completionSource.Task;
                    }
                }
            }

            [TargetRpc]
            public async void ConnectToMatch(NetworkConnection target, Match match)
            {
                UniTask<bool> connectToServer = ConnectToServer();
                await connectToServer;
                if (connectToServer.Status == UniTaskStatus.Succeeded)
                {

                }
            }

            public async UniTask<Match> FindMatch(IPlayerMediator playerMediator)
            {
                UniTaskCompletionSource<Match> completionSource = new();

                if (TryFindMatchInPowerRange(playerMediator.Model.StatsGetter.TotalPower, kLowLevelRange, out Match match))
                {
                    if (completionSource.TrySetResult(match))
                    {
                        return completionSource.Task;
                    }
                }
                else
                {
                    if (TryHostGame(out Match hostedMatch))
                    {

                    }
                }
                await UniTask.WaitUntil(() => _networkManager.IsHostStarted == true);
            }

            public void EndMatch(string id)
            {
                throw new System.NotImplementedException();
            }

            public void StartMatch()
            {
                //_roundService.CreateRound();
            }

            public bool TryHostGame(out Match match)
            {
                bool isHosted = false;
                match = null;

                if (!NetworkClient.active)
                {
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        match = CreateMatch();
                        SetNetworkAddress(match.ID);
                        _networkManager.OnHostStarted += OnHostStarted;
                        _networkManager.StartHost();
                        _networkManager.StartClient();
                        isHosted = true;
                    }
                }
                return isHosted;
            }

            private async UniTask<bool> ConnectToServer()
            {
                UniTaskCompletionSource<bool> completionSource = new();
                _networkManager.OnClientConnected += OnSuccess;
                _networkManager.OnClientFailed += OnFail;
                SetNetworkAddress(kDedicatedServerName);
                _networkManager.StartClient();
                await completionSource.Task;
                var waiter = completionSource.GetResult(0);
                return waiter;
                
                void OnSuccess()
                {
                    _networkManager.OnClientConnected -= OnSuccess;
                    completionSource.TrySetResult(true);
                }

                void OnFail()
                {
                    completionSource.TrySetResult(false);
                    _networkManager.OnClientFailed -= OnFail;
                }
            }

            public void JoinGame()
            {
                _networkManager.StartClient();
                //SetNetworkAddress();
            }

            public void FindGame(IPlayerMediator player)
            {
                Match match;

                if (_networkMatches.Count > 0)
                {
                    JoinGame();
                    match = GetMatch("000");
                    match.AddPlayer(player);
                    _networkManager.OnSceneChanged += SetMatch;

                }
                else
                {
                    if (TryHostGame(out Match hostedMatch))
                    {
                        hostedMatch.AddPlayer(player);
                    }
                    _networkManager.OnSceneChanged += SetMatch;
                }

                if (match.PlayersCount == 2)
                {
                   // _networkManager.ServerChangeScene(_playSceneName);
                }
                else
                {
                    //_waitForPlayerText.SetActive(true);
                }

                void SetMatch()
                {
                    _networkManager.OnSceneChanged -= SetMatch;
                    _networkManager.InitMatch(match);
                }


            }

            private void OnHostStarted()
            {
                _networkManager.OnHostStarted -= OnHostStarted;
                //_hostStartedText.gameObject.SetActive(true);
                //_findButton.gameObject.SetActive(false);
            }

            public bool TryFindMatchInPowerRange(float playerPower, float offsetValue, out Match match)
            {
                match = null;
                var selectedMatch = _availableMatches.First(x => playerPower.InRange(x.GetMiddlePower(), offsetValue));
                if (selectedMatch is not null)
                {
                    match = selectedMatch;
                    return true;
                }
                return false;
            }


            private void SetNetworkAddress(string address)
            {
                _networkManager.networkAddress = address;
            }

            private string GetRandomMatchID()
            {
                return DataUtils.GetUniqueKey(kIdLength);
            }
        }
    }
}
    


