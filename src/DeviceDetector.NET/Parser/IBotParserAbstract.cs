using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public interface IBotParserAbstract: IAbstractParser<BotMatchResult>
    {
        bool DiscardDetails { get; set; }
    }
}