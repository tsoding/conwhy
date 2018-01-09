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
        graphics.PreferredBackBufferWidth <- 1000
        graphics.PreferredBackBufferHeight <- 1000
        graphics.ApplyChanges()
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        base.Initialize()
        ()

    override this.LoadContent() =
        ()

    override this.Update(gameTime) =
        let currentMouseState = Mouse.GetState()
        let currentKeyboardState = Keyboard.GetState()
        
        if this.KeyPressed currentKeyboardState Keys.A
        then world := (!world) |> movePlayer Left |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.D
        then world := (!world) |> movePlayer Right |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.W
        then world := (!world) |> movePlayer Up |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.X
        then world := (!world) |> movePlayer Down |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.Q
        then world := (!world) |> movePlayer LeftUp |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.E
        then world := (!world) |> movePlayer RightUp |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.Z
        then world := (!world) |> movePlayer LeftDown |> nextWorld
        else if this.KeyPressed currentKeyboardState Keys.C
        then world := (!world) |> movePlayer RightDown |> nextWorld
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
