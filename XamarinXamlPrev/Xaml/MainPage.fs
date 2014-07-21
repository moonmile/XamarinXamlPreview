namespace XamarinXamlPrev
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open System.IO
open System.Reflection
open Moonmile.XForms
open System.Threading.Tasks
open System.Net.Http

module XForms =
    type Page with
        /// <summary>
        /// alias FindByName
        /// </summary>
        /// <param name="name"></param>
        member this.FindByName<'T when 'T :> Element >( name:string) =
            FindByName(name, this) :?> 'T

open XForms // Page.FindByName<>() を入れ替える

type MainPage(path:string) =
    inherit ContentPage()
    let mutable page:Page = null
    let mutable buttonNextPage:Button = null
    let mutable buttonStopWatch:Button = null
    let mutable buttonStopWatchDownload:Button = null
    let mutable label:Label = null
    
    let pushPageVM(rn:string, vm:ViewModelBase) =
        let xaml = ResourceLoader.GetString(rn)
        let next = ParseXaml.LoadXaml(xaml) 
        next.BindingContext <- vm
        let t = new Task( fun() -> 
            let task = page.Navigation.PushAsync(next)
            task.Start()
        )
        t.RunSynchronously()
    
    let pushPage(rn:string) =
        let xaml = ResourceLoader.GetString(rn)
        let next = ParseXaml.LoadXaml(xaml) 
        let t = new Task( fun() -> 
            let task = page.Navigation.PushAsync(next)
            task.Start()
        )
        t.RunSynchronously()

    let pushPageDL(url:string) =
        
        try 
            let hc = new HttpClient()
            let xaml = hc.GetStringAsync(url).Result
            let next = ParseXaml.LoadXaml(xaml) 
            let t = new Task( fun() -> 
                let task = page.Navigation.PushAsync(next)
                task.Start()
            )
            t.RunSynchronously()
        with
        | _ -> 
            label.Text <- String.Format("Error: cannot open {0}", url )

    do 
        let xaml = ResourceLoader.GetString(path)

        page <- ParseXaml.LoadXaml(xaml)

        buttonNextPage <- page.FindByName<Button>("buttonNextPage")
        buttonStopWatch <- page.FindByName<Button>("buttonStopWatch")
        buttonStopWatchDownload <- page.FindByName<Button>("buttonStopWatchDownload")
        label <- page.FindByName<Label>("label")
        
        buttonNextPage.Clicked.Add( fun(e) -> 
                pushPageVM("NextPage.xml", new ViewModelNextPage())
            )
        buttonStopWatch.Clicked.Add( 
            fun(e) -> pushPage("StopWatchPage.xml"))

        buttonStopWatchDownload.Clicked.Add( 
            fun(e) -> pushPageDL("http://moonmile.net/up/DownLoadPage.xml"))

    member this.Page 
        with get() = page
