module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open MonoGame.Extended

type Direction = Left | Right | Up | Down | LeftUp | RightUp | LeftDown | RightDown

type World = private {
  alive: Set<int * int>;
  size: int * int;
  player: int * int
}

let private modulo n m = ((n % m) + m) % m

let private moveCoord (direction: Direction) ((row, column): (int * int)): (int * int) =
    match direction with
    | Left      -> (row    , column - 1)
    | Right     -> (row    , column + 1)
    | Up        -> (row - 1, column)
    | Down      -> (row + 1, column)
    | LeftUp    -> (row - 1, column - 1)
    | RightUp   -> (row - 1, column + 1)
    | LeftDown  -> (row + 1, column - 1)
    | RightDown -> (row + 1, column + 1)

let private wrapCoord ((rows, columns): (int * int)) ((row, column): (int * int)): (int * int) =
    (modulo row rows, modulo column columns)

let private indexMap (map: string list): (char * int * int) list =
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
                |> List.filter (fun (x, _, _) -> x = '@' || x = '+')
                |> List.map (fun (_, i, j) -> (i, j))
                |> Set.ofList
        player = (match indexedMap |> List.filter (fun (x, _, _) -> x = '+') with
                  | [('+', i, j)] -> (i, j)
                  | []            -> failwith "Not enough players"
                  | _             -> failwith "Too many players")
    }

let private countNeighbors (world: World) ((i, j) : int * int): int =
  let (rows, columns) = world.size
  [for i in -1 .. 1 do
   for j in -1 .. 1 do
   yield (i, j)]
  |> List.filter (fun x -> x <> (0, 0))
  |> List.sumBy (fun (di, dj) ->
           if Set.contains (modulo (i + di) rows, modulo (j + dj) columns) world.alive
           then 1
           else 0)

let private nextGenCell (world: World) ((i, j): int * int): (int * int) list =
  let neighbors = countNeighbors world (i, j) in
  match Set.contains (i, j) (Set.add world.player world.alive) with
  | false when neighbors = 3 -> [(i, j)]
  | true when neighbors < 2 || neighbors > 3 -> []
  | false -> []
  | true -> [(i, j)]

let private nextWorld (world: World): World =
  let (row, column) = world.size
  { world with alive = [for i in 0 .. (row - 1) do
                        for j in 0 .. (column - 1) do
                        yield nextGenCell world (i, j)]
                       |> List.concat
                       |> Set.ofList
  }

let makeTurn (direction: Direction) (world: World): World option =
    let world' = nextWorld world
    let player' = world.player
                  |> moveCoord direction
                  |> wrapCoord world.size
    if Set.contains player' world'.alive
    then Some { world' with player = player' }
    else None

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
    world.player |> renderCell (Color (200, 128, 128))
