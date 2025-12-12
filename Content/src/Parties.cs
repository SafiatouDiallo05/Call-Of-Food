using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Serialization;

[Serializable]

[XmlRoot("parties", Namespace = "http://www.l3miage.fr/parties")]
//cette classe va nous permettre, après instanciation, ajout d'une Partie puis sérialisation, de stocker l'historique des parties jouees
public class Parties
{
    [XmlElement("partie")] public List<Partie> _PartiesJouees { get; set; } = new List<Partie>();
}