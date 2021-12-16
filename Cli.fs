namespace DataDeft.Fsyndenton

open System
open System.IO
open System.Collections.Generic


module Cli =


    let logger (s: string) = System.Console.WriteLine s


    [<StructuredFormatDisplay("SourceFolder: {SourceFolder} :: TargetFolder: {TargetFolder}")>]
    type CommandLineOptions =
        { SourceFolder: string
          TargetFolder: string }

    let isValidFolder f = true


    let rec parseCommandLineRec args optionsSoFar =
        match args with

        // when args are empty return
        | [] -> optionsSoFar

        // source folder
        | "--source" :: xs ->
            match xs with
            | folder :: xss ->
                match isValidFolder folder with
                | true ->
                    parseCommandLineRec
                        xss
                        { optionsSoFar with
                              SourceFolder = folder }
                | false ->
                    logger <| sprintf "Unsupported folder: %A" folder
                    Environment.Exit 1
                    parseCommandLineRec xss optionsSoFar // never reach

            | [] ->
                logger <| sprintf "SourceFolder cannot be empty"
                Environment.Exit 1
                parseCommandLineRec xs optionsSoFar // never reach

        // target folder
        | "--target" :: xs ->
            match xs with
            | folder :: xss ->
                match isValidFolder folder with
                | true ->
                    parseCommandLineRec
                        xss
                        { optionsSoFar with
                              TargetFolder = folder }
                | false ->
                    logger <| sprintf "Unsupported folder: %A" folder
                    Environment.Exit 1
                    parseCommandLineRec xss optionsSoFar // never reach

            | [] ->
                logger <| sprintf "TargetFolder cannot be empty"
                Environment.Exit 1
                parseCommandLineRec xs optionsSoFar // never rea

        // handle unrecognized option and keep looping
        | x :: xs ->
            logger <| sprintf "Option %s is unrecognized" x
            parseCommandLineRec xs optionsSoFar

    // create the "public" parse function
    let parseCommandLine args =
        // create the defaults
        let defaultOptions =
            { SourceFolder = "empty"
              TargetFolder = "empty" }

        // call the recursive one with the initial options
        parseCommandLineRec args defaultOptions
