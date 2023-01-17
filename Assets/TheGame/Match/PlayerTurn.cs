using System.Collections.Generic;
using System.Threading.Tasks;


namespace TheGame
{

    public interface IPlayerTurn
    {
        void StartTurn();
    }

    public class PlayerTurn : IPlayerTurn
    {
        private int _moves;
        private int _actions;
        private PlayerController _mediator;
        private List<Actions> _availableActions;

        public PlayerTurn(PlayerController mediator, List<Actions> actions)
        {
            _mediator = mediator;
            _availableActions = actions;
        }

        public void StartTurn()
        {
            throw new System.NotImplementedException();
        }
    }
}

