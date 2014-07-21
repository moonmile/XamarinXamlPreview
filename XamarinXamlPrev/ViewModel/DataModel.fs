namespace XamarinXamlPrev
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open Moonmile.XFormsProvider
open System.IO
open System.Reflection

type DataMode() =
    inherit ViewModelBase()

    let mutable appName = "xaml preview"
    let mutable version = "alpha"

    member this.AppName
        with get() = appName
    member this.Version
        with get() = version
        and set(v) = version <- v
    member this.Updated 
        with get() = DateTime.Now.ToString()

type ViewModelNextPage() =
    inherit ViewModelBase()

    let mutable updated  = DateTime.Now

    member this.Updated 
        with get() = updated.ToString()
        and set(value) = 
            if updated.ToString() <> value then
                updated <- DateTime.Parse( value )
                base.OnPropertyChanged("Updated")

    member this.UpdatedCommand 
        with get() =
            let cmd = 
                new DelegateCommand(
                    (fun (o) -> true ),
                    (fun (o) -> this.Updated <- DateTime.Now.ToString()))
            cmd
