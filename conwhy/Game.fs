module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open MonoGame.Extended

type World = private {
  alive: Set<int * int>;
  size: int * int;
  player: int * int
}

let modulo n m = ((n % m) + m) % m

let indexMap (map: string list): (char * int * int) list =
    map
    |> List.mapi (fun i row ->
        row
        |> Seq.toList
        |> List.mapi (fun j cell -> (cell, i, j)))
    |> List.concat

let makeWorld (map: string list): World =
    let indexedMap = indexMap map
    {
        size = (List.length map, String.length map.[0]);
        alive = indexedMap
                |> List.filter (fun (x, _, _) -> x = '@')
                |> List.map (fun (_, i, j) -> (i, j))
                |> Set.ofList
        player = (match indexedMap |> List.filter (fun (x, _, _) -> x = '+') with
                  | [('+', i, j)] -> (i, j)
                  | []            -> failwith "Not enough players"
                  | _             -> failwith "Too many players")
    }

let countNeighbors (world: World) ((i, j) : int * int): int =
  let (rows, columns) = world.size
  [for i in -1 .. 1 do
   for j in -1 .. 1 do
   yield (i, j)]
  |> List.filter (fun x -> x <> (0, 0))
  |> List.sumBy (fun (di, dj) ->
           if Set.contains (modulo (i + di) rows, modulo (j + dj) columns) world.alive
           then 1
           else 0)

let nextGenCell (world: World) ((i, j): int * int): (int * int) list =
  let neighbors = countNeighbors world (i, j) in
  match Set.contains (i, j) (Set.add world.player world.alive) with
  | false when neighbors = 3 -> [(i, j)]
  | true when neighbors < 2 || neighbors > 3 -> []
  | false -> []
  | true -> [(i, j)]

let nextWorld (world: World): World =
  let (row, column) = world.size
  match nextGenCell world world.player with
  | [nextPlayer] -> { world with alive = [for i in 0 .. (row - 1) do
                                          for j in 0 .. (column - 1) do
                                          yield nextGenCell world (i, j)]
                                         |> List.concat
                                         |> Set.ofList
                                         |> Set.remove nextPlayer
                                 player = nextPlayer
                    }
  | _ -> failwith "Game Over"

let renderWorld (world: World) (spriteBatch: SpriteBatch) (viewport: Viewport): unit =
    let (rows, columns) = world.size
    let cellWidth = (float32 viewport.Width) / (float32 columns)
    let cellHeight = (float32 viewport.Height) / (float32 rows)
    let renderCell (color: Color) ((row, column): (int * int)): unit =
        spriteBatch.FillRectangle(
            RectangleF(float32 column * cellWidth,
                       float32 row * cellHeight,
                       cellWidth, cellHeight),
            color)
    world.alive |> Set.iter (renderCell (Color (128, 128, 128)))
    world.player |> renderCell (Color (128, 0, 0))
