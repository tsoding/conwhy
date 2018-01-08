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
        this.GraphicsDevice.Clear Color.Blue
        let viewport = this.GraphicsDevice.Viewport
        spriteBatch.Begin();
        spriteBatch.DrawLine(new Vector2(0.0f, 0.0f), new Vector2(float32 viewport.Width, float32 viewport.Height), Color.Red, 10.0f);
        spriteBatch.DrawLine(new Vector2(float32 viewport.Width, 0.0f), new Vector2(0.0f, float32 viewport.Height), Color.Red, 10.0f);
        spriteBatch.End();
        ()

[<EntryPoint>]
let main _ =
    use g = new MonogameRunner()
    g.Run()
    0
