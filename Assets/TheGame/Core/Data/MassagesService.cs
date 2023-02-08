namespace TheGame.Massager
{
    public interface IMassageService
    {
        void SendMassage(SystemMassage massage, object sender);

    }

    public enum SystemMassage
    {
        UserDataError
    }

    public class MassagesService : IMassageService
    {
        public void SendMassage(SystemMassage massage, object sender)
        {
            UnityEngine.Debug.LogFormat($"Massage: {massage} recieved from {nameof(sender)}");
        }
    }
}
    


