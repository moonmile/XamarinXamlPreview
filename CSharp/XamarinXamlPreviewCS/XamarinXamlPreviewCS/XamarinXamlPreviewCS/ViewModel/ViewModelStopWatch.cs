using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinXamlPreviewCS.Libs;

namespace XamarinXamlPreviewCS.ViewModel
{
    public delegate void ElapsedEventHandler( object sender, EventArgs e);
    public class ViewModelStopWatch : Libs.ViewModelBase
    {
        protected DateTime startTime = DateTime.Now;
        protected DateTime endTime = DateTime.Now;
        protected TimeSpan time = new TimeSpan();

        /// リフレクションで iOS/Android の System.Timers.Timer を持ってくる
        /// 現在調節中
        public object makeTimer(int interval, ElapsedEventHandler hdr)
        {
            var asm = Assembly.Load(new AssemblyName("System"));
            var t = asm.GetType("System.Timers.Timer");
            var item = System.Activator.CreateInstance(t);

            t.GetRuntimeProperty("Enabled").SetValue(item, true);
            t.GetRuntimeProperty("AutoReset").SetValue( item, true );
            t.GetRuntimeProperty("Interval").SetValue( item, interval );
            t.GetRuntimeEvent("Elapsed").AddEventHandler(item, hdr);
            return item;
        }

        public string Time
        {
            get
            {
                time = endTime - startTime;
                return string.Format("{0:00}min {1:00}sec {2:000}", time.Minutes, time.Seconds, time.Milliseconds);
            }
        }
        public string StartTime
        {
            get { return startTime.ToString(); }
        }

        public ICommand StartCommand
        {
            get
            {
                var cmd = new DelegateCommand(
                    (o) => true,
                    (o) => {
                        startTime = DateTime.Now;
                        endTime = startTime;
                        this.OnPropertyChanged("StartTime");
                        this.OnPropertyChanged("Time");
                    });
                return cmd;
            }
        }
        public ICommand StopCommand
        {
            get
            {
                var cmd = new DelegateCommand(
                    (o) => true,
                    (o) =>
                    {
                        endTime = DateTime.Now;
                        this.OnPropertyChanged("Time");
                    });
                return cmd;
            }
        }
        public ICommand ResetCommand
        {
            get
            {
                var cmd = new DelegateCommand(
                    (o) => true,
                    (o) =>
                    {
                        startTime = DateTime.Now;
                        endTime = startTime;
                        this.OnPropertyChanged("StartTime");
                        this.OnPropertyChanged("Time");
                    });
                return cmd;
            }
        }
    }
}
