using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConvertidorKML
{
    class Program
    {
        static void Main(string[] args)
        {
            String XML = args[0];
            StringBuilder kml = new StringBuilder();
            XmlReader lector = XmlReader.Create(XML);
            kml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            kml.Append("<kml xmlns=\"http://www.opengis.net/kml/2.2\">\n");
            kml.Append("<Document>\n");
           
            string persona="";
            

            while (lector.Read())
            {


                switch (lector.Name)
                {

                    case "persona":
                        if (lector.HasAttributes) {
                            lector.MoveToNextAttribute();
                            persona = lector.Value;
                            lector.MoveToNextAttribute();
                            persona += " "+lector.Value;
                        }

                        break;

                    case "coordenadasNacimiento":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            kml.Append("<Placemark>\n");
                           

                                
                            kml.Append("<name> Nacimiento: " + persona + "</name>\n");
                          
                            kml.Append("<description>Aquí nacio: " + persona + "</description>\n");
                         

                            kml.Append("<Point>\n");
                            kml.Append("<coordinates>");



                        }

                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            kml.Append("</coordinates>\n");
                           
                            kml.Append("</Point>\n");
                            kml.Append("<altitudeMode>absoluto</altitudeMode>\n");
                            
                           
                            kml.Append("</Placemark>\n");

                        }
                        break;
                    case "coordenadasFallecimiento":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            kml.Append("<Placemark>\n");



                            kml.Append("<name> Fallecimiento: " + persona + "</name>\n");

                            kml.Append("<description>Aquí falleció: " + persona + "</description>\n");
                            kml.Append("<Point>\n");
                            kml.Append("<coordinates>");



                        }

                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            kml.Append("</coordinates>\n");
                            kml.Append("</Point>\n");
                            kml.Append("<altitudeMode>absoluto</altitudeMode>\n");
                            kml.Append("</Placemark>\n");

                        }
                        break;
                    case "longitud":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            lector.Read();
                            kml.Append(lector.Value+",");

                        }
                        break;

                    case "latitud":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            lector.Read();
                            kml.Append(lector.Value + ",");

                        }
                        break;

                    case "altitud":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            lector.Read();
                            kml.Append(lector.Value);

                        }
                        break;



                    default:
                        break;

                }

            }

            kml.Append("</Document>\n");
            kml.Append("</kml>\n");
            File.WriteAllText(args[1], kml.ToString());

        }

    }
}
