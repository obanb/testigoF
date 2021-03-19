module helloworld.tests.common

open Expecto
open System

type ListItem = { prop: int; }
type ListItem2 = { newProp: string; }
let simpleTest =
  test "A simple test" {
    let expected = 4
    Expect.equal expected (2+2) "2+2 = 4"
  }
    
  test "A simple test" {
    let expected = 4
    Expect.equal expected (2+2) "2+2 = 4"
  }
  
  test "LIST.CHOOSE - multiple result types concat to single string" {
    let results = [Error "error"; Error "error2"]
    
    let expected = results |> List.choose (fun res ->
               match res with
                     | Error msg -> Some msg
                     | Ok _ -> None)
                                  |> (fun filtered -> match filtered with
                                          | [] -> "ok result"
                                          | _ ->  (",", filtered) |> String.Join)
                                  
    Expect.equal expected "error,error2" "Should return equal strings."
  }
  
  test "LIST.SEQUENCE_RESULT_A multiple result types into first result Error - short circuit" {
    let results = [Error "error";Error "error2"; Ok "ok"]
    
    let short = results |> List.sequenceResultA
    
    let expected = match short with
                   | Ok results -> "ok"
                   | Error e -> e
                                  
    Expect.equal expected "error" "Should return equal strings."
  }
  
  
  test "LIST.TRAVERSE_RESULT_A multiple result types into first result Error - short circuit" {
    let results = ["error2";"ok";"error"]
    
    let resultFn s = match s with
                     | "error" | "error2" -> Error "error msg"
                     | _ -> Ok "ok"
    
    let short = results |> List.traverseResultA resultFn
    
    let expected = match short with
                   | Ok results -> "ok"
                   | Error e -> e
                                  
    Expect.equal expected "error msg" "Should return equal strings."
  }
  
  test "LIST.TRAVERSE_ASYNC_A multiple async map results into single async list" {
    let origin = ["error2";"ok";"error"]
    
    let resultFn (s:string) = async{
        let res = s
        return res 
    }
    
    let short = origin |> List.traverseAsyncA resultFn |> Async.RunSynchronously
    
    let expected =["error2";"ok";"error"]
                                
    Expect.equal expected origin "Should return equal lists of strings."
  }
  
  test "Some common list operations" {
    let initialList = [{ prop = 1}; {prop = 2}]
    
    let filter = initialList |> List.filter (fun item -> item.prop = 1)
    let map = initialList |> List.map (fun item -> item.prop + 1)
    let reduce = map |> List.reduce (fun acu next -> acu + next)
    let fold = map |> List.fold (fun acu next -> acu + next) 0
    let foldRecordToString = initialList |> List.fold (fun acu next -> acu + next.prop.ToString()) ""
    let foldRecordsToRecord = initialList |> List.fold (fun acu next -> { newProp = acu.newProp + next.prop.ToString() }) { newProp = "0" }
    
    let expectedFilter = [{prop = 1}]
    let expectedMap = [2;3]
    let expectedReduce = 5
    let expectedFold = 5
    let expectedFoldRecordToString = "12"
    let expectedFoldRecordsToRecord = { newProp = "012" }
                                
    Expect.equal expectedFilter filter "Should return filtered list"
    Expect.equal expectedMap map "Should return remapped list of ints"
    Expect.equal reduce expectedReduce "Should return reduced int"
    Expect.equal fold expectedFold "Should return folded int"
    Expect.equal foldRecordToString expectedFoldRecordToString "Should return folded string"
    Expect.equal foldRecordsToRecord expectedFoldRecordsToRecord "Should return folded record"
  }
  
  

  
  