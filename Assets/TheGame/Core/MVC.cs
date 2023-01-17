namespace TheGame
{
    public interface IView
    {
        void Show();
    }

    public interface IModel
    {

    }

    public interface IMediator<TView, TModel> where TView : IView where TModel : IModel
	{
		TView View { get; }
		TModel Model { get; }
	}
}


