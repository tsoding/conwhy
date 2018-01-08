module Game

type Cell = Dead | Alive
type World = {
  cells: Cell list list;
  size: int * int
}

let parseCell (c: char): Cell =
  match c with
  | '@' -> Alive
  | _ -> Dead

let makeWorld (map: string list): World = {
  cells = [for row in map do
           yield [for c in row do
                  yield (parseCell c)]];
  size = (List.length map, String.length map.[0])
}

let renderCell (cell: Cell): string =
  match cell with
  | Dead -> "."
  | Alive -> "@"

let countNeighbors (world: World) ((i, j) : int * int): int =
  [for i in -1 .. 1 do
   for j in -1 .. 1 do
   yield (i, j)]
  |> List.filter (fun x -> x <> (0, 0))
  |> List.map (fun (di, dj) ->
         try
           match world.cells.[i + di].[j + dj] with
           | Dead -> 0
           | Alive -> 1
         with
         | :? System.ArgumentException as ex -> 0)
  |> List.fold (+) 0

let nextGenCell (world: World) ((i, j): int * int): Cell =
  let neighbors = countNeighbors world (i, j) in
  match world.cells.[i].[j] with
  | Dead when neighbors = 3 -> Alive
  | Alive when neighbors < 2 || neighbors > 3 -> Dead
  | x -> x

let updateWorld (world: World): World =
  { cells = world.cells
            |> List.mapi (fun i row ->
                   List.mapi (fun j cell ->
                       nextGenCell world (i, j)) row);
    size = world.size
  }

let renderWorld (world: World): string =
  world.cells
  |> List.map (fun (row) -> row
                            |> List.map renderCell
                            |> String.concat "")
  |> String.concat "\n"
