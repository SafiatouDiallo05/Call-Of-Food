using System.Drawing;
using System.Xml;

namespace Serialization;

//cette classe nous permet de créer un objet Labyrinthe dont nous récupérons les attributs dans le fichier niveau.xml (paramètre du constructeur) avec le parser DOM
public class Labyrinthe
{
    private int largeur;
    private int hauteur;
    public int[,] grille;

    public Labyrinthe(string filename)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);
        XmlNode root = doc.DocumentElement;
        XmlElement rootElt = (XmlElement)root;
        XmlElement labyrinthe = (XmlElement)rootElt.GetElementsByTagName("labyrinthe").Item(0);
        largeur = int.Parse(labyrinthe.GetAttribute("largeur"));
        hauteur = int.Parse(labyrinthe.GetAttribute("hauteur"));
        grille = new int[largeur, hauteur];
        XmlElement cases = (XmlElement)labyrinthe.GetElementsByTagName("cases").Item(0);
        XmlNodeList lignesLabyrinthe = cases.GetElementsByTagName("ligne");
        
        for (int y = 0; y < hauteur; y++)
        {
            string ligne = lignesLabyrinthe[y].InnerText;
            for (int x = 0; x < largeur; x++)
                grille[x, y] = ligne[x] == '1' ? 1 : 0;
        }
    }

    public int get_Largeur()
    {
        return largeur;
    }

    public int get_Hauteur()
    {
        return hauteur;
    }

    public int get_Grille(int x, int y)
    {
        return grille[x,y];
    }
}