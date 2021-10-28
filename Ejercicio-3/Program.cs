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
            string punto="";
            string nombre = "";
            kml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            kml.Append("<kml xmlns=\"http://www.opengis.net/kml/2.2\">\n");
            kml.Append("<Document>\n");
            
            while (lector.Read())
            {


                switch (lector.Name)
                {


                    case "cp:CadastralParcel":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            kml.Append("<Placemark>\n");
                            if (lector.HasAttributes)
                            {

                                lector.MoveToNextAttribute();
                                kml.Append("<description> \n");
                                kml.Append(lector.Value+"\n");


                            }

                        }

                        else if (lector.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            
                           
                            kml.Append("<Style id='roja'>\n");
                            kml.Append("<LineStyle>\n");
                            kml.Append("<color>#ffff0000</color>\n");
                            kml.Append("<width>10</width>\n");
                            kml.Append("</LineStyle>\n");
                            kml.Append("</Style>\n");
                            kml.Append("</Placemark>\n");
                            kml.Append("<Placemark>\n");
                            kml.Append("<name>Referencia: " + nombre + "</name>\n");
                            kml.Append(punto);
                            kml.Append("</Placemark>\n");

                        }
                        break;
                  


                    case "gml:posList":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {
                            kml.Append("<LineString>\n");
                            kml.Append("<extrude>1</extrude>\n");
                            kml.Append("<tessellate>1</tessellate>\n");
                            kml.Append("<coordinates>\n");

                            lector.Read();
                            string[] coordUTM = lector.Value.Split(" ");
                            double longitud;
                            double latitud;
                            for (int i = 0;i< coordUTM.Length; i = i + 2) {
                                if (!coordUTM[i].Contains(".")) {
                                    coordUTM[i] += ".00";
                                }
                                if (!coordUTM[i+1].Contains("."))
                                {
                                    coordUTM[i+1] += ".00";
                                }
                                if (coordUTM[i].Split(".")[1].Length<2)
                                {
                                    coordUTM[i] += "0";
                                }
                                if (coordUTM[i+1].Split(".")[1].Length<2)
                                {
                                    coordUTM[i + 1] += "0";
                                }
                                UTMToLatLon(double.Parse(coordUTM[i]), double.Parse(coordUTM[i+1]),double.Parse(args[2]),out latitud,out longitud);
                              
                                kml.Append(longitud.ToString().Replace(",",".")+","+latitud.ToString().Replace(",", ".") +" "+ "\n");


                            }

                            kml.Append("</coordinates>\n");
                            kml.Append("</LineString>\n");

                        }
                        break;
                    

                    case "cp:areaValue":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {


                            lector.Read();

                            kml.Append( "Area: " + lector.Value + "m2 \n");
                            kml.Append("</description> \n");

                        }
                        break;

                    case "cp:nationalCadastralReference":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {


                            lector.Read();
                            kml.Append("<name>"+lector.Value+"</name>\n");
                            nombre = lector.Value;
                           

                        }
                        break;

                    case "gml:pos":
                        if (lector.NodeType.Equals(XmlNodeType.Element))
                        {

                            
                            lector.Read();
                            double longitud;
                            double latitud;
                            string[] coordUTM = lector.Value.Split(" ");
                            if (!coordUTM[0].Contains("."))
                            {
                                coordUTM[0] += ".00";
                            }
                            if (!coordUTM[1].Contains("."))
                            {
                                coordUTM[1] += ".00";
                            }
                            if (coordUTM[0].Split(".")[1].Length < 2)
                            {
                                coordUTM[0] += "0";
                            }
                            if (coordUTM[1].Split(".")[1].Length < 2)
                            {
                                coordUTM[1] += "0";
                            }
                            UTMToLatLon(double.Parse(coordUTM[0]), double.Parse(coordUTM[1]), double.Parse(args[2]), out latitud, out longitud);
                            punto="<Point>\n<coordinates>" + longitud.ToString().Replace(",", ".") + "," + latitud.ToString().Replace(",", ".") + "</coordinates>\n</Point>\n";


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

        public static void UTMToLatLon(double Easting, double Northing, double Zone,  out double latitude, out double longitude)
        {
            double DtoR = Math.PI / 180, RtoD = 180 / Math.PI;
            double a = 6378137, f = 0.00335281066474748071984552861852, northernN0 = 0,  E0 = 500000,
                n = f / (2 - f), k0 = 0.9996,
                A = a * (1 + (1 / 4) * Math.Pow(n, 2) + (1 / 64) * Math.Pow(n, 4) + (1 / 256) * Math.Pow(n, 6) + (25 / 16384) * Math.Pow(n, 8) + (49 / 65536) * Math.Pow(n, 10)) / (1 + n),
                beta1 = n / 2 - (2 / 3) * Math.Pow(n, 2) + (37 / 96) * Math.Pow(n, 3) - (1 / 360) * Math.Pow(n, 4) - (81 / 512) * Math.Pow(n, 5) + (96199 / 604800) * Math.Pow(n, 6) - (5406467 / 38707200) * Math.Pow(n, 7) + (7944359 / 67737600) * Math.Pow(n, 8) - (7378753979 / 97542144000) * Math.Pow(n, 9) + (25123531261 / 804722688000) * Math.Pow(n, 10),
                beta2 = (1 / 48) * Math.Pow(n, 2) + (1 / 15) * Math.Pow(n, 3) - (437 / 1440) * Math.Pow(n, 4) + (46 / 105) * Math.Pow(n, 5) - (1118711 / 3870720) * Math.Pow(n, 6) + (51841 / 1209600) * Math.Pow(n, 7) + (24749483 / 348364800) * Math.Pow(n, 8) - (115295683 / 1397088000) * Math.Pow(n, 9) + (5487737251099 / 51502252032000) * Math.Pow(n, 10),
                beta3 = (17 / 480) * Math.Pow(n, 3) - (37 / 840) * Math.Pow(n, 4) - (209 / 4480) * Math.Pow(n, 5) + (5569 / 90720) * Math.Pow(n, 6) + (9261899 / 58060800) * Math.Pow(n, 7) - (6457463 / 17740800) * Math.Pow(n, 8) + (2473691167 / 9289728000) * Math.Pow(n, 9) - (852549456029 / 20922789888000) * Math.Pow(n, 10),
                beta4 = (4397 / 161280) * Math.Pow(n, 4) - (11 / 504) * Math.Pow(n, 5) - (830251 / 7257600) * Math.Pow(n, 6) + (466511 / 2494800) * Math.Pow(n, 7) + (324154477 / 7664025600) * Math.Pow(n, 8) - (937932223 / 3891888000) * Math.Pow(n, 9) - (89112264211 / 5230697472000) * Math.Pow(n, 10),
                beta5 = (4583 / 161280) * Math.Pow(n, 5) - (108847 / 3991680) * Math.Pow(n, 6) - (8005831 / 63866880) * Math.Pow(n, 7) + (22894433 / 124540416) * Math.Pow(n, 8) + (112731569449 / 557941063680) * Math.Pow(n, 9) - (5391039814733 / 10461394944000) * Math.Pow(n, 10),
                beta6 = (20648693 / 638668800) * Math.Pow(n, 6) - (16363163 / 518918400) * Math.Pow(n, 7) - (2204645983 / 12915302400) * Math.Pow(n, 8) + (4543317553 / 18162144000) * Math.Pow(n, 9) + (54894890298749 / 167382319104000) * Math.Pow(n, 10),
                beta7 = (219941297 / 5535129600) * Math.Pow(n, 7) - (497323811 / 12454041600) * Math.Pow(n, 8) - (79431132943 / 332107776000) * Math.Pow(n, 9) + (4346429528407 / 12703122432000) * Math.Pow(n, 10),
                beta8 = (191773887257 / 3719607091200) * Math.Pow(n, 8) - (17822319343 / 336825216000) * Math.Pow(n, 9) - (497155444501631 / 1422749712384000) * Math.Pow(n, 10),
                beta9 = (11025641854267 / 158083301376000) * Math.Pow(n, 9) - (492293158444691 / 6758061133824000) * Math.Pow(n, 10),
                beta10 = (7028504530429621 / 72085985427456000) * Math.Pow(n, 10),
                delta1 = 2 * n - (2 / 3) * Math.Pow(n, 2) - 2 * Math.Pow(n, 3),
                delta2 = (7 / 3) * Math.Pow(n, 2) - (8 / 5) * Math.Pow(n, 3),
                delta3 = (56 / 15) * Math.Pow(n, 3),
                ksi = (Northing / 100 - northernN0) / (k0 * A), eta = (Easting / 100 - E0) / (k0 * A),
                ksi_prime = ksi - (beta1 * Math.Sin(2 * ksi) * Math.Cosh(2 * eta) + beta2 * Math.Sin(4 * ksi) * Math.Cosh(4 * eta) + beta3 * Math.Sin(6 * ksi) * Math.Cosh(6 * eta) + beta4 * Math.Sin(8 * ksi) * Math.Cosh(8 * eta) + beta5 * Math.Sin(10 * ksi) * Math.Cosh(10 * eta) +
                            beta6 * Math.Sin(12 * ksi) * Math.Cosh(12 * eta) + beta7 * Math.Sin(14 * ksi) * Math.Cosh(14 * eta) + beta8 * Math.Sin(16 * ksi) * Math.Cosh(16 * eta) + beta9 * Math.Sin(18 * ksi) * Math.Cosh(18 * eta) + beta10 * Math.Sin(20 * ksi) * Math.Cosh(20 * eta)),
                eta_prime = eta - (beta1 * Math.Cos(2 * ksi) * Math.Sinh(2 * eta) + beta2 * Math.Cos(4 * ksi) * Math.Sinh(4 * eta) + beta3 * Math.Cos(6 * ksi) * Math.Sinh(6 * eta)),
                sigma_prime = 1 - (2 * beta1 * Math.Cos(2 * ksi) * Math.Cosh(2 * eta) + 2 * beta2 * Math.Cos(4 * ksi) * Math.Cosh(4 * eta) + 2 * beta3 * Math.Cos(6 * ksi) * Math.Cosh(6 * eta)),
                taw_prime = 2 * beta1 * Math.Sin(2 * ksi) * Math.Sinh(2 * eta) + 2 * beta2 * Math.Sin(4 * ksi) * Math.Sinh(4 * eta) + 2 * beta3 * Math.Sin(6 * ksi) * Math.Sinh(6 * eta),
                ki = Math.Asin(Math.Sin(ksi_prime) / Math.Cosh(eta_prime));

            latitude = (ki + delta1 * Math.Sin(2 * ki) + delta2 * Math.Sin(4 * ki) + delta3 * Math.Sin(6 * ki)) * RtoD;
            
            double longitude0 = Zone * 6 * DtoR - 183 * DtoR;
            longitude = (longitude0 + Math.Atan(Math.Sinh(eta_prime) / Math.Cos(ksi_prime))) * RtoD;
            
        }

    }
}
