using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;

namespace Serialization;

//cette classe nous permet de créer un objet Objectif (objectif car le but du joueur est de le manger avant que le temps ne s'écoule)
//dont nous récupérons les attributs dans le fichier niveau.xml (paramètre du constructeur) avec le parser DOM
public class Objectif
{
    private Point position;
    private string typeObjectif;
    

    public Objectif(string filename)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);
        XmlNode root = doc.DocumentElement;
        XmlElement rootElt = (XmlElement)root;
        XmlElement labyrinthe = (XmlElement)rootElt.GetElementsByTagName("labyrinthe").Item(0);
        XmlElement arrivee = (XmlElement)rootElt.GetElementsByTagName("arrivee").Item(0);
        string positionX = arrivee.GetAttribute("x");
        string positionY = arrivee.GetAttribute("y");
        position = new Point(int.Parse(positionX), int.Parse(positionY));
        XmlElement objectif = (XmlElement)rootElt.GetElementsByTagName("objectif").Item(0);
        XmlElement leTypeObjectif = (XmlElement)rootElt.GetElementsByTagName("typeObjectif").Item(0);
        typeObjectif = leTypeObjectif.InnerText;
    }

    public string getNomSpriteObjectif()
    {
        return typeObjectif.ToLower();
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