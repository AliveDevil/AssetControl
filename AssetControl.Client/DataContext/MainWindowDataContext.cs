using AssetControl.Client.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetControl.Client.DataContext
{
    public class MainWindowDataContext : NotifyPropertyChangedBase
    {
        public MainWindowDataContext()
        {
            _remoteObjects = new ObservableCollection<RemoteObject>();
        }

        private readonly ObservableCollection<RemoteObject> _remoteObjects;
        public ObservableCollection<RemoteObject> RemoteObjects
        {
            get
            {
                return _remoteObjects;
            }
        }

        private RemoteObject _selectedRemoteObject;
        public RemoteObject SelectedRemoteObject
        {
            get
            {
                return _selectedRemoteObject;
            }
            set
            {
                if (value is Asset)
                {
                    CurrentAsset = value as Asset;
                }

                _selectedRemoteObject = value;
                onPropertyChanged("SelectedRemoteObject");
            }
        }

        private Asset _currentAsset;
        public Asset CurrentAsset
        {
            get
            {
                return _currentAsset;
            }
            set
            {
                _currentAsset = value;
                onPropertyChanged("CurrentAsset");
            }
        }
    }
}
