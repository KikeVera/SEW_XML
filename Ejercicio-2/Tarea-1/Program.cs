using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConvertidorXML
{
    class Program
    {
        static void Main(string[] args)
        {
            String XML = args[0];
            StringBuilder html = new StringBuilder();

            html.Append("<!DOCTYPE HTML>\n");
            html.Append("<html lang=\"es\">\n");
            html.Append("<head>\n");
            html.Append("<meta charset=\"UTF-8\"/>\n");
            html.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"estilo.css\" />\n");
            html.Append("<title>Arbol genealógico</title>\n");
            html.Append("</head>\n");
            html.Append("<body>\n");

            XmlReader lector = XmlReader.Create(XML);
            while (lector.Read())
            {

                if (lector.NodeType.Equals(XmlNodeType.Text))
                {
                    html.Append(lector.Value);
                }

                switch (lector.Name)
                {

                    case "arbol":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<h1> Arbol genealógico</h1> \n");
                        }
                        break;

                    case "persona":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<section>\n");
                            if (lector.HasAttributes)
                            {
                               
                                lector.MoveToNextAttribute();
                                html.Append("<h2>" + lector.Value);
                                lector.MoveToNextAttribute();
                                html.Append(" " + lector.Value+ "</h2>\n");
                              ;
                            }
                        }

                        break;

                    case "fechaNacimiento":
                    case "lugarNacimiento":
                    case "fechaFallecimiento":
                    case "lugarFallecimiento":
                    case "longitud":
                    case "latitud":
                    case "altitud":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<li>" + lector.Name + ": ");
                            
                           


                        }




                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</li>\n");

                        }
                        break;
                    case "datosNacimiento":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<section>\n");
                            html.Append("<h3> Datos de nacimiento </h3>\n");
                            html.Append("<ul>\n");
                       
                            
                           
                        }
                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</ul>\n");
                            html.Append("</section>\n");

                        }
                        break;
                    case "datosFallecimiento":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<section>\n");
                            html.Append("<h3> Datos de fallecimiento </h3>\n");
                            html.Append("<ul>\n");



                        }
                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</ul>\n");
                            html.Append("</section>\n");

                        }
                        break;

                    case "fotografias":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<section>\n");
                            html.Append("<h3> Galería de fotos </h3>\n");
                            
                        }
                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</section>\n");

                        }

                        break;

                    case "videos":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<section>\n");
                            html.Append("<h3> Galería de videos </h3>\n");
                        }
                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</section>\n");

                        }

                        break;

                    case "comentarios":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<section>\n");
                            html.Append("<h3> Comentarios </h3>\n");
                        }
                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</section>\n");
                            html.Append("</section>\n");

                        }

                        break;
                    case "foto":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                           
                              html.Append("<figure>");
                            if (lector.Read())
                            {
                                lector.MoveToNextAttribute();
                                html.Append("<img alt=\"" + lector.Value + "\" src=\"multimedia/" + lector.Value + "\">");
                            }
                            
                        }
                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</figure>\n");
                        }


                        break;

                    case "video":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            html.Append("<figure>\n");
                            if (lector.Read())
                            {
                                html.Append("<video src=\"multimedia/" + lector.Value + "\" controls preload=\"auto\"></video>\n");

                            }


                        }
                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</figure>\n");
                        }


                        break;
                    case "comentario":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {

                            html.Append("<p>");


                        }

                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            html.Append("</p>\n");
                        }


                       break;



                    default:
                        break;

                }




            }

            html.Append("</body>\n");
            html.Append("</html>\n");
            File.WriteAllText(args[1], html.ToString());


        }
    }
}
