using BaseProjectApp.Library.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Utility
{
    public class Enumerations
    {

        public static List<ListItemNN> GetNumbersList(List<ListItemNN> NumbersList, int Count = 5 , bool FloorsCounter = false , string Lang = "en")
        {
            if (FloorsCounter == true)
            { 
                NumbersList.Add(new ListItemNN { Id = null, Value = "Any" }); 
                NumbersList.Add(new ListItemNN { Id = 0, Value = (Lang == "en" ? "Ground Floor" : "الطابق الأرضي")});
            }
            else
            {
                NumbersList.Add(new ListItemNN { Id = 0, Value = "Any" }); 
            }

            for (int i = 1; i <= Count; i++)
            {
                if (Count == i)
                {
                    NumbersList.Add(new ListItemNN { Id = i, Value = $"{i.ToString()}+" });
                }
                else
                {
                    NumbersList.Add(new ListItemNN { Id = i, Value = i.ToString() });
                }
            }

            return NumbersList; 
        }

        public static Dictionary<int, string> GetDataAsDictionary<T>() where T : System.Enum
        {
            var result = new Dictionary<int, string>();
            var values = Enum.GetValues(typeof(T));

            foreach (int item in values)
                result.Add(item, Enum.GetName(typeof(T), item));
            return result;
        }

        public static List<ListItem> GetDataAsItems<T>() where T : System.Enum
        {
            var result = new List<ListItem>();
            var values = Enum.GetValues(typeof(T));

            foreach (int item in values)
                result.Add(new ListItem { Id = item, Value = (Enum.GetName(typeof(T), item)).Replace("_", " ") });
            return result;
        }
          

    }
}
