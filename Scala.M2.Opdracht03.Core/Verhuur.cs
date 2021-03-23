using System;
using System.Collections.Generic;
using System.Text;

namespace Scala.M2.Opdracht03.Core
{
    public class Verhuur
    {
        public string VakantiehuisID { get; set; }
        public string Klant { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public int AantalDagen
        {
            get
            {
                return (Tot - Vanaf).Days;
            }
        }
        public Verhuur()
        { }
        public Verhuur(string vakantiehuisID, string klant, DateTime vanaf, DateTime tot)
        {
            VakantiehuisID = vakantiehuisID;
            Klant = klant;
            Vanaf = vanaf;
            Tot = tot;
        }
        public override string ToString()
        {
            return $"{Klant} : {Vanaf.ToString("ddd dd/MM/yyyy")} - {Tot.ToString("ddd dd/MM/yyyy")}";
        }
    }
}
