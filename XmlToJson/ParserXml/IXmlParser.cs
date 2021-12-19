using Newtonsoft.Json.Linq;

namespace XmlToJson.ParserXml
{
    interface IXmlParser
    {
        void StartParserXml(string filepath);
        JArray GetCollectionJsonObject();
    }
}
