using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using ZbW.Testing.Dms.Client.Model;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.Client.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Win32;

    using Prism.Commands;
    using Prism.Mvvm;

    using ZbW.Testing.Dms.Client.Repositories;

    public class DocumentDetailViewModel : BindableBase
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
        public FileServices FileServices;

        public DocumentDetailViewModel()
        {

        }
        public DocumentDetailViewModel(string benutzer, Action navigateBack)
        {
            _navigateBack = navigateBack;
            Benutzer = benutzer;
            Erfassungsdatum = DateTime.Now;
            TypItems = ComboBoxItems.Typ;

            CmdDurchsuchen = new DelegateCommand(OnCmdDurchsuchen);
            CmdSpeichern = new DelegateCommand(OnCmdSpeichern);
            memoryPath = ConfigurationManager.AppSettings["RepositoryDir"];
           
            FileServices = new FileServices();

            FileServices.CreatePhatIfNotExist(memoryPath);
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
                if (FileChosen())
                {
                    MemoryFiles();
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

        public bool FileChosen()
        {
            return _filePath?.Length > 0;
        }

        public void MemoryFiles()
        {
            var year = ValutaDatum.Value.Year;
            var Guid = System.Guid.NewGuid();
            var newPhate = memoryPath + "\\" + year + "\\" + Guid;
            FileServices.CreatePhatIfNotExist(memoryPath + "\\" + year);
            FileServices.GeneratXMl(newPhate, CreateMeatInfo(Guid.ToString()));
            FileServices.CopyTo(_filePath, newPhate + "_Content." + _filePath.Split().Last().Split('.').Last());
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
    }
}