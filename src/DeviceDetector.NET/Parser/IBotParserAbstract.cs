namespace DeviceDetectorNET.Parser
{
    public interface IBotParserAbstract: IParserAbstract
    {
        bool DiscardDetails { get; set; }
    }
}