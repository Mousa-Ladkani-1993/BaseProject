using BaseProjectApp.Library.Templates.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Utility
{
    public static class TreeBuilder
    {
        ////////////////////////////////////////////////
        /// 
        public static IList<MobileCustomMenuNode> BuildMenusTree(IEnumerable<MobileCustomMenuNode> source)
        {
            if (source == null || source.Count() == 0)
                return new List<MobileCustomMenuNode>();

            try
            {
                var ids = (source.Select(o => o.Id)?.ToList())?.Cast<int?>().ToList();
                var ItemsWithInvalidParents = source.Where(o => !ids.Contains(o.ParentId))?.ToList();

                foreach (var Item in ItemsWithInvalidParents)
                {
                    Item.ParentId = null;
                }


                var Pillars = source.GroupBy(i => i.ParentId);
                var roots = Pillars.FirstOrDefault(g => g.Key.HasValue == false).ToList();

                if (roots.Count > 0)
                {
                    var dict = Pillars.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                    for (int i = 0; i < roots.Count; i++)
                        AddMenus(roots[i], dict);
                }

                return roots;
            }

            catch (System.Exception ex) { }

            return new List<MobileCustomMenuNode>();
        }
        private static void AddMenus(MobileCustomMenuNode node, IDictionary<int, List<MobileCustomMenuNode>> source)
        {
            if (source.ContainsKey(node.Id))
            {
                node.Children = source[node.Id];
                for (int i = 0; i < node.Children.Count; i++)
                    AddMenus(node.Children[i], source);
            }
            else
            {
                node.Children = new List<MobileCustomMenuNode>();
            }
        }
        ////////////////////////////////////////////////
        ///


        ////////////////////////////////////////////////
        /// 
        public static IList<LocationNodeDTO> BuildLocationsTree(IEnumerable<LocationNodeDTO> source)
        {
            if (source == null || source.Count() == 0)
                return new List<LocationNodeDTO>();

            try
            {
                var ids = (source.Select(o => o.Id)?.ToList())?.Cast<int?>().ToList();
                var ItemsWithInvalidParents = source.Where(o => !ids.Contains(o.ParentId))?.ToList();

                foreach (var Item in ItemsWithInvalidParents)
                {
                    Item.ParentId = null;
                }


                var Pillars = source.GroupBy(i => i.ParentId);
                var roots = Pillars.FirstOrDefault(g => g.Key.HasValue == false).ToList();

                if (roots.Count > 0)
                {
                    var dict = Pillars.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                    for (int i = 0; i < roots.Count; i++)
                        AddLocations(roots[i], dict);
                }

                return roots;
            }

            catch (System.Exception ex) { }

            return new List<LocationNodeDTO>();
        }
        private static void AddLocations(LocationNodeDTO node, IDictionary<int, List<LocationNodeDTO>> source)
        {
            if (source.ContainsKey(node.Id))
            {
                node.Children = source[node.Id];
                for (int i = 0; i < node.Children.Count; i++)
                    AddLocations(node.Children[i], source);
            }
            else
            {
                node.Children = new List<LocationNodeDTO>();
            }
        } 
        ////////////////////////////////////////////////
        ///

    }
}
