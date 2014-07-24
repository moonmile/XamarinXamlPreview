namespace XamarinXamlPrev
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open System.IO
open System.Reflection
open System.Threading.Tasks
open System.Net.Http
open Moonmile.XFormsProvider

module Async =
    let AwaitTaskVoid : (Task -> Async<unit>) =
        Async.AwaitIAsyncResult >> Async.Ignore

type MainPage(path:string) =
    inherit ContentPage()
    let mutable page:Page = null
    let mutable buttonNextPage:Button = null
    let mutable buttonStopWatch:Button = null
    let mutable buttonStopWatchDownload:Button = null
    let mutable label:Label = null

    let pushNextPage(next) =
        let AwaitTaskVoid : (Task -> Async<unit>) =
            Async.AwaitIAsyncResult >> Async.Ignore
        page.Navigation.PushAsync(next) |> AwaitTaskVoid |> ignore
        
    
    let pushPageVM(rn:string, vm:ViewModelBase) =
        let xaml = ResourceLoader.GetString(rn)
        let next = Moonmile.XForms.ParseXaml.LoadXaml(xaml) 
        next.BindingContext <- vm
        pushNextPage(next)

    let pushPage(rn:string) =
        let xaml = ResourceLoader.GetString(rn)
        let next = Moonmile.XForms.ParseXaml.LoadXaml(xaml) 
        pushNextPage(next)

    let pushPageDL(url:string, vm:ViewModelBase) =
        
        try 
            let hc = new HttpClient()
            let xaml = hc.GetStringAsync(url).Result
            let next = Moonmile.XForms.ParseXaml.LoadXaml(xaml) 
            if vm <> null then next.BindingContext <- vm
            pushNextPage(next)

        with
        | _ -> 
            label.Text <- String.Format("Error: cannot open {0}", url )

    do 
        let xaml = ResourceLoader.GetString(path)

        page <- Moonmile.XForms.ParseXaml.LoadXaml(xaml)

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
            fun(e) -> pushPageDL("http://moonmile.net/up/DownLoadPage.xml", new ViewModelStopWatch()))

    member this.Page 
        with get() = page
