namespace DataDeft.Fsyndenton


open System.IO
open System
open FSharp.Data


module Config =


    [<Literal>]
    let configFile = "config.json"


    type Config = JsonProvider<configFile>


    let config =
        Config.Parse(File.ReadAllText(configFile))


    let dateTimeFormat = config.DateTimeFormat
