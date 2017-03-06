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
                Position = index
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
            var sortedAffinitivies = result.Select(t => t.OrderBy(x => sortedsupport.Keys.ToList().IndexOf(x)).ToList()).ToList();


           var minsupportFirstStage = support.Where(t => t.Value > 2).ToDictionary(t=>t.Key,t=>t.Value);

           

            var FPtree = new FPTree<int>();
          foreach(var item in sortedAffinitivies)
            {
                Node<int> currNode = null;
                for(int i=0;i<item.Count;i++)
                {
                    if(i==0)
                    {
                        FPtree.Add(item[i]);
                        currNode = FPtree.Root.Children.FirstOrDefault(t => Node<int>.CompareTo(t.Symbol, item[i]));
                    }
                    else
                    {
                        FPtree.Add(item[i], currNode);
                        currNode=currNode.Children.FirstOrDefault(t => Node<int>.CompareTo(t.Symbol, item[i]));
                    }
                  
                }
            }


            Console.WriteLine("Press any key to exit.");
        }

    }
}
