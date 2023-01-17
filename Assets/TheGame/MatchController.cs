using Mirror;


namespace TheGame
{
    public class MatchController : NetworkBehaviour
    {
        private Match _match;
        public void Init(Match match)
        {
            _match = match;
            StartMatch();
        }

        private void StartMatch()
        {
            if (_match.IsRoundHasPlayers)
            {
                var player = _match.GetPlayerFromQueue();
                //player.StartTurn(StartMatch);
            }
            else
            {
                StartNewRound();
            }
        }

        private void StartNewRound()
        {
            _match.PrepareQueue();
            StartMatch();
        }
    }

    public interface IMatchObserver
    {
        void Notify();
    }

    public interface IObservableMatch
    {
        void AddObserver(IMatchObserver observer);
        void RemoveObserver(IMatchObserver observer);
    }
}
    


