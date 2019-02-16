using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.Client.ViewModels
{
    using System.Collections.Generic;

    using Prism.Commands;
    using Prism.Mvvm;

    using ZbW.Testing.Dms.Client.Model;
    using ZbW.Testing.Dms.Client.Repositories;

    internal class SearchViewModel : BindableBase
    {
        private List<MetadataItem> _filteredMetadataItems;

        private MetadataItem _selectedMetadataItem;

        private string _selectedTypItem;

        private string _suchbegriff;

        private List<string> _typItems;
        private string memoryPath;

        private FileServices fileServices;

        public SearchViewModel()
        {
            TypItems = ComboBoxItems.Typ;

            CmdSuchen = new DelegateCommand(OnCmdSuchen);
            CmdReset = new DelegateCommand(OnCmdReset);
            CmdOeffnen = new DelegateCommand(OnCmdOeffnen, OnCanCmdOeffnen);
            memoryPath = ConfigurationManager.AppSettings["RepositoryDir"];
            fileServices=new FileServices();
        }

        public DelegateCommand CmdOeffnen { get; }

        public DelegateCommand CmdSuchen { get; }

        public DelegateCommand CmdReset { get; }

        public string Suchbegriff
        {
            get
            {
                return _suchbegriff;
            }

            set
            {
                SetProperty(ref _suchbegriff, value);
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

        public List<MetadataItem> FilteredMetadataItems
        {
            get
            {
                return _filteredMetadataItems;
            }

            set
            {
                SetProperty(ref _filteredMetadataItems, value);
            }
        }

        public MetadataItem SelectedMetadataItem
        {
            get
            {
                return _selectedMetadataItem;
            }

            set
            {
                if (SetProperty(ref _selectedMetadataItem, value))
                {
                    CmdOeffnen.RaiseCanExecuteChanged();
                }
            }
        }

        private bool OnCanCmdOeffnen()
        {
            return SelectedMetadataItem != null;
        }

        private void OnCmdOeffnen()
        {
            // TODO: Add your Code here
            var phate = memoryPath + "\\" + SelectedMetadataItem.ValueDate.Year + "\\" + SelectedMetadataItem.Guid + "_Content." + SelectedMetadataItem.FileName.Split('.').Last();
            fileServices.OpenFile(phate);
        }

        private void OnCmdSuchen()
        {
            Search();
        }

        private void Search()
        {
            FilteredMetadataItems = new List<MetadataItem>();
            var directories = Directory.GetDirectories(memoryPath);
            var result = new List<MetadataItem>();
            foreach (var dict in directories)
            {
                DirectoryInfo d = new DirectoryInfo(dict);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.xml");
                foreach (var file in Files)
                {
                    var data = MetadataItem.Deserialize(file.FullName);
                    if (FileIsOk(data))
                    {
                        result.Add(data);
                    }
                }
            }

            FilteredMetadataItems = result;
        }

        public bool FileIsOk(MetadataItem item)
        {
            if (
                (
                    (SuchbegriffHasValueAndSelectedTypItemHasNoValue())
                    &&
                    (
                        (item.FileName != null && item.FileName.Contains(Suchbegriff)) ||
                        (item.Designation != null && item.Designation.Contains(Suchbegriff))
                    )
                )
                ||
                (
                    (SuchbegriffHasNoValueAndSelectedTypItemHasValue())
                    && (SelectedTypItem == item.Type)
                )
                ||
                (
                    (SuchbegriffAndSelectedTypItemHasValue())
                    &&
                    (
                        (item.FileName != null && item.FileName.Contains(Suchbegriff) &&
                         SelectedTypItem == item.Type) ||
                        (item.Designation != null && item.Designation.Contains(Suchbegriff) &&
                         SelectedTypItem == item.Type)
                    )
                )
            )
            {
                return true;
            }
            else if (item.Keywords != null)
            {
                foreach (var keyW in item.Keywords)
                {
                    if
                    (
                        (
                            (SuchbegriffHasValueAndSelectedTypItemHasNoValue())
                            &&
                            keyW.Contains(Suchbegriff)
                        )
                       
                        ||
                        (
                            (SuchbegriffAndSelectedTypItemHasValue())
                            &&
                            (keyW.Contains(Suchbegriff) && SelectedTypItem == item.Type)
                        )
                    )
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        private bool SuchbegriffHasValue()
        {
            return Suchbegriff != null && Suchbegriff.Length >= 1;
        }
        private bool SelectedTypItemHasValue()
        {
            return SelectedTypItem != null;
        }

        private bool SuchbegriffHasValueAndSelectedTypItemHasNoValue()
        {
            return SuchbegriffHasValue() && !SelectedTypItemHasValue();
        }
        private bool SuchbegriffHasNoValueAndSelectedTypItemHasValue()
        {
            return !SuchbegriffHasValue() && SelectedTypItemHasValue();
        }
        private bool SuchbegriffAndSelectedTypItemHasValue()
        {
            return SuchbegriffHasValue() && SelectedTypItemHasValue();
        }


        private void OnCmdReset()
        {
            FilteredMetadataItems = new List<MetadataItem>();
            Suchbegriff = string.Empty;
            SelectedTypItem = null;
        }
    }
}