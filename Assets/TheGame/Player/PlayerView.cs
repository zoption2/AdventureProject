using UnityEngine;

namespace TheGame
{

    public enum Character
    {
        Samurai
    }

    public interface IPlayerView : IView
    {

    }

    public class PlayerView : MonoBehaviour, IPlayerView
	{
        [SerializeField] private Transform _spellPoint;

        public void Show()
        {
            gameObject.SetActive(true);
        }
	}
}


