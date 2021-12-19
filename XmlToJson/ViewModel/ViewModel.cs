using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;
using XmlToJson.ParserXml;

namespace XmlToJson
{
    class ViewModel
    {
        private RelayCommand _fileDialogCommand;
        private IXmlParser xmlParser;
        public RelayCommand FileDialogCommand => _fileDialogCommand ?? (_fileDialogCommand = new RelayCommand(OpenFileDialog, null));

        private void OpenFileDialog()
        {
            MessageBox.Show("Выберете файл Example.xml , прилагался к заданию", "Внимание");
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Xml Files|*.xml;";
            dialog.ShowDialog();
            var filePath = dialog.FileName;
            if (filePath != null) StartParser(filePath);
        }

        private void StartParser(string filepath)
        {
            xmlParser ??= new XmlParser();
            xmlParser.StartParserXml(filepath);
            ShowCollectionJsonObject();
        }

        private void ShowCollectionJsonObject()
        {
            JArray collectionJsonObject = xmlParser.GetCollectionJsonObject();
            CreateTxtFileJson(ref collectionJsonObject);
            MessageBox.Show("Массив Json объектов можно посмотреть, в дебагe в классе ViewModel, в методе ShowCollectionJsonObject или в файле json.txt","Создание Json объекта - Выполнено");            
        }
        private void CreateTxtFileJson(ref JArray collectionJsonObject)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamWriter sw = new StreamWriter("json.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, collectionJsonObject);
            }
        }
    }
}
