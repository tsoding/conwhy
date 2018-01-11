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
    let mutable fontu = Unchecked.defaultof<SpriteFont>
    let initialWorld = makeWorld [".+........";
                                  "..@.......";
                                  "@@@...@...";
                                  ".....@.@..";
                                  "......@...";
                                  ".....@@@..";
                                  "..@@......";
                                  ".@@@..@@..";
                                  "..........";
                                  "..........";]
    let mutable world = ref (Some initialWorld)
    let previousMouseState = ref (Mouse.GetState())
    let previousKeyboardState = ref (Keyboard.GetState())

    member this.KeyPressed (keyboardState: KeyboardState) (key: Keys): bool =
        (!previousKeyboardState).IsKeyUp(key) && keyboardState.IsKeyDown(key)

    member this.PlayTurn (direction: Direction): unit =
        world := (!world) |> Option.bind (makeTurn direction)

    override this.Initialize() =
        this.IsMouseVisible <- true
        graphics.PreferredBackBufferWidth <- 1000
        graphics.PreferredBackBufferHeight <- 1000

        graphics.ApplyChanges()
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        base.Initialize()
        ()

    override this.LoadContent() =
        fontu <- this.Content.Load<SpriteFont>("Font")
        ()

    override this.Update(gameTime) =
        let currentMouseState = Mouse.GetState()
        let currentKeyboardState = Keyboard.GetState()
        
        if this.KeyPressed currentKeyboardState Keys.A
        then this.PlayTurn Left
        else if this.KeyPressed currentKeyboardState Keys.D
        then this.PlayTurn Right
        else if this.KeyPressed currentKeyboardState Keys.W
        then this.PlayTurn Up
        else if this.KeyPressed currentKeyboardState Keys.X
        then this.PlayTurn Down
        else if this.KeyPressed currentKeyboardState Keys.Q
        then this.PlayTurn LeftUp
        else if this.KeyPressed currentKeyboardState Keys.E
        then this.PlayTurn RightUp
        else if this.KeyPressed currentKeyboardState Keys.Z
        then this.PlayTurn LeftDown
        else if this.KeyPressed currentKeyboardState Keys.C
        then this.PlayTurn RightDown
        else if this.KeyPressed currentKeyboardState Keys.Space
        then world := Some initialWorld
        else ()

        previousMouseState := currentMouseState
        previousKeyboardState := currentKeyboardState

    override this.Draw(gameTime) =
        this.GraphicsDevice.Clear(Color(24, 24, 24))
        spriteBatch.Begin();
        match !world with
        | Some world -> renderWorld world spriteBatch this.GraphicsDevice.Viewport
        | None -> this.DrawGameOverScreen ()
        spriteBatch.End();
        ()

    member this.DrawGameOverScreen () =
        spriteBatch.DrawString(fontu, "Game Over. Press SPACE to restart.", Vector2(0.0f, 0.0f), Color(255, 128, 128))

[<EntryPoint>]
let main _ =
    use g = new MonogameRunner()
    g.Run()
    0
