using Newtonsoft.Json.Linq;
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
            MessageBox.Show("Массив Json объектов можно посмотреть, в дебагe в классе ViewModel, в методе ShowCollectionJsonObject","Создание Json объекта - Выполнено");
        }

    }
}
