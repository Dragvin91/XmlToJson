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
        }
    }
}
