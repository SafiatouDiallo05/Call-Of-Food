using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Serialization;


namespace BasicMonoGame;
//classe qui implémente la logique de notre jeu. Pour chaque méthode que nous avons nous-même implémenter, nous expliquons à quoi elle nous sert.
    public class MyGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int tileSize = 40;

        private Labyrinthe labyrinthe;
        private Joueur joueur;
        private Objectif objectif;//
        private bool victoire = false;
        private bool echec = false;//
        
        private Texture2D joueurTexture;//
        private Texture2D objectifTexture;//
        private Texture2D murTexture;
        private Texture2D solTexture;
        private Texture2D victoireTexture;//
        private Texture2D echecTexture;//

        private Sprite _joueurSprite;
        private Sprite _objectifSprite;

        private double moveTimer = 0;//
        private double moveInterval = 150; // ms
        private DateTime heureDebut;//
        
        
        private const double TEMPS_TOTAL_EN_SECONDES = 60.0; // Durée totale de la partie en secondes
        private double _tempsRestant; //temps restant pour le joueur
        private SpriteFont _policeTemps;//police du TImer
        
        public MyGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           Exiting += Game1_Exiting; //Evenement nous permettant d'appeller Game1_Exiting juste avant de quitter le jeu
        }

        //methode nous permettant d'enregistrer une partie dans parties.xml
        //et de générer et enregistrer le fichier ClassementParties.html contenant le classement des parties gagnées
        private void Game1_Exiting(object sender, Microsoft.Xna.Framework.ExitingEventArgs e)
        {
           enregistrerPartie("./Content/data/parties.xml");
           XslTransform(
               "./Content/data/parties.xml", "./Content/data/ClassementParties.xsl",
               "./Content/data/ClassementParties.html");
        }
         
        protected override void Initialize()
        {
            Validateur.ValidateXmlFile("http://www.l3miage.fr/niveau", "./Content/data/niveau.xml",
                "./Content/data/niveau.xsd");
            labyrinthe = new Labyrinthe("./Content/data/niveau.xml");
            joueur = new Joueur("./Content/data/niveau.xml");
            objectif = new Objectif("./Content/data/niveau.xml");

            _graphics.PreferredBackBufferWidth = labyrinthe.get_Largeur() * tileSize; //largeur de la fenêtre
            _graphics.PreferredBackBufferHeight = labyrinthe.get_Hauteur() * tileSize;//hauteur de la fenêtre
            _graphics.ApplyChanges();
            
            heureDebut = DateTime.Now;
            _tempsRestant = TEMPS_TOTAL_EN_SECONDES;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

            joueurTexture = Content.Load<Texture2D>("images/" + joueur.getNomSpriteJoueur());
            objectifTexture = Content.Load<Texture2D>("images/" + objectif.getNomSpriteObjectif());
            murTexture = Content.Load<Texture2D>("images/mur");
            solTexture = Content.Load<Texture2D>("images/sol");
            victoireTexture = Content.Load<Texture2D>("images/victoire");
            echecTexture = Content.Load<Texture2D>("images/echec");
            _policeTemps = Content.Load<SpriteFont>("fonts/04B_30");
           

            _joueurSprite = new Sprite(joueurTexture, new Vector2(joueur.getX() * tileSize, joueur.getY() * tileSize), tileSize);
            _objectifSprite = new Sprite(objectifTexture, new Vector2(objectif.getX() * tileSize, objectif.getY() * tileSize), tileSize);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();
            moveTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            
            double tempsPasse = gameTime.ElapsedGameTime.TotalSeconds;
            
            // Décrémenter le temps restant
            if (_tempsRestant > 0 && !victoire)
            {
                _tempsRestant -= tempsPasse;
            }
            
            //logique d'échec
            if (_tempsRestant <= 0)//si le temps est ecoulé
            {
                _tempsRestant = 0;
                echec = true;
                victoire = false;
                
            }
            
            //logique de victoire
            if (joueur.getPosition() == objectif.getPosition())
            {
                victoire = true;
                echec = false;
                
            }
                

            if (!victoire && moveTimer >= moveInterval || !echec && moveTimer >= moveInterval)
            {
                Point move = Point.Zero;

                if (k.IsKeyDown(Keys.Up)) move.Y = -1;
                else if (k.IsKeyDown(Keys.Down)) move.Y = 1;
                else if (k.IsKeyDown(Keys.Left)) move.X = -1;
                else if (k.IsKeyDown(Keys.Right)) move.X = 1;

                if (move != Point.Zero)
                {
                    joueur.Deplacer(move.X, move.Y, labyrinthe);
                    moveTimer = 0;
                }
            }
            // Mettre à jour la position du sprite selon la grille
            _joueurSprite._Position = new Vector2(joueur.getX() * tileSize, joueur.getY() * tileSize);
            
            if (victoire && k.IsKeyDown(Keys.R)|| echec && k.IsKeyDown(Keys.R))
                Reinitialiser();
            
            if (k.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        //cette méthode nous permet de faire tous les changements nécessaires lorsque le joueur décide de rejouer (appuyer su 'R')
        private void Reinitialiser()
        {
            enregistrerPartie("./Content/data/parties.xml");
            victoire = false;
            echec = false;
            joueur = new Joueur("./Content/data/niveau.xml");
            _joueurSprite._Position = new Vector2(joueur.getX() * tileSize, joueur.getY() * tileSize);
            _tempsRestant = TEMPS_TOTAL_EN_SECONDES;
        }

        //Cette méthode nous permet d'appliquer une transformation xslt à un fichier xml et d'enregistrer le résultat dans un fichier html
        private void XslTransform(String xmlFilePath, String xsltFilePath, String htmlFilePath)
        {
            XPathDocument xpathDoc = new XPathDocument(xmlFilePath);
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltFilePath);
            XmlTextWriter htmlWriter = new XmlTextWriter(htmlFilePath, null);
            xslt.Transform(xpathDoc, null, htmlWriter);
            htmlWriter.Close();
        }

        //Cette méthode nous permet d'enregistrer une partie de jeu. La logique est :
        //On déserialise parties.xml pour obtenir un objet parties.cs (s'il n'existe pas encore, on commence par créer un objet Parties.cs)
        //on crée un objet partie.cs contenant les données de notre partie
        //on ajoute l'objet partie.cs à la liste contenue dans l'objet Parties.cs
        //on re-sérialise Parties.cs pour avoir parties.xml
        private void enregistrerPartie(String filename)
        {
            XMLManager<Parties> Parties_Serializer;
            Parties_Serializer = new XMLManager<Parties>();
            
            double tempsConsomme = TEMPS_TOTAL_EN_SECONDES - _tempsRestant;
            TimeSpan tempsEcoule = TimeSpan.FromSeconds(tempsConsomme);
            String tempsEcouleFormat = tempsEcoule.ToString(@"mm\:ss\:ff");
            String dateFormat = System.DateTime.Now.ToString(@"dd/MM/yyyy HH:mm");
            Partie partieJouee = new Partie(joueur.getNomSpriteJoueur(), dateFormat, tempsEcouleFormat, victoire);
            Parties listeParties;
            
            bool fichierExiste = File.Exists(filename);

            // Si le fichier existe, on vérifie s'il est vide
            if (fichierExiste)
            {
                FileInfo fi = new FileInfo(filename);
                if (fi.Length == 0) // Si le fichier ne contient rien
                {
                    fichierExiste = false; // Traiter le fichier comme inexistant
                }
            }
            
            if (!fichierExiste)
            { 
                listeParties = new Parties();
            
            }
            else
            {
                listeParties = Parties_Serializer.Load(filename);
            }
            listeParties._PartiesJouees.Add(partieJouee);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("pa", "http://www.l3miage.fr/parties");
            Parties_Serializer.Save(filename, listeParties, ns);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            for (int y = 0; y < labyrinthe.get_Largeur(); y++)
            {
                for (int x = 0; x < labyrinthe.get_Hauteur(); x++)
                {
                    Texture2D texture = labyrinthe.get_Grille(x, y) == 1 ? murTexture : solTexture;
                    _spriteBatch.Draw(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.White);
                }
            }

            _objectifSprite.Draw(_spriteBatch);
            _joueurSprite.Draw(_spriteBatch);
            
            // Convertir les secondes en duree pour l'affichage
            TimeSpan tempsFormat = TimeSpan.FromSeconds(_tempsRestant);

            // L'affichage sera en (Minutes:Secondes)
            string tempsAffiche = tempsFormat.ToString(@"mm\:ss"); 
    
            // Définir la position d'affichage (ex: en haut au centre)
            Vector2 tailleTexte = _policeTemps.MeasureString(tempsAffiche);
            Vector2 positionAffichage = new Vector2(labyrinthe.get_Largeur() * tileSize / 2 - tailleTexte.X / 2, 10);
    
            // Couleur rouge si le temps est presque écoulé
            Color couleur = (_tempsRestant <= 10) ? Color.Black : Color.White;

            _spriteBatch.DrawString(_policeTemps, tempsAffiche, positionAffichage, couleur);

            if (victoire)
                _spriteBatch.Draw(victoireTexture, new Rectangle(0,0, labyrinthe.get_Largeur() * tileSize/2,labyrinthe.get_Hauteur() * tileSize/2), Color.White);
            
            if(echec)
                _spriteBatch.Draw(echecTexture, new Rectangle(0,0, labyrinthe.get_Largeur() * tileSize/2,labyrinthe.get_Hauteur() * tileSize/2), Color.White);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }