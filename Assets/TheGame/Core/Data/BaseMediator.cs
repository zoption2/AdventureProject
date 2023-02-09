using TheGame.Utils;
using Cysharp.Threading.Tasks;

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

    public abstract class BaseMediator<T> where T: IDataUtils, new()
    {
        protected T Utils { get; private set; }

        public BaseMediator()
        {
            Utils = new T();
        }

        public virtual async UniTask Initialize()
        {
            await UniTask.Delay(50);
        }
    }
}
    


