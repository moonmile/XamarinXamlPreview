namespace XamarinXamlPrev
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open Moonmile.XFormsProvider
open System.IO
open System.Reflection
open System.ComponentModel
open System.Windows.Input

[<AllowNullLiteral>]
type ViewModelBase() =
    let propertyChangedEvent = new DelegateEvent<PropertyChangedEventHandler>()
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member x.PropertyChanged = propertyChangedEvent.Publish
    member x.OnPropertyChanged propertyName = 
        propertyChangedEvent.Trigger([| x; new PropertyChangedEventArgs(propertyName) |])

type DelegateCommand(can, exec) =
    let canExecute:(obj -> bool) = can
    let execute:(obj -> unit ) = exec
    let eh = new DelegateEvent<EventHandler> ()
    interface ICommand with
        [<CLIEvent>]
        member x.CanExecuteChanged = eh.Publish
        member x.CanExecute(parameter: obj): bool = canExecute( parameter )
        member x.Execute(parameter: obj): unit = execute(parameter)
