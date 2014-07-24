using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Moonmile.XFormsProvider;
using System.Net.Http;
using XamarinXamlPreviewCS.ViewModel;

namespace XamarinXamlPreviewCS
{
    public static class PageExtentions
    {
        public static T FindName<T>(this Page page, string name)
            where T: Element
        {
            return Moonmile.XForms.FindByName(name, page) as T;
        }

    }

    public class MainPage : ContentPage
    {
        private Page page = null;
        private Button buttonNextPage;
        private Button buttonStopWatch;
        private Button buttonStopWatchDownload;
        private Label label;

        public Page Page { get { return page;  } }

        public MainPage(string path)
        {
            var xaml = Libs.ResourceLoader.GetString(path);
            page = PageXaml.LoadXaml(xaml);
            buttonNextPage = page.FindByName<Button>("buttonNextPage");
            buttonStopWatch = page.FindByName<Button>("buttonStopWatch");
            buttonStopWatchDownload = page.FindByName<Button>("buttonStopWatchDownload");
            label = page.FindByName<Label>("label");

            buttonNextPage.Clicked += (s, e) => pushPage("NextPage.xml", new ViewModelNextPage());
            buttonStopWatch.Clicked += (s, e) => pushPage("StopWatchPage.xml");
            buttonStopWatchDownload.Clicked += (s, e) =>
            {
                pushPage(new Uri("http://moonmile.net/up/DownLoadPage.xml"), new ViewModelStopWatch());
            };
            

        }

        public async void pushPage(string rn, Libs.ViewModelBase vm = null )
        {
            var xaml = Libs.ResourceLoader.GetString(rn);
            var next = PageXaml.LoadXaml(xaml);
            if( vm != null )
                next.BindingContext = vm;
            // await page.Navigation.PushAsync(next);
            await page.Navigation.PushAsync(next);
        }
        public async void pushPage(Uri uri, Libs.ViewModelBase vm = null)
        {
            try
            {
                var hc = new HttpClient();
                var xaml = await hc.GetStringAsync(uri);
                var next = PageXaml.LoadXaml(xaml);
                if (vm != null)
                    next.BindingContext = vm;
                await page.Navigation.PushAsync(next);
            }
            catch
            {
                label.Text = String.Format("Error: cannot open {0}", uri);
            }
        }
    }
}
