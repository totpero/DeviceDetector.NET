namespace DeviceDetectorNET.Parser.Device
{
    public interface IDeviceParserAbstract: IParserAbstract
    {
        string GetModel();
        string GetBrand();
    }
}