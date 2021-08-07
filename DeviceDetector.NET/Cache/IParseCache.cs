namespace DeviceDetectorNET.Cache
{
    internal interface IParseCache
    {
        DeviceDetectorCachedData FindById(string key);
        void Upsert(string key, DeviceDetectorCachedData data);
    }
}