namespace DataDeft.Fsyndenton


open Mono.Unix
open System
open System.IO
open System.Collections.Generic


module Main =


    let logger (s: string) =
        System.Console.WriteLine(sprintf "%s %s" (DateTime.Now.ToString Config.dateTimeFormat) s)


    let getEnumerates
        (folderSource: string)
        (folderTarget: string)
        : option<IEnumerable<string>> * option<IEnumerable<string>> =

        let sourceDirInfoEnum =
            try
                Some(Directory.EnumerateFiles(folderSource, "*.*", SearchOption.AllDirectories))
            with
            | ex -> None

        let targetDirInfoEnum =
            try
                Some(Directory.EnumerateFiles(folderTarget, "*.*", SearchOption.AllDirectories))
            with
            | ex -> None

        (sourceDirInfoEnum, targetDirInfoEnum)


    let iterateOver (s: IEnumerable<string>) (t: IEnumerable<string>) =

        for pathEntry in s do
            try
                let fileStat =
                    UnixFileSystemInfo.GetFileSystemEntry(pathEntry)

                let isRegular = fileStat.IsRegularFile

                if isRegular then
                    use f =
                        System.IO.File.Open(pathEntry, FileMode.Open)

                    if f.CanRead then

                        // while canread
                        // readSome (16_000_000)
                        // writeSome to new location
                        // concat files on target

                        // let s = Span<byte>()
                        // let r = f.Read(s)
                        // let hash =
                        //     Hashing.getHashedValueSpanBytes Hashing.xxHasher s

                        logger <| sprintf "%s :: %A" pathEntry isRegular

            with
            | ex -> logger <| sprintf "%s" ex.Message


    let ops s t =
        let enumerates = getEnumerates s t

        match enumerates with
        | Some s, Some t -> iterateOver s t
        | _ -> logger "error"

        ()


    [<EntryPoint>]
    let main argv =
        logger "Starting..."
        logger <| sprintf "ARGV length %A" argv.Length
        let commandLineArgumentsParsed = Cli.parseCommandLine (Array.toList argv)
        logger <| sprintf "%A" commandLineArgumentsParsed

        let source = commandLineArgumentsParsed.SourceFolder
        let target = commandLineArgumentsParsed.TargetFolder

        match (source, target) with
        | "empty", _
        | _, "empty" ->
            logger
            <| sprintf "Please supply both source and target folders"

            Environment.Exit 1
        | s, t -> ops s t

        0
