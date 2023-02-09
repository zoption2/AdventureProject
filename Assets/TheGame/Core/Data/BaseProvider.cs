using TheGame.Utils;

namespace TheGame.Data
{
    public interface IDataUtils
    {
        int IDLength { get; }
        void Save();
        string GetUniqueID();
    }

    public abstract class BaseDataUtility : IDataUtils
    {
        public virtual int IDLength { get; } = 7;

        public virtual string GetUniqueID()
        {
            return  SupportUtility.GetUniqueKey(IDLength);
        }

        public virtual void Save()
        {
            GPrefs.Save();
        }
    }

    public abstract class BaseProvider<T> where T: IDataUtils, new()
    {
        protected T Utils { get; private set; }

        public BaseProvider()
        {
            Utils = new T();
        }
    }
}
    


