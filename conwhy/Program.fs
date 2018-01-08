open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
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

    override this.Initialize() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        base.Initialize()
        ()

    override this.LoadContent() =
        ()

    override this.Update(gameTime) =
        ()

    override this.Draw(gameTime) =
        this.GraphicsDevice.Clear(Color(24, 24, 24))
        let viewport = this.GraphicsDevice.Viewport
        spriteBatch.Begin();
        this.DrawGrid(100, 100);
        spriteBatch.End();
        ()

    member this.DrawGrid(rows: int, columns: int) =
        let viewport = this.GraphicsDevice.Viewport
        let cellHeight = (float32 viewport.Height) / (float32 rows)
        let cellWidth = (float32 viewport.Width) / (float32 columns)
        for i in 1 .. rows do
            spriteBatch.DrawLine(
                new Vector2(0.0f, float32 i * cellHeight),
                new Vector2(float32 viewport.Width, float32 i * cellHeight),
                Color(128, 128, 128))
        for i in 1 .. columns do
            spriteBatch.DrawLine(
                new Vector2(float32 i * cellWidth, 0.0f),
                new Vector2(float32 i * cellWidth, float32 viewport.Height),
                Color(128, 128, 128))
        ()

[<EntryPoint>]
let main _ =
    use g = new MonogameRunner()
    g.Run()
    0
