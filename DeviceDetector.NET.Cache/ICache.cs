namespace DeviceDetectorNET.Cache
{
    public interface ICache
    {
        object Fetch(string id);
        bool Contains(string id);
        bool Save(string id,object data, int lifeTime = 0);
        bool Delete(string id);
        bool FlushAll();
    }
}