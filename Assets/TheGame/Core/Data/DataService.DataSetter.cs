namespace TheGame.Data
{
    public interface IDataSetter
    {

    }

    public partial class DataService
    {
        private partial class DataSetter : IDataSetter
        {
            private DataService _service;

            public DataSetter(DataService service)
            {
                _service = service;
            }
        }
    }
}
    


