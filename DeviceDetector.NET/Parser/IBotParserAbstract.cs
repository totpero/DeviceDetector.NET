using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public interface IBotParserAbstract: IParserAbstract<BotMatchResult>
    {
        bool DiscardDetails { get; set; }
    }
}