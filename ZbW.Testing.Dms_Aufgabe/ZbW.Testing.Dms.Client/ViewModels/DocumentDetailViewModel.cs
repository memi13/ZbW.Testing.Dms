using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using ZbW.Testing.Dms.Client.Model;

namespace ZbW.Testing.Dms.Client.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Win32;

    using Prism.Commands;
    using Prism.Mvvm;

    using ZbW.Testing.Dms.Client.Repositories;

    internal class DocumentDetailViewModel : BindableBase
    {
        private readonly Action _navigateBack;

        private string _benutzer;

        private string _bezeichnung;

        private DateTime _erfassungsdatum;

        private string _filePath;

        private bool _isRemoveFileEnabled;

        private string _selectedTypItem;

        private string _stichwoerter;

        private List<string> _typItems;

        private DateTime? _valutaDatum;
        private string memoryPath;

        public DocumentDetailViewModel(string benutzer, Action navigateBack)
        {
            _navigateBack = navigateBack;
            Benutzer = benutzer;
            Erfassungsdatum = DateTime.Now;
            TypItems = ComboBoxItems.Typ;

            CmdDurchsuchen = new DelegateCommand(OnCmdDurchsuchen);
            CmdSpeichern = new DelegateCommand(OnCmdSpeichern);
            memoryPath = ConfigurationManager.AppSettings["RepositoryDir"];
            CreatePhatIfNotExist(memoryPath);
        }

        public string Stichwoerter
        {
            get
            {
                return _stichwoerter;
            }

            set
            {
                SetProperty(ref _stichwoerter, value);
            }
        }

        public string Bezeichnung
        {
            get
            {
                return _bezeichnung;
            }

            set
            {
                SetProperty(ref _bezeichnung, value);
            }
        }

        public List<string> TypItems
        {
            get
            {
                return _typItems;
            }

            set
            {
                SetProperty(ref _typItems, value);
            }
        }

        public string SelectedTypItem
        {
            get
            {
                return _selectedTypItem;
            }

            set
            {
                SetProperty(ref _selectedTypItem, value);
            }
        }

        public DateTime Erfassungsdatum
        {
            get
            {
                return _erfassungsdatum;
            }

            set
            {
                SetProperty(ref _erfassungsdatum, value);
            }
        }

        public string Benutzer
        {
            get
            {
                return _benutzer;
            }

            set
            {
                SetProperty(ref _benutzer, value);
            }
        }

        public DelegateCommand CmdDurchsuchen { get; }

        public DelegateCommand CmdSpeichern { get; }

        public DateTime? ValutaDatum
        {
            get
            {
                return _valutaDatum;
            }

            set
            {
                SetProperty(ref _valutaDatum, value);
            }
        }

        public bool IsRemoveFileEnabled
        {
            get
            {
                return _isRemoveFileEnabled;
            }

            set
            {
                SetProperty(ref _isRemoveFileEnabled, value);
            }
        }

        private void OnCmdDurchsuchen()
        {
            var openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                _filePath = openFileDialog.FileName;
            }
        }

        private void OnCmdSpeichern()
        {
            // TODO: Add your Code here
            if
                (
                    !BezeichungIsValid() || !SelectedTypItemIsValid() || !ValutaDatumIsValid()
                )
            {
                showMessageBox("Es müssen alle Pflichtfelder ausgefüllt werden!", "Information");
            }
            else
            {
                if (FilePhatHasValue())
                {
                    CopyTo();
                    if (IsRemoveFileEnabled)
                    {
                        File.Delete(_filePath);
                    }
                    _navigateBack();
                }
                else
                {
                    showMessageBox("Keine Datei ausgwählt", "Information");
                }
            }


        }

        public bool BezeichungIsValid()
        {
            return Bezeichnung?.Length > 0;
        }

        public bool SelectedTypItemIsValid()
        {
            return SelectedTypItem?.Length > 0;
        }

        public bool ValutaDatumIsValid()
        {
            return ValutaDatum != null;
        }

        public void showMessageBox(string text, string titel)
        {
            MessageBox.Show(text, text, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool FilePhatHasValue()
        {
            return _filePath?.Length > 0;
        }

        public void CopyTo()
        {
            var year = ValutaDatum.Value.Year;
            var Guid = System.Guid.NewGuid();
            CreatePhatIfNotExist(memoryPath + "\\" + year);
            var xml= MetadataItem.Seralize(CreateMeatInfo(Guid.ToString()));
           
            var newPhate = memoryPath + "\\" + year + "\\" + Guid;
            using (File.Create(newPhate + "_Metadata.xml")) ;

            using (TextWriter tw = new StreamWriter(newPhate + "_Metadata.xml"))
            {
                tw.WriteLine(xml);
                tw.Close();
            }
           
           
            File.Copy(_filePath, newPhate+ "_Content."+_filePath.Split().Last().Split('.').Last());
        }

        private bool CreatePhatIfNotExist(string phat)
        {
            if (CheckPathExist(phat))
                return true;
            else
            {
                var phatParts = phat.Split('\\');
                var phateComplit = false;
                var counter = 0;
                while (!phateComplit)
                {
                    var newphate = string.Empty;
                    for (int i = 0; i < counter + 1; i++)
                    {
                        newphate = newphate + phatParts[i] + "\\";
                    }

                    if (!CheckPathExist(newphate))
                    {
                        System.IO.Directory.CreateDirectory(newphate);
                    }
                    counter++;

                    phateComplit = CheckPathExist(phat);
                }
                return true;
            }

        }
        public bool CheckPathExist(string phat)
        {
            return Directory.Exists(phat);
        }

        public MetadataItem CreateMeatInfo(string id)
        {
            var meta = new MetadataItem();
            meta.Guid = id;
            meta.ValueDate = (DateTime)ValutaDatum;
            meta.FileName = _filePath.Split('\\').Last();
            meta.Type = SelectedTypItem;
            meta.Keywords = new List<string>();
            meta.Designation = Bezeichnung;
            if (Stichwoerter != null)
                if (Stichwoerter.Contains(','))
                    meta.Keywords = Stichwoerter.Split(',').ToList();
                else
                {
                    meta.Keywords.Add(Stichwoerter);
                }
            meta.User = Benutzer;
            meta.CreareDate = Erfassungsdatum;
            return meta;
        }

        public string CreareXMl(MetadataItem meta)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(MetadataItem));
            var subReq = meta;
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                     xml = sww.ToString(); // Your XML
                    return xml;
                }
            }
        }

    }
}