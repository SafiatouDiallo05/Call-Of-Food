using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

public static class Validateur // Classe pour valider des fichiers xml en fonction de leurs  schémas
//Ex: Validateur.ValidateXmlFile("http://www.l3miage.fr/parties", "./Content/data/parties.xml", "./Content/data/parties.xsd")
{
    // La méthode est statique peut être appelée sans instance
    public static async Task ValidateXmlFile(string schemaNameSpace, string xsdFilePath, string xmlFilePath)
    {
        var settings = new XmlReaderSettings();
        settings.Schemas.Add(schemaNameSpace, xsdFilePath);
        settings.ValidationType = ValidationType.Schema;
        Console.WriteLine("Nombre de schemas utilisés dans la validation : " + settings.Schemas.Count);
        settings.ValidationEventHandler += ValidationCallBack;
        var readItems = XmlReader.Create(xmlFilePath, settings);
        while(readItems.Read()){ }
    }

    private static void ValidationCallBack(object? sender, ValidationEventArgs e)
    {
        if (e.Severity.Equals(XmlSeverityType.Warning))
        {
            Console.Write("WARNING: ");
            Console.Write(e.Message);
        }else if (e.Severity.Equals(XmlSeverityType.Error))
        {
            Console.Write("ERROR: ");
            Console.WriteLine(e.Message);
        }
    }
}