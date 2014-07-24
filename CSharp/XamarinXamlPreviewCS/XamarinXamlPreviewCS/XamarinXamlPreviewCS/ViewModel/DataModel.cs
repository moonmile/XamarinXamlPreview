using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinXamlPreviewCS.Libs;

namespace XamarinXamlPreviewCS.ViewModel
{
    public class DataModel : ViewModelBase
    {
        public string AppName { get; internal set; }
        public string Version { get; internal set; }
        public string Updated { get { return DateTime.Now.ToString(); } }

        public DataModel()
        {
            this.AppName = "Xaml preview";
            this.Version = "Alpha version";
        }
    }

    public class ViewModelNextPage : ViewModelBase
    {
        private DateTime updated = DateTime.Now;

        public string Updated
        {
            get { return updated.ToString(); }
            set
            {
                if (updated.ToString() != value)
                {
                    updated = DateTime.Parse(value);
                    base.OnPropertyChanged("Updated");
                }
            }
        }
        public ICommand UpdatedCommand
        {
            get
            {
                var cmd = new DelegateCommand(
                    (s) => { return true; },
                    (s) => { this.Updated = DateTime.Now.ToString(); });
                return cmd;
            }
        }
    }
}
