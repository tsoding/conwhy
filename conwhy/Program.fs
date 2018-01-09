open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open MonoGame.Extended

open Game

type MonogameRunner () as this =
    inherit Game()
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable world = ref (makeWorld [".+........";
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
        then world := nextWorld (!world)
        else ()

    override this.Draw(gameTime) =
        this.GraphicsDevice.Clear(Color(24, 24, 24))
        spriteBatch.Begin();
        renderWorld !world spriteBatch this.GraphicsDevice.Viewport;
        spriteBatch.End();
        ()

[<EntryPoint>]
let main _ =
    use g = new MonogameRunner()
    g.Run()
    0
