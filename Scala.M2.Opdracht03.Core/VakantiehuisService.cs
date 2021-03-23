using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Newtonsoft;

namespace Scala.M2.Opdracht03.Core
{
    public class VakantiehuisService
    {
        public List<Vakantiehuis> Vakantiehuizen { get; private set; }
        public VakantiehuisService()
        {
            Vakantiehuizen = new List<Vakantiehuis>();
            DoSeeding();
            OrderList();
        }
        private void DoSeeding()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            path = path + "vakantiehuizen.json";
            if (File.Exists(path))
            {
                string inhoud = File.ReadAllText(path);
                Vakantiehuizen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Vakantiehuis>>(inhoud);
            }
            else
            {
                Vakantiehuizen.Add(new Vakantiehuis("001", "Zonnebloem", "Klaverstraat 1", "8000 Brugge", 75M, 2));
                Vakantiehuizen.Add(new Vakantiehuis("002", "Papaver", "Stoofstraat 5", "8000 Brugge", 95M, 2));
                Vakantiehuizen.Add(new Vakantiehuis("003", "Pissebloem", "Steenstraat 100", "8000 Brugge", 185M, 6));
            }
        }
        public void SaveListToJson()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            path = path + "vakantiehuizen.json";
            if (File.Exists(path))
                File.Delete(path);
            string inhoud = Newtonsoft.Json.JsonConvert.SerializeObject(Vakantiehuizen);
            File.WriteAllText(path, inhoud);
        }
        public void OrderList()
        {
            Vakantiehuizen = Vakantiehuizen.OrderBy(o => o.Huisnaam).ToList();
        }
        public void VoegVakantiehuisToe(Vakantiehuis vakantiehuis)
        {
            Vakantiehuizen.Add(vakantiehuis);
            OrderList();
        }
        public void VerwijderVakantiehuis(Vakantiehuis vakantiehuis)
        {
            Vakantiehuizen.Remove(vakantiehuis);
            OrderList();
        }

        //public Vakantiehuis ZoekVakantiehuis(string id)
        //{
        //    foreach (Vakantiehuis vakantiehuis in Vakantiehuizen)
        //    {
        //        if (vakantiehuis.ID == id)
        //        {
        //            return vakantiehuis;
        //        }
        //    }
        //    return null;
        //}

    }
}
