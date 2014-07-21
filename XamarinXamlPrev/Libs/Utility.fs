namespace XamarinXamlPrev
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open Moonmile.XFormsProvider
open Moonmile.XForms
open System.IO
open System.Reflection

type ResourceLoader() =
    static member val Assembly:Assembly = null with get, set
    static member val Names= [||] with get, set

    static member GetObject(resourceName:string) =
        if ResourceLoader.Assembly = null then
            ResourceLoader.Assembly <- typeof<ResourceLoader>.GetTypeInfo().Assembly
            ResourceLoader.Names <- ResourceLoader.Assembly.GetManifestResourceNames()
        try
            let path = 
                ResourceLoader.Names 
                |> Seq.find( fun(x) -> x.EndsWith(resourceName, StringComparison.CurrentCultureIgnoreCase))
            ResourceLoader.Assembly.GetManifestResourceStream(path)
        with
        | _ -> null

    static member GetString(resourceName:string) =
        use st = ResourceLoader.GetObject(resourceName)
        if st <> null then
            use sr = new StreamReader(st)
            let xaml = sr.ReadToEnd()
            xaml
        else
            ""

