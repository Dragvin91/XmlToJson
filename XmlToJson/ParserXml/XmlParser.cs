using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using XmlToJson.Model;

namespace XmlToJson.ParserXml
{
    internal class XmlParser : IXmlParser
    {
        private string filePath;
        public void StartParserXml(string filepath)
        {
            filePath = filepath;
            ParserXml();
        }
        private void ParserXml()
        {
            var collectionSynchronousMachine = new List<SynchronousMachine>();
            var collectionVoltageLevel = new List<VoltageLevel>();
            var collectionSubstation = new List<Substation>();

            XmlDocument xmlDoc = new XmlDocument();
            string namespaceXml = "http://iec.ch/TC57/2014/CIM-schema-cim16#";
            xmlDoc.Load(filePath);

            GetSynchronousMachines(ref xmlDoc,ref namespaceXml, ref collectionSynchronousMachine);
            GetVoltageLevels(ref xmlDoc, ref namespaceXml, ref collectionVoltageLevel);
            GetSubstations(ref xmlDoc, ref namespaceXml, ref collectionSubstation);

            JArray collectionJsonObject = GetJsonObjects(collectionSubstation, collectionVoltageLevel, collectionSynchronousMachine);
        }
        
        private void GetSynchronousMachines(ref XmlDocument xmlDoc, ref string namespaceXml, ref List<SynchronousMachine> collectionSynchronousMachine)
        {
            foreach (XmlElement synchronousMachine in GetXmlNodeList(ref xmlDoc, ref namespaceXml, "SynchronousMachine"))
            {
                XmlNodeList elementsEqContainer = synchronousMachine.GetElementsByTagName("Equipment.EquipmentContainer", namespaceXml);
                collectionSynchronousMachine.Add(new SynchronousMachine { NameGenerator = GetNameObject(synchronousMachine, namespaceXml), Resource = elementsEqContainer[0].Attributes[0].InnerText });
            }
        }

        private void GetVoltageLevels(ref XmlDocument xmlDoc, ref string namespaceXml, ref List<VoltageLevel> collectionVoltageLevel)
        {           
            foreach (XmlElement voltageLevel in GetXmlNodeList(ref xmlDoc, ref namespaceXml, "VoltageLevel"))
            {
                XmlNodeList elementsSubstation = voltageLevel.GetElementsByTagName("VoltageLevel.Substation", namespaceXml);
                collectionVoltageLevel.Add(new VoltageLevel
                {
                    Name = GetNameObject(voltageLevel, namespaceXml),
                    Identification = voltageLevel.Attributes[0].InnerText,
                    SubstationResource = elementsSubstation[0].Attributes[0].InnerText
                });
            }
        }

        private void GetSubstations(ref XmlDocument xmlDoc, ref string namespaceXml, ref List<Substation> collectionSubstation)
        {
            foreach (XmlElement substation in GetXmlNodeList(ref xmlDoc, ref namespaceXml, "Substation"))
            {
                collectionSubstation.Add(new Substation { Name = GetNameObject(substation, namespaceXml), Identification = substation.Attributes[0].InnerText });
            }
        }

        private string GetNameObject(XmlElement xmlElement, string namespaceXml)
        {
            XmlNodeList elementsIdentificationName = xmlElement.GetElementsByTagName("IdentifiedObject.name", namespaceXml);
            return elementsIdentificationName[0].InnerText;
        }

        private XmlNodeList GetXmlNodeList(ref XmlDocument xmlDoc, ref string namespaceXml, string nametag)
        {
            var collectionXmlNodeList = xmlDoc.GetElementsByTagName(nametag, namespaceXml);
            return collectionXmlNodeList;
        }

        private JArray GetJsonObjects(IList<Substation> collectionSubstation, IList<VoltageLevel> collectionVoltageLevel, IList<SynchronousMachine> collectionSynchronousMachine)
        {
            var collectionJsonObject = new JArray();
            foreach (var substation in collectionSubstation)
            {
                foreach (var voltage in GetVoltageLevelForSubstation(ref collectionVoltageLevel, substation.Identification))
                {                  
                    var tempObject = new JObject();                   
                    tempObject[voltage.Name] = GetNameMachineForJobject(ref collectionSynchronousMachine, voltage.Identification);

                    var newObject = new JObject();
                    newObject[substation.Name] = tempObject;

                    collectionJsonObject.Add(newObject);
                }
            }
            return collectionJsonObject;
        }
        private IEnumerable<VoltageLevel> GetVoltageLevelForSubstation(ref IList<VoltageLevel> collectionVoltageLevel, string identification)
        {
            var collectionVoltage = collectionVoltageLevel.Where(voltage => voltage.SubstationResource == identification);
            return collectionVoltage;
        }

        private IEnumerable<SynchronousMachine> GetSynchronousMachinesForVoltageLevel(ref IList<SynchronousMachine> collectionSynchronousMachine, string identification)
        {
            var collectionSynchronous = collectionSynchronousMachine.Where(machine => machine.Resource == identification);
            return collectionSynchronous;
        }

        private JArray GetNameMachineForJobject(ref IList<SynchronousMachine> collectionSynchronousMachine, string identificator)
        {
            var synchronousMachine = new JArray();
            foreach (var machine in GetSynchronousMachinesForVoltageLevel(ref collectionSynchronousMachine, identificator))
            {
                synchronousMachine.Add(machine.NameGenerator);
            }
            return synchronousMachine;
        }
    }

}
