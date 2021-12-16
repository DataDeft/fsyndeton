namespace DataDeft.Fsyndenton

open System
open System.Data.HashFunction
open System.Data.HashFunction.xxHash

module Hashing =



    let xxHasher =
        let config =
            new xxHashConfig (HashSizeInBits = 64, Seed = 1337UL)

        xxHashFactory.Instance.Create(config)


    let getHashedValueString (xxHasher: IxxHash) (inp: string) = xxHasher.ComputeHash(inp).AsHexString()


    let getHashedValueBytes (xxHasher: IxxHash) (inp: byte) = xxHasher.ComputeHash(inp).AsHexString()


    let getHashedValueSpanBytes (xxHasher: IxxHash) (inp: Span<byte>) = xxHasher.ComputeHash(inp.ToArray()).AsHexString()
