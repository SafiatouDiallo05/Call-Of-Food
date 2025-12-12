<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:pa="http://www.l3miage.fr/parties"> 
    
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/">
        <html>
            <head>
                <title>Classement des Parties Jouées (et Gagnées) </title>
                <style>
                    body { font-family: sans-serif; background-color: #f4f4f4; padding: 20px; }
                    table { border-collapse: collapse; width: 60%; margin: 20px auto; background-color: white; }
                    th, td { border: 1px solid #ddd; padding: 10px; text-align: left; }
                    th { background-color: #007bff; color: white; }
                    tr:nth-child(even) { background-color: #f2f2f2; }
                </style>
            </head>
            <body>
                <h1>Tableau des Parties Gagnées</h1>
                <table>
                    <tr>
                        <th>Rang</th>
                        <th>Date de la partie</th>
                        <th>Temps de jeu (en secondes)</th>
                       <!-- <th>Nom du joueur</th>-->
                    </tr>

                    <xsl:apply-templates select="pa:parties/pa:partie[pa:victoire = 'true']">
                        <xsl:sort select="pa:tempsPartie" order="ascending"/>
                    </xsl:apply-templates>
                </table>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="pa:partie">

        <tr>
            <td><xsl:value-of select="position()"/></td>

            <td><xsl:value-of select="pa:datePartie"/></td>

            <td><xsl:value-of select="pa:tempsPartie"/></td>

        </tr>
    </xsl:template>
    

</xsl:stylesheet>