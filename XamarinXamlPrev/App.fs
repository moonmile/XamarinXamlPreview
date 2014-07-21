namespace XamarinXamlPrev
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open System.IO
open System.Reflection
open Moonmile.XForms

type App() = 
    static member GetMainPage() =
        let page = MainPage("MainPage.xml").Page
        page.BindingContext <- new DataMode()
        new NavigationPage(page)
