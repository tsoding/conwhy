module Game

type World = private {
  alive: Set<int * int>;
  size: int * int
}

let worldSize (world: World): int * int =
    world.size

let aliveCells (world: World): (int * int) list =
    Set.fold (fun l se -> se::l) [] world.alive

let makeWorld (map: string list): World = {
  size = (List.length map, String.length map.[0]);
  alive = map
          |> List.mapi (fun i row ->
              row
              |> Seq.toList
              |> List.mapi (fun j cell ->
                 if cell = '@'
                 then [(i, j)]
                 else [])
              |> List.concat)
          |> List.concat
          |> Set.ofList
}

let countNeighbors (world: World) ((i, j) : int * int): int =
  [for i in -1 .. 1 do
   for j in -1 .. 1 do
   yield (i, j)]
  |> List.filter (fun x -> x <> (0, 0))
  |> List.map (fun (di, dj) ->
           if Set.contains (i + di, j + dj) world.alive
           then 1
           else 0)
  |> List.fold (+) 0

let nextGenCell (world: World) ((i, j): int * int): (int * int) list =
  let neighbors = countNeighbors world (i, j) in
  match Set.contains (i, j) world.alive with
  | false when neighbors = 3 -> [(i, j)]
  | true when neighbors < 2 || neighbors > 3 -> []
  | false -> []
  | true -> [(i, j)]

let updateWorld (world: World): World =
  let (row, column) = world.size
  { alive = [for i in 0 .. (row - 1) do
             for j in 0 .. (column - 1) do
             yield nextGenCell world (i, j)]
            |> List.concat
            |> Set.ofList;
    size = world.size
  }