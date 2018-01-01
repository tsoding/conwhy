open System

type Cell = Dead | Alive
type World = Cell list list

let renderCell (cell: Cell): string =
  match cell with
  | Dead -> "."
  | Alive -> "@"

let countNeighbors (world: World) ((i, j) : int * int): int =
  0

let nextGenCell (world: World) ((i, j): int * int): Cell =
  let neighbors = countNeighbors world (i, j) in
  match world.[i].[j] with
  | Dead when neighbors = 3 -> Alive
  | Alive -> Alive

let updateWorld (world: World): World =
  world
  |> List.mapi (fun i row ->
         List.mapi (fun j cell ->
             nextGenCell world (i, j)) row)

let renderWorld (world: World): string =
  world
  |> List.map (fun (row) -> row
                            |> List.map renderCell
                            |> String.concat "")
  |> String.concat "\n"

let rec gameLoop (world: World): int =
  match Console.ReadLine() with
  | "quit" -> 0
  | _ ->
     world |> renderWorld |> printfn "%s";
     world |> updateWorld |> gameLoop

[<EntryPoint>]
let main _ =
  let n = 10
  let m = 10
  let world = [ for i in 1 .. n -> [ for j in 1 .. n -> Dead ] ]
  gameLoop world
