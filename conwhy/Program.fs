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
    let mutable world = ref (makeWorld [".+@.......";
                                        "..@.......";
                                        "@@@.......";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";
                                        "..........";])
    let previousMouseState = ref (Mouse.GetState())

    override this.Initialize() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        base.Initialize()
        ()

    override this.LoadContent() =
        ()

    override this.Update(gameTime) =
        let currentMouseState = Mouse.GetState()
        if ((!previousMouseState).LeftButton = ButtonState.Released && currentMouseState.LeftButton = ButtonState.Pressed)
        then world := nextWorld (!world)
        else ()
        previousMouseState := currentMouseState

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
