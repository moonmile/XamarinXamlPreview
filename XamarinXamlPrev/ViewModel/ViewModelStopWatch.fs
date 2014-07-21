namespace XamarinXamlPrev
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open Moonmile.XFormsProvider
open System.Threading.Tasks
open System.Reflection

type ElapsedEventHandler = delegate of (obj * EventArgs) -> unit

type ViewModelStopWatch() =
    inherit ViewModelBase()

    let mutable startTime = DateTime.Now
    let mutable endTime  = DateTime.Now
    let mutable time:TimeSpan = new TimeSpan()
    let mutable task:Task = null
    let mutable taskflag = false


    /// リフレクションで iOS/Android の System.Timers.Timer を持ってくる
    /// 現在調節中
    let makeTimer (interval:int, hdr:ElapsedEventHandler) =
        let asm = Assembly.Load(AssemblyName("System"))
        let t = asm.GetType( "System.Timers.Timer" )
        let item = System.Activator.CreateInstance(t)

        t.GetRuntimeProperty("Enabled").SetValue( item, true )
        t.GetRuntimeProperty("AutoReset").SetValue( item, true )
        t.GetRuntimeProperty("Interval").SetValue( item, interval )
        t.GetRuntimeEvent("Elapsed").AddEventHandler( item, hdr )
        t.GetRuntimeMethod("Start", null).Invoke(item, null) |> ignore
        item


    member this.Time 
        with get() =  
            time <- endTime - startTime
            String.Format("{0:00}min {1:00}sec {2:000}", time.Minutes, time.Seconds, time.Milliseconds )
    member this.StartTime
        with get() = startTime.ToString()

    member this.StartCommand 
        with get() =
            let cmd = 
                new DelegateCommand(
                    (fun (o) -> true ),
                    (fun (o) -> 
                        startTime <- DateTime.Now 
                        endTime <- startTime
                        this.OnPropertyChanged("StartTime")
                        this.OnPropertyChanged("Time")
                        // makeTimer(2000, new ElapsedEventHandler(fun(s,e)-> 
                        //     System.Diagnostics.Debug.WriteLine("time {0}", endTime ))) |> ignore
                        // )))
                        ))
            cmd

    member this.StopCommand 
        with get() =
            let cmd = 
                new DelegateCommand(
                    (fun (o) -> true ),
                    (fun (o) -> 
                        taskflag <- false
                        endTime <- DateTime.Now 
                        this.OnPropertyChanged("Time")
                    ))    
            cmd

    member this.ResetCommand 
        with get() =
            let cmd = 
                new DelegateCommand(
                    (fun (o) -> true ),
                    (fun (o) -> 
                        taskflag <- false
                        startTime <- DateTime.Now
                        endTime <- DateTime.Now 
                        this.OnPropertyChanged("StartTime")
                        this.OnPropertyChanged("Time")
                    ))    
            cmd
