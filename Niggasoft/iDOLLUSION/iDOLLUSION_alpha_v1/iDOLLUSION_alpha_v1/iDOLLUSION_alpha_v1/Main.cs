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
      public static  Texture2D background, splash, silverButton, goldButton, silverButtonR, goldButtonR, mouseIcon, mainmenu, button, sparkle, characterSelection, characterSelected, characterUnselected, mainMap, collisionMap;
      public    SpriteFont gameFont;
      public    Rectangle backgroundRect, silverButtonRect, goldButtonRect, mouseIconRect, mainmenuRect, buttonExitRect, buttonStartRect, character1Rect, character2Rect;
        private int screenWidth, screenHeight;
//public variables     

         int directionSilver = 1;
         int directionGold = 1;
         int buttonSizeDir = 1;
        private Vector2 exitStringCoord = new Vector2(637, 520);
        private Vector2 startStringCoord = new Vector2(637, 380);
         Song techworld;
        SoundEffect edenEffect;
         SoundEffect nocturneEffect;
        public double VERSION = .01;  //VERSION NUMBER GOES HERE
        
         int splashTimer = 0;
        Random rnd = new Random();

        public enum Location
        {
            Splash,
            MainMenu,
            CharacterSelection,
            MainMap,
            Home,
            Office,
            Studio
        };

    Location currentLocation = Location.Splash;

        // object arrays
    //testing


        public Main()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            string toProducer = "ToProducer";
                    XMLReader.readText(toProducer);

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

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            edenEffect = Content.Load<SoundEffect>("sounds/eden");
            techworld = Content.Load<Song>("sounds/techworld");
            nocturneEffect = Content.Load<SoundEffect>("sounds/nocturne");
            collisionMap = Content.Load<Texture2D>("images/mainMap/collisionMap");
            mainMap = Content.Load<Texture2D>("images/mainMap/mainMap");

            MediaPlayer.Play(techworld);
            MediaPlayer.IsRepeating = true;



        }
        protected override void UnloadContent()
        {
        }

//GAME UPDATE LOOP HERE
        protected override void Update(GameTime gameTime)
        {
 

            
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) Exit();
            if (currentLocation == Location.Splash && (Mouse.GetState().LeftButton == ButtonState.Pressed))
            {
                currentLocation++;
                edenEffect.Play();
            }
            mouseIconRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, mouseIcon.Width, mouseIcon.Height);
            base.Update(gameTime);
        }


//DRAW LOOP HERE
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

       var mouseState = Mouse.GetState();
        Point mousePosition = new Point(mouseState.X, mouseState.Y);
            spriteBatch.Begin();
            if (currentLocation == Location.Splash)
            {
                spriteBatch.Draw(splash, backgroundRect, Color.White);
                silverButtonRect.X +=5*directionSilver;
                goldButtonRect.X +=6*directionGold;
                    //handle collisions with walls
                if (silverButtonRect.X == 220 || silverButtonRect.X == 510)
                {
                    directionSilver *= -1;
                }
                if (goldButtonRect.X < 0 || goldButtonRect.X + goldButtonR.Width > 1280)
                {
                    directionGold *= -1;
                }
            }
            if (currentLocation == Location.Splash)
            {
                spriteBatch.DrawString(gameFont,"Ver: "+ VERSION, new Vector2(0,690), Color.White );
                splashTimer++;
                if (splashTimer > 2000)
                {
                    currentLocation++;
                edenEffect.Play();
                }
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
            else if (currentLocation == Location.MainMenu)
            {
                MediaPlayer.Stop();
                techworld.Dispose();
                    spriteBatch.Draw(mainmenu, backgroundRect, Color.White);
                    spriteBatch.Draw(button, buttonExitRect, Color.White);
                    spriteBatch.Draw(button, buttonStartRect, Color.White);
                    spriteBatch.DrawString(gameFont, "Exit", exitStringCoord, Color.Brown ); //TODO change font size, move dynamically
                    spriteBatch.DrawString(gameFont, "Start", startStringCoord, Color.DarkGoldenrod ); //TODO change font size, move dynamically

                if (rnd.Next(0, 1) == 0)
                { 
                    new Sparkle();
                }
 /////// make buttons grow and shrink                   
                if (buttonSizeDir ==1)
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
                   exitStringCoord.X -=2;
                buttonExitRect.Width -= 2;
                buttonExitRect.X -= 1;
                    startStringCoord.X += 2;
                    buttonStartRect.X += 1;
                    buttonStartRect.Width += 2;
                    if (buttonExitRect.Width < 290)
                    {
                        buttonSizeDir = 1;
                    }
                }
////////

                int numSparkles = Sparkle.sparkles.Length;
                for (int i = 0; i != numSparkles; i++)
                {
                    Sparkle thisSparkle = Sparkle.sparkles[i];
                    if (thisSparkle != null && !thisSparkle.dead) //handle null when no sparkles are around
                    {
                        thisSparkle.updateLifespan();
                        spriteBatch.Draw(sparkle, thisSparkle.position, Color.White);
                        if (i == numSparkles)
                        {
                            i = 0;
                        }
                    }
                    else
                    {
                        spriteBatch.DrawString(gameFont,  "Not sparkly at all..." , new Vector2(200, 400), Color.Red);
                    }
                }



                if (buttonExitRect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)  //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    Exit();
                }
                if (buttonStartRect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)  //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    currentLocation++;

                }
                
            }
            if (currentLocation == Location.CharacterSelection)
            {
              spriteBatch.Draw(characterSelection, backgroundRect, Color.White);
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

                if (character2Rect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)  //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    currentLocation++;
                }
                if (character1Rect.Contains(mousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed)  //add buffer for previous clicks so that a single click doesnt trigger this from the previous screen
                {
                    currentLocation++;
                }


            }
            else if (currentLocation == Location.MainMap) //past splash and mainmenu
                {
                    
                //draw main navigational map and invisible collision map
                spriteBatch.Draw(collisionMap, new Rectangle(0,0,screenWidth,screenHeight), Color.White);
                spriteBatch.Draw(mainMap, backgroundRect, Color.White);

                edenEffect.Dispose();
                }
            spriteBatch.Draw(mouseIcon, mouseIconRect, Color.White);
            spriteBatch.DrawString(gameFont, Mouse.GetState().X.ToString() + " " + Mouse.GetState().Y.ToString(), new Vector2(0, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
