using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConvertidorSVG
{
    class Program
    {
        
        static void Main(string[] args)
        {
            String XML = args[0];
            StringBuilder svg = new StringBuilder();
            XmlReader lector = XmlReader.Create(XML);
            svg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            svg.Append("<svg width=\"auto\" height=\"auto\" style =\"overflow: visible \" version =\"1.1\" xmlns =\"http://www.w3.org/2000/svg\">\n");
            svg.Append("<text x = \"16em\" y = \"1.5em\" fill = \"black\" font-size = \"3em\">Arbol genealógico </text>\n");
            double x=180;
            double y=0;
            bool desplIzq=true;
            int nivel = 1;
           
            


            while (lector.Read())
            {


                switch (lector.Name)
                {
                    case "persona":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {

                                                                 
                            if (desplIzq) {
                                x =x- (120 / Math.Pow(nivel,2));
                                y += 10;
                                
                            }

                            else
                            {
                            
                                x =x+ (120 / Math.Pow(nivel, 2)) ;
                                y += 10;
                                

                            }
                            nivel++;

                            svg.Append("<ellipse cx = \"" + x.ToString().Replace(",",".") + "em\" cy = \"" + y.ToString().Replace(",", ".") + "em\" " +
                                "rx = \"5em\" ry = \"3em\" fill = \"blue\" />\n");

                            if (nivel <= 4)
                            {
                                svg.Append("<line x1=\"" + (x - (120 / Math.Pow(nivel, 2))).ToString().Replace(",", ".") + "em\" y1=\"" + (y + 7).ToString().Replace(",", ".") + "em\" " +
                                    "x2 =\"" + (x).ToString().Replace(",", ".") + "em\" y2 =\"" + (y + 3).ToString().Replace(",", ".") + "em\" " +
                                    "stroke =\"red\" stroke-width=\"0.1em\"></line>\n");
                                svg.Append("<line x1=\"" + (x + (120 / Math.Pow(nivel, 2))).ToString().Replace(",", ".") + "em\" y1=\"" + (y + 7).ToString().Replace(",", ".") + "em\" " +
                                    "x2 =\"" + (x).ToString().Replace(",", ".") + "em\" y2 =\"" + (y + 3).ToString().Replace(",", ".") + "em\" " +
                                    "stroke =\"red\" stroke-width=\"0.1em\"></line>\n");
                            }


                            if (lector.HasAttributes)
                            {

                                lector.MoveToNextAttribute();
                                svg.Append("<text x = \"" + (x-2).ToString().Replace(",", ".") + "em\" y = \"" + (y - 0.9).ToString().Replace(",", ".") + "em\" " +
                                "fill = \"white\" font-size = \"1em\">"+lector.Value+ "</text>\n");
                                lector.MoveToNextAttribute();
                                string[] apellidos = lector.Value.Split(" ");

                                if (apellidos.Length > 2) {
                                    apellidos = apellidosCompuestos(apellidos);
                                }
                                
                                svg.Append("<text x = \"" + (x - 2).ToString().Replace(",", ".") + "em\" y = \"" + (y+0.3).ToString().Replace(",", ".") + "em\" " +
                                "fill = \"white\" font-size = \"1em\">" + apellidos[0] + "</text>\n");

                                svg.Append("<text x = \"" + (x - 2).ToString().Replace(",", ".") + "em\" y = \"" + (y + 1.5).ToString().Replace(",", ".") + "em\" " +
                                "fill = \"white\" font-size = \"1em\">" + apellidos[1] + "</text>\n");




                            }


                        }

                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            nivel--;
                            if (desplIzq)
                            {
                                desplIzq = false;
                                x =x+ (120 / Math.Pow(nivel, 2));
                                y -= 10;
                            }

                            else
                            {
                                desplIzq = true;
                                x =x- (120 / Math.Pow(nivel, 2)) ;
                                y -= 10;
                            }
                            

                        }
                        break;
                   

                    default:
                        break;



                }

            }



            svg.Append("</svg>");
            File.WriteAllText(args[1], svg.ToString());

        }

        private static String[] apellidosCompuestos(String[] apellidos) {
            string[] nApellidos= new string[2];
            String a="";
            int n = 0;
            foreach (String apellido in apellidos) {
                a += apellido+" ";
                if (apellido.Length >= 4) {
                    nApellidos[n] = a;
                    a = "";
                    n++;
                }
                

            }

            return nApellidos;
        }


    }
}
