using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;

namespace Serialization;

//cette classe nous permet de créer un objet Joueur dont nous récupérons les attributs dans le fichier niveau.xml (paramètre du constructeur) avec le parser DOM
public class Joueur
{
    private Point position;
    private String typeJoueur;
    

    public Joueur(String filename)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);
        XmlNode root = doc.DocumentElement;
        XmlElement rootElt = (XmlElement)root;
        XmlElement labyrinthe = (XmlElement)rootElt.GetElementsByTagName("labyrinthe").Item(0);
        XmlElement depart = (XmlElement)rootElt.GetElementsByTagName("depart").Item(0);
        String positionX = depart.GetAttribute("x");
        String positionY = depart.GetAttribute("y");
        position = new Point(int.Parse(positionX), int.Parse(positionY));
        XmlElement joueur = (XmlElement)rootElt.GetElementsByTagName("joueur").Item(0);
        XmlElement leTypeJoueur = (XmlElement)rootElt.GetElementsByTagName("typeJoueur").Item(0);
        typeJoueur = leTypeJoueur.InnerText;
    }

    public String getNomSpriteJoueur()
    {
        return typeJoueur.ToLower();
    }
    
    public void Deplacer(int dx, int dy, Labyrinthe lab)
    {
        int nx = position.X + dx;
        int ny = position.Y + dy;
        if (nx >= 0 && nx < lab.get_Largeur() && ny >= 0 && ny < lab.get_Hauteur() && lab.get_Grille(nx,ny) == 0)
            position = new Point(nx, ny);
    }

    public int getX()
    {
        return position.X;
    }

    public int getY()
    {
        return position.Y;
    }

    public Point getPosition()
    {
        return position;
    }

}