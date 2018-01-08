open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open MonoGame.Extended

open Game

let rec cliRunner (world: World): int =
  match Console.ReadLine() with
  | "quit" -> 0
  | _ ->
     world |> renderWorld |> printfn "%s";
     world |> updateWorld |> cliRunner

type MonogameRunner () as this =
    inherit Game()
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable world = ref (makeWorld [".@........";
                                        "..@.......";
                                        "@@@.......";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";])
    let lastMouseState = ref (Mouse.GetState())
    let currentMouseState = ref (Mouse.GetState())

    override this.Initialize() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        base.Initialize()
        ()

    override this.LoadContent() =
        ()

    override this.Update(gameTime) =
        lastMouseState := !currentMouseState
        currentMouseState := Mouse.GetState()
        if ((!lastMouseState).LeftButton = ButtonState.Released && (!currentMouseState).LeftButton = ButtonState.Pressed)
        then world := updateWorld (!world)
        else ()

    override this.Draw(gameTime) =
        this.GraphicsDevice.Clear(Color(24, 24, 24))
        let viewport = this.GraphicsDevice.Viewport
        spriteBatch.Begin();
        this.DrawWorld(!world);
        this.DrawGrid (!world).size;
        spriteBatch.End();
        ()

    member this.DrawWorld (world: World) =
        let (rows, columns) = world.size
        let viewport = this.GraphicsDevice.Viewport
        let cellHeight = (float32 viewport.Height) / (float32 rows)
        let cellWidth = (float32 viewport.Width) / (float32 columns)
        world.cells
        |> List.iteri (fun i row ->
            row
            |> List.iteri (fun j cell ->
                match cell with
                | Alive -> spriteBatch.FillRectangle(
                               new RectangleF(float32 j * cellWidth,
                                              float32 i * cellHeight,
                                              cellWidth, cellHeight),
                               Color(128, 128, 128))
                | Dead -> ()))
        ()

    member this.DrawGrid(rows: int, columns: int) =
        let viewport = this.GraphicsDevice.Viewport
        let cellHeight = (float32 viewport.Height) / (float32 rows)
        let cellWidth = (float32 viewport.Width) / (float32 columns)
        for i in 1 .. rows do
            spriteBatch.DrawLine(
                new Vector2(0.0f, float32 i * cellHeight),
                new Vector2(float32 viewport.Width, float32 i * cellHeight),
                Color(50, 50, 50))
        for i in 1 .. columns do
            spriteBatch.DrawLine(
                new Vector2(float32 i * cellWidth, 0.0f),
                new Vector2(float32 i * cellWidth, float32 viewport.Height),
                Color(50, 50, 50))
        ()

[<EntryPoint>]
let main _ =
    use g = new MonogameRunner()
    g.Run()
    0
