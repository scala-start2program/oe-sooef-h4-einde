using System; 
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Scala.M2.Opdracht03.Core
{
    public class VerhuurService
    {
        public List<Verhuur> Verhuringen;
        public VerhuurService()
        {
            Verhuringen = new List<Verhuur>();
            DoSeeding();
            SorteerVerhuringen();
        }
        private void DoSeeding()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            path = path + "verhuringen.json";
            if (File.Exists(path))
            {
                string inhoud = File.ReadAllText(path);
                Verhuringen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Verhuur>>(inhoud);
            }
            else
            {
                Verhuringen.Add(new Verhuur("001", "Willems Wim", new DateTime(2021, 05, 01), new DateTime(2021, 05, 07)));
                Verhuringen.Add(new Verhuur("001", "Anthierens An", new DateTime(2021, 05, 07), new DateTime(2021, 05, 10)));
                Verhuringen.Add(new Verhuur("003", "Hendrix Henri", new DateTime(2021, 01, 01), new DateTime(2021, 01, 15)));
                Verhuringen.Add(new Verhuur("003", "Karpers Karel", new DateTime(2021, 04, 15), new DateTime(2021, 04, 30)));
            }
        }
        public void SaveListToJson()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            path = path + "verhuringen.json";
            if (File.Exists(path))
                File.Delete(path);
            string inhoud = Newtonsoft.Json.JsonConvert.SerializeObject(Verhuringen);
            File.WriteAllText(path, inhoud);
        }
        public void VoegVerhuurToe(Verhuur verhuur)
        {
            Verhuringen.Add(verhuur);
            SorteerVerhuringen();
        }

        private void SorteerVerhuringen()
        {
            Verhuringen = Verhuringen.OrderByDescending(v => v.Vanaf).ToList();
        }
        public void VerwijderVerhuring(Verhuur verhuur)
        {
            Verhuringen.Remove(verhuur);
        }
        public void VerwijderingVakantiehuis(string vakantiehuisID)
        {
            //foreach(Verhuur verhuur in Verhuringen)
            //{
            //    if(verhuur.VakantiehuisID == vakantiehuisID)
            //    {
            //        Verhuringen.Remove(verhuur);
            //    }
            //}

            for(int v = Verhuringen.Count -1; v>= 0; v--)
            {
                if(Verhuringen[v].VakantiehuisID == vakantiehuisID)
                {
                    Verhuringen.RemoveAt(v);
                }
            }
        }
        public List<Verhuur> VerhuringenVanHuis(string vakantiehuisID)
        {
            List<Verhuur> verhuringenPerHuis = new List<Verhuur>();
            foreach(Verhuur verhuur in Verhuringen)
            {
                if(verhuur.VakantiehuisID == vakantiehuisID)
                {
                    verhuringenPerHuis.Add(verhuur);
                }
            }
            return verhuringenPerHuis;
        }
        public bool IsErOverlap(string vakantiehuisID, DateTime vanaf, DateTime tot)
        {
            foreach(Verhuur verhuur in Verhuringen)
            {
                if(verhuur.VakantiehuisID == vakantiehuisID)
                {
                    if (vanaf >= verhuur.Vanaf && vanaf < verhuur.Tot)
                    {
                        return true;
                    }
                    else if (tot > verhuur.Vanaf && tot <= verhuur.Tot)
                    {
                        return true;
                    }
                    else if (vanaf <= verhuur.Vanaf && tot >= verhuur.Tot)
                        return true;
                }
            }
            return false;
        }
        public bool IsErOverlap(Verhuur dezeVerhuur, string vakantiehuisID, DateTime vanaf, DateTime tot)
        {
            foreach (Verhuur verhuur in Verhuringen)
            {
                if (dezeVerhuur != verhuur &&  verhuur.VakantiehuisID == vakantiehuisID)
                {
                    if (vanaf >= verhuur.Vanaf && vanaf < verhuur.Tot)
                    {
                        return true;
                    }
                    else if (tot > verhuur.Vanaf && tot <= verhuur.Tot)
                    {
                        return true;
                    }
                    else if (vanaf <= verhuur.Vanaf && tot >= verhuur.Tot)
                        return true;
                }
            }
            return false;
        }
    }
}
