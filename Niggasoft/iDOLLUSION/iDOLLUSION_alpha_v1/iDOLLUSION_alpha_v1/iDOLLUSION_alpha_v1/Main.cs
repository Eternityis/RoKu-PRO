using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace iDOLLUSION_alpha_v1
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
      public static  Texture2D background, splash, schedule, silverButton, goldButton, silverButtonR, goldButtonR, mouseIcon, mainmenu, button, sparkle, characterSelection, characterSelected, characterUnselected, mainMap, collisionMap, producer;
      public    SpriteFont gameFont;
      public    Rectangle backgroundRect, silverButtonRect, goldButtonRect, mouseIconRect, mainmenuRect, buttonExitRect, buttonStartRect, character1Rect, character2Rect, producerRect;
        private int screenWidth, screenHeight;



//determines the direction of the arrows on splash screen. Needs cleaning. Move to seperate class?
         int directionSilver = 1;
         int directionGold = 1;


//Sound Effects and Songs are declared here
        private SoundEffect edenEffect;
         Song techworld;

//default producer location. Theres a better way to do this
    static  int producerX = 570;
    static  int producerY = 660;


//Main Menu button location stuff
int buttonSizeDir = 1;
 private Vector2 exitStringCoord = new Vector2(637, 520);
 private Vector2 startStringCoord = new Vector2(637, 380);

//Project versino number, for version control. Just forget about it for now
public double VERSION = .01;  

         
int splashTimer = 0;
 Random rnd = new Random();


//ENUMS GO HERE
        public  enum Scene  //Which screen to display.  Every additional location needs an entry
        {
            Splash, //Progression through menu screens is handled via currentScene++;
            MainMenu,
            CharacterSelection, //This shouldnt be accessible except when chosenIdol = Idols.None
            MainMap,
            Home, //vvv are unimplemented thus far
            Office,
            Studio,
            Schedule
        };

    public static Scene currentScene = Scene.Splash; //This sets the first screen displayed as the Splash screen


        public enum Idols  //Pick a character, any character. These are just placeholder names
        {
            Haruhi,
            Sayaka,
            None
        };
        private Idols chosenIdol = Idols.None;  //this allows us to bring up the character selection screen only once



        // object arrays
    //testing

//PUBLIC METHODS
        public static int getProducer(string c)
        {
        switch (c)
            {
            case ("X"):
                    {
                    return producerX;
                    }
default:
                {
                    return producerY;
                }
            }
        }


        public static void setProducerLocation(int x, int y)
        {
            producerX = x;
            producerY = y;
        }



        public  void setGameScreen(Scene scene)
        {
            if (currentScene != scene)
            {
                if (scene == Scene.CharacterSelection && chosenIdol != Idols.None)
                {
                    return;
                }
                else
                {
                    currentScene = scene;
                 //   Collision.resetProducerLocation();
                }
            }
            return;
        }

        public  Scene getCurrentScreen()
        {
            return (currentScene);
        }

      public  bool clicked()
            {
          if (Mouse.GetState().LeftButton == ButtonState.Pressed)
          {
              return true;
          }
                    return false;
            }





/////////////////////////////////////////////







        public Main()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
         //   string toProducer = "ToProducer";
        //    XMLReader.readText(toProducer);

        }
        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            backgroundRect = new Rectangle(0,0,screenWidth,screenHeight);
            mainmenuRect = new Rectangle(0,0,screenWidth,screenHeight);
            silverButtonRect = new Rectangle(230,410,90,49);
            goldButtonRect = new Rectangle(60,0,60,29);
            buttonExitRect = new Rectangle(510, 500, 300, 100);
            buttonStartRect = new Rectangle(510, 360, 300, 100);
            character1Rect = new Rectangle(200, 300, 300, 200);
            character2Rect = new Rectangle(800, 300, 300, 200);
            producerRect = new Rectangle(producerX, producerY, 50, 50);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
//Textures
            silverButton = Content.Load <Texture2D> ("sprites/silverArrow");
            button = Content.Load<Texture2D>("sprites/button");
            goldButton = Content.Load<Texture2D>("sprites/goldenArrow");
            silverButtonR = Content.Load <Texture2D> ("sprites/silverArrowReversed");
            goldButtonR = Content.Load<Texture2D>("sprites/goldenArrowReversed");
            sparkle = Content.Load<Texture2D>("sprites/goldButton");
            characterSelection = Content.Load<Texture2D>("images/characterSelection");
            characterSelected = Content.Load <Texture2D> ("sprites/characterSelected");
            characterUnselected = Content.Load<Texture2D>("sprites/characterUnselected");
            background = Content.Load<Texture2D>("images/background");
            mainmenu = Content.Load<Texture2D>("images/mainmenu");
            splash = Content.Load<Texture2D>("images/splash");
            gameFont = Content.Load<SpriteFont>("fonts/gameFont");
            mouseIcon = Content.Load<Texture2D>("sprites/mouseIcon");
            collisionMap = Content.Load<Texture2D>("images/mainMap/collisionMap");
            mainMap = Content.Load<Texture2D>("images/mainMap/mainMap");
            producer = Content.Load<Texture2D>("images/characters/producer");
            schedule = Content.Load<Texture2D>("images/schedule/schedule");
//Sounds
            edenEffect = Content.Load<SoundEffect>("sounds/eden");
            techworld = Content.Load<Song>("sounds/techworld");

            MediaPlayer.Play(techworld);
            MediaPlayer.IsRepeating = true;



        }
        protected override void UnloadContent()
        {
        }

//GAME UPDATE LOOP HERE
        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

 



            if (currentScene == Scene.MainMap)
            {
                if (ks.IsKeyDown(Keys.P))
                {
                   setGameScreen(Scene.Schedule);
                }
                //movement controls for main map go here
                if (ks != null)
                {
                    int movementSpeed = 7;
                    Vector2 producerShift = new Vector2(0,0);
                    if(ks.IsKeyDown(Keys.W))
                    {
                        producerShift.Y-=movementSpeed;
                    }
                    if(ks.IsKeyDown(Keys.S))
                    {
                        producerShift.Y+=movementSpeed;
                    }
                    if(ks.IsKeyDown(Keys.A))
                    {
                        producerShift.X-=movementSpeed;
                    }
                    if(ks.IsKeyDown(Keys.D))
                    {
                        producerShift.X+=movementSpeed;
                    }
                    Collision.processMovement(producerShift);
                }
            }
            if (currentScene == Scene.Schedule)
            {
                if (ks.IsKeyDown(Keys.W))
                {
                   setGameScreen(Scene.MainMap);
                }
            }


 if (Keyboard.GetState().IsKeyDown(Keys.Space)) Exit();
          
  if (currentScene == Scene.Splash && (Mouse.GetState().LeftButton == ButtonState.Pressed))
            {
                currentScene++;   //Clicking on teh splash proceeds tothe next screen

if (!edenEffect.IsDisposed) //check if already unloaded, if not play, otherwise, reload
                {
                                    edenEffect.Play();
                }
else
{
    //add code that reloads edenEffect
}
            }
            mouseIconRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, mouseIcon.Width, mouseIcon.Height); //Mouse icon
            base.Update(gameTime);
        }












