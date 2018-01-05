open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Cell = Dead | Alive
type World = Cell list list

let parseCell (c: char): Cell =
  match c with
  | '.' -> Dead
  | '@' -> Alive

let makeWorld (map: string list): World =
  [for row in map do
     yield [for c in row do
              yield (parseCell c)]]

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
           match world.[i + di].[j + dj] with
           | Dead -> 0
           | Alive -> 1
         with
         | :? System.ArgumentException as ex -> 0)
  |> List.fold (+) 0

let nextGenCell (world: World) ((i, j): int * int): Cell =
  let neighbors = countNeighbors world (i, j) in
  match world.[i].[j] with
  | Dead when neighbors = 3 -> Alive
  | Alive when neighbors < 2 || neighbors > 3 -> Dead
  | x -> x

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

type Game1 () as this =
    inherit Game()
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    override this.Initialize() =
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        do base.Initialize()
        ()

    override this.LoadContent() = 
        ()

    override this.Update(gameTime) =
        ()

    override this.Draw(gameTime) = 
        do this.GraphicsDevice.Clear Color.Red
        ()

[<EntryPoint>]
let main _ =
    use g = new Game1()
    g.Run()
    0