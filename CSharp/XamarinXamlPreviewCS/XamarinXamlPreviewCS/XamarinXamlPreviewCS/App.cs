using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Moonmile.XFormsProvider;

namespace XamarinXamlPreviewCS
{
    public class App
    {
        public static Page GetMainPage()
        {
            var page = new MainPage("MainPage.xml").Page;
            page.BindingContext = new ViewModel.DataModel();
            return new NavigationPage(page);

        }
    }
}
