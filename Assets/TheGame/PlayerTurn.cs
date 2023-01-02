using System.Collections.Generic;


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
        private PlayerMediator _mediator;
        private List<Actions> _availableActions;

        public PlayerTurn(PlayerMediator mediator, List<Actions> actions)
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

