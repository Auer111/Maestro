using Maestro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Maestro.CS
{
    public class XmlParser
    {
        private static List<xmlElement> els;
        private static List<Attribute> ats;
        private static Regex tagname;
        private static Regex opentag;

        public static List<xmlElement> Parse(string xml)
        {
            els = new List<xmlElement>();
            ats = new List<Attribute>();
            tagname = new Regex(@"(?<=<)\w.+?(?=[ >\/])");
            opentag = new Regex(@"(?<=<)\w.+?(?=[>])");

            XElement document = XElement.Parse(xml);

            CheckNode(document);
            return els;
        }


        private static void CheckNode(XElement node)
        {
            if (node.Elements().Any())
            {
                els.Add(new xmlElement
                {
                    Name = tagname.Match(node.ToString()).Value,
                    Raw = opentag.Match(node.ToString()).Value,
                    Value = node.Value,
                    hasChildren = true,
                    Depth = node.AncestorsAndSelf().Count()
                });

                foreach (XElement child in node.Elements())
                {
                    CheckNode(child);
                }
            }
            else
            {
                els.Add(new xmlElement
                {
                    Name = tagname.Match(node.ToString()).Value,
                    Value = node.Value,
                    hasChildren = false,
                    Depth = node.AncestorsAndSelf().Count()
                });
            }

        }

    }

}