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
    let previousMouseState = ref (Mouse.GetState())
    let previousKeyboardState = ref (Keyboard.GetState())

    member this.KeyPressed (keyboardState: KeyboardState) (key: Keys): bool =
        (!previousKeyboardState).IsKeyUp(key) && keyboardState.IsKeyDown(key)

    override this.Initialize() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        base.Initialize()
        ()

    override this.LoadContent() =
        ()

    override this.Update(gameTime) =
        let currentMouseState = Mouse.GetState()
        let currentKeyboardState = Keyboard.GetState()
        
        if this.KeyPressed currentKeyboardState Keys.Left
        then world := (!world) |> movePlayer Left |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.Right
        then world := (!world) |> movePlayer Right |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.Up
        then world := (!world) |> movePlayer Up |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.Down
        then world := (!world) |> movePlayer Down |> nextWorld
        else ()

        previousMouseState := currentMouseState
        previousKeyboardState := currentKeyboardState

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