//DRAW LOOP HERE
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
       var mouseState = Mouse.GetState();              // use mouseState instead of Mouse.GetState() from here on out


        Point mousePosition = new Point(mouseState.X, mouseState.Y);
            spriteBatch.Begin();
            if (currentScene == Scene.Splash) //SPLASH DRAW LOOP
            {
                spriteBatch.Draw(splash, backgroundRect, Color.White);
                silverButtonRect.X += 5*directionSilver;
                goldButtonRect.X += 6*directionGold;
               
 //handle collisions between arrows and walls
                if (silverButtonRect.X == 220 || silverButtonRect.X == 510)
                {
                    directionSilver *= -1;
                }
                if (goldButtonRect.X < 0 || goldButtonRect.X + goldButtonR.Width > 1280)
                {
                    directionGold *= -1;
                }

                spriteBatch.DrawString(gameFont, "Ver: " + VERSION, new Vector2(0, 690), Color.White);  //Draw game version number
             
   splashTimer++;     //Automatically proceed to main menu after a time
                if (splashTimer > 2000)
                {
                    currentScene++;
                    splashTimer = 0;
                    edenEffect.Play();
                }
//Flip arrow sprite as neccessary
                if (directionSilver > 0)
                {
                    spriteBatch.Draw(silverButton, silverButtonRect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(silverButtonR, silverButtonRect, Color.White);
                }
                if (directionGold > 0)
                {
                    spriteBatch.Draw(goldButton, goldButtonRect, Color.White);
                }
                if (directionGold < 0)
                {
                    spriteBatch.Draw(goldButtonR, goldButtonRect, Color.White);
                }
            }
////////////////////////////////////////////////////////////////////////////////////
/// 
/// 
            else if (currentScene == Scene.MainMenu) //MAIN MENU DRAW LOOP
            {
                MediaPlayer.Stop();
                techworld.Dispose();

                spriteBatch.Draw(mainmenu, backgroundRect, Color.White);
                spriteBatch.Draw(button, buttonExitRect, Color.White);
                spriteBatch.Draw(button, buttonStartRect, Color.White);
                spriteBatch.DrawString(gameFont, "Exit", exitStringCoord, Color.Brown);
                    //TODO change font size, move dynamically
                spriteBatch.DrawString(gameFont, "Start", startStringCoord, Color.DarkGoldenrod);
                    //TODO change font size, move dynamically

                /////// make buttons grow and shrink                   
                if (buttonSizeDir == 1) //Whether the button is shrinking or growing
                {
                    exitStringCoord.X += 2;
                    buttonExitRect.Width += 2;
                    buttonExitRect.X += 1;
                    startStringCoord.X -= 2;
                    buttonStartRect.X -= 1;
                    buttonStartRect.Width -= 2;
                    if (buttonExitRect.Width > 310)
                    {
                        buttonSizeDir = 0;
                    }
                }
                if (buttonSizeDir == 0)
                {
                    exitStringCoord.X -= 2;
                    buttonExitRect.Width -= 2;
                    buttonExitRect.X -= 1;
                    startStringCoord.X += 2;
                    buttonStartRect.X += 1;
                    buttonStartRect.Width += 2;
                    if (buttonExitRect.Width < 290)
                    {
                        buttonSizeDir = 1;
                    }
                } // End of button resizing


                if (buttonExitRect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    Exit();
                }
                if (buttonStartRect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    if (chosenIdol == Idols.None)
                    {
                        currentScene++;
                    }
                    else
                    {
                        currentScene += 2;
                    }

                }

            }
/////////////////////////////////////////////////////////////////////////
 
            if (currentScene == Scene.CharacterSelection)  //CHARACTER SELECTION DRAW LOOP
            {
              spriteBatch.Draw(characterSelection, backgroundRect, Color.White);// Draw appropriate background

                           // This makes the portrait you hover over glow
                if (character1Rect.Contains(mousePosition))      
                {
                    spriteBatch.Draw(characterSelected, character1Rect, Color.White);
                }
                else
                {
                  spriteBatch.Draw(characterUnselected, character1Rect, Color.White);

                }
                if (character2Rect.Contains(mousePosition))
                {
                    spriteBatch.Draw(characterSelected, character2Rect, Color.White);
                }
                else
                {
                  spriteBatch.Draw(characterUnselected, character2Rect, Color.White);

                }

//Determine which button has been selected
                if (character2Rect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)  //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    chosenIdol = Idols.Haruhi;
                    currentScene++;
                }
                if (character1Rect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)  //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    chosenIdol = Idols.Sayaka;
                    currentScene++;
                }


            }
/////////////////////////////////////////////////////////////////////////


            else if (currentScene == Scene.MainMap) //MAIN MAP DRAW LOOP
                {
                    
                //draw main navigational map and invisible collision map.  Remember to go in order [back to front}
                spriteBatch.Draw(collisionMap, new Rectangle(0,0,screenWidth,screenHeight), Color.White);
                spriteBatch.Draw(mainMap, backgroundRect, Color.White);
                spriteBatch.Draw(producer,  new Rectangle(producerX, producerY , 40, 40), Color.White);
                edenEffect.Dispose();
                }

            else if (currentScene == Scene.Schedule) //MAIN MAP DRAW LOOP
                {
                    
                spriteBatch.Draw(schedule, backgroundRect, Color.White);
                }
////////////////////////////////////////////////////////////////////////
 
//ALWAYS DRAW LOOP
            spriteBatch.Draw(mouseIcon, mouseIconRect, Color.White);
            spriteBatch.DrawString(gameFont, Mouse.GetState().X.ToString() + " " + Mouse.GetState().Y.ToString(), new Vector2(0, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
