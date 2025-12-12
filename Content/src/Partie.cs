using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Serialization;

[Serializable]

[XmlRoot("partie", Namespace = "http://www.l3miage.fr/parties")]
//cette classe nous sert à la sérialisation. Elle va nous servir à créer un objet Partie (qui contient les informations sur la partie jouee)
//qui sera ajouté à un autre objet Parties (qui contient une liste de partie) qui lui aussi sera sérialisé en fichier parties.xml pour voir l'historique des parties jouees
public class Partie
{
    [XmlElement("joueurPartie")] public String _joueurPartie { get; set; }

    [XmlElement("datePartie")] public String _DatePartie { get; set; }
    //duration en XML devient TimeSpan en C#
    [XmlElement("tempsPartie")] public String _TempsPartie { get; set; }
    
    [XmlElement("victoire")] public bool _victoire { get; set; }

    public Partie(String joueurPartie, String datePartie, String tempsPartie,  bool victoireOuPas)
    {
        _joueurPartie = joueurPartie;
        _DatePartie = datePartie;
        _TempsPartie = tempsPartie;
        _victoire = victoireOuPas;
    }

    public Partie()
    {
        
    }
    
    
}