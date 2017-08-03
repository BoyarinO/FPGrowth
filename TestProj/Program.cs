using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FPGrowthLibrary;
namespace TestProj
{
    class Program
    {
        static void Main(string[] args)
        {
        
            
            string text = File.ReadAllText("C:\\pdfrecipes.txt");
            string[] stringSeparators = new string[] { "\\r\\n" };
            string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

            var linesep = new string[] { "+" };
            //List of recipes
            List<string[]> afiinitives = new List<string[]>();
            for(int i=0; i<lines.Count(); i++)
            {
                if (lines[i].Contains("Affinities"))
                {
                    while(lines[i+1].Contains("+"))
                    {
                        var oneline = lines[i + 1].Split(linesep, StringSplitOptions.None);
                        
                       for(int z=0;z<oneline.Count(); z++)
                        {
                            oneline[z]= oneline[z].Replace("\\t", " ").Trim(); 
                            //truffle, black           
                            if(oneline[z].Contains(","))
                            {
                                oneline[z] = oneline[z].Substring(0, oneline[z].IndexOf(','));
                            }               
                        }
                       
                        afiinitives.Add(oneline);
                        i++;
                    }
                }
            }
            //Count of recipes
            var count = afiinitives.Sum(t =>t.Count());
            //List with names of ingridients 
            var ingrideints = afiinitives.SelectMany(t => t).ToArray().Distinct();
            //Unic count of ingridients 
            var uniccount = ingrideints.Count();
            //Set key to every ingridient
            var groupOfIngridients_Index = ingrideints.Select((item, index) => new
            {
                ItemName = item,
                Position = index+1
            }).ToDictionary(t=>t.Position, t=>t.ItemName);
            //List of recipes(key instead of string) 
            var result  = afiinitives.Select((item, index) =>
            item.Select((ingred,ind)=>                 
                  groupOfIngridients_Index.FirstOrDefault(t=>t.Value==ingred).Key).ToList()).ToList();

            //Support of every ingridient 
            var support = new Dictionary<int, int>();
            foreach (var item in result)
            {
                foreach(var ing in item)
                {
                    if (support.Any(t => t.Key == ing))
                    {
                        support[ing] += 1;
                    }
                    else
                    {
                        support.Add(ing, 1);
                    }
                }
            }
            //Sort by support
           var sortedsupport = support.OrderByDescending(t => t.Value).ToDictionary(t=>t.Key,t=>t.Value);

            //Sort recipes by support
            //Ready to FP_growth
            var sortedAffinitivies = result.Select(t => t.OrderBy(xr => sortedsupport.Keys.ToList().IndexOf(xr)).ToList()).ToList();


           var minsupportFirstStage = support.Where(t => t.Value > 2).ToDictionary(t=>t.Key,t=>t.Value);
         for(int i=0;i<sortedAffinitivies.Count();i++)
            {
                for(int j=0;j<sortedAffinitivies[i].Count();j++)
                {
                    if (sortedAffinitivies[i][j] == 2)
                    {
                        var x = 1;
                    }
                }
            }


            var Fptes = new FPTree<int>(sortedAffinitivies);
            var currn = Fptes.Root;
            var symbolTocheck = 2;
            var firstCount = sortedAffinitivies.Count(t=>t.Contains(2));
            var currnode = new Node<int>();
            int sec = 0;
            var firstnode = currn.Children.First(t => t.Symbol == 2);
         while(firstnode.Sibling!=null)
            {
                sec++;
                firstnode = firstnode.Sibling;
            }

         


            Console.WriteLine("Press any key to exit.");
        }

    }
}
