using FPGrowthLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPGrowthLibrary
{
    public class FPTree<T>
    {
        public Node<T> Root { get; set; }
        public int Count { get; set; }
        private List<Node<T>> LastInserted = new List<Node<T>>();
        private List<Node<T>> FirstInserted = new List<Node<T>>();
        private FPTree()
        {
            var rootNode = new Node<T>();
            rootNode.Parent = null;
            Root = rootNode;
        }
        public FPTree(IEnumerable<IList<T>> DataList) : this()
        {
            foreach (var item in DataList)
            {
                var currElement = Root;
                for (int i = 0; i < item.Count(); i++)
                {

                    if (currElement.Children != null && currElement.Children.Any() && currElement.Children.Any(t => Node<T>.CompareTo(t.Symbol, item[i])))
                    {
                        currElement = currElement.Children.First(t => Node<T>.CompareTo(t.Symbol, item[i]));
                        currElement.Weight += 1;
                    }
                    else
                    {
                        var node = new Node<T>();
                        //does link changes?
                        node.Parent = currElement;
                        currElement.Children.Add(node);
                        node.Symbol = item[i];
                        currElement = node;

                        if (LastInserted.Any(t => Node<T>.CompareTo(t.Symbol, item[i])))
                        {
                            var lastInserted = LastInserted.First(t => Node<T>.CompareTo(t.Symbol, item[i]));
                            lastInserted.RightSibling = currElement;
                            currElement.LeftSibling = lastInserted;
                            LastInserted.Remove(lastInserted);
                        }
                        LastInserted.Add(currElement);

                        if (!FirstInserted.Any(t => Node<T>.CompareTo(t.Symbol, item[i])))
                        {
                            FirstInserted.Add(currElement);
                        }
                    }

                }
            }
        }
        public List<IEnumerable<T>> GetFrequentItemsForItem(T item)
        {
            var subTree = this.GetSubTree(item);
            var frequentItems = subTree.ExtractFrequentItems(item);
            return frequentItems;
        }
        private FPTree<T> GetSubTree(T item)
        {
            var currElement = FirstInserted.FirstOrDefault(t => Node<T>.CompareTo(t.Symbol, item));
            var subTree = new FPTree<T>();

            if (currElement != null)
            {
                while (currElement.RightSibling != null)
                {
                    var brunch = new Stack<Node<T>>();
                    currElement.Children = null;

                    while (currElement.Parent != null)
                    {
                        if (currElement.Parent.Children.Count > 1)
                        {
                            currElement.Parent.Children = currElement.Parent.Children.Where(t => Node<T>.CompareTo(t.Symbol, currElement.Symbol)).ToList();
                        }
                        brunch.Push(currElement);

                        currElement = currElement.Parent;
                    }                    
                    subTree.InsertBrunch(brunch);
                    currElement = brunch.Last().RightSibling;
                }              
            }
            return subTree;
        }
        private void InsertBrunch(IEnumerable<Node<T>> brunch)
        {
            var currElement = this.Root;

            for (int i = 0; i < brunch.Count(); i++)
            {
                var symbol = brunch.ElementAt(i).Symbol;

                if (currElement.Children != null && currElement.Children.Any() && currElement.Children.Any(t => Node<T>.CompareTo(t.Symbol, symbol)))
                {
                    currElement = currElement.Children.First(t => Node<T>.CompareTo(t.Symbol, symbol));
                    currElement.Weight += 1;
                }
                else
                {
                    //Or use brunch[i] instead creting new node                  
                    var node = new Node<T>(symbol);
                    //does link changes?
                    node.Parent = currElement;
                    currElement.Children.Add(node);
                    currElement = node;

                    if (LastInserted.Any(t => Node<T>.CompareTo(t.Symbol, symbol)))
                    {
                        var lastInserted = LastInserted.First(t => Node<T>.CompareTo(t.Symbol, symbol));
                        lastInserted.RightSibling = currElement;
                        currElement.LeftSibling = lastInserted;
                        LastInserted.Remove(lastInserted);
                    }
                    LastInserted.Add(currElement);

                    if (!FirstInserted.Any(t => Node<T>.CompareTo(t.Symbol, symbol)))
                    {
                        FirstInserted.Add(currElement);
                    }
                }
            }
        }
        private List<IEnumerable<T>> ExtractFrequentItems(T item, int minSupp = 2)
        {
            var currElement = this.GetFirstNode(item);
            var brunches = new List<IEnumerable<T>>();
            while (currElement.RightSibling != null)
            {
                var brunch = new Stack<T>();
                var el = currElement.Parent;
                while (el.Parent != null)
                {
                    brunch.Push(el.Symbol);
                    el = el.Parent;
                }
                brunches.Add(brunch);
                currElement = currElement.RightSibling;
            }

            var items = brunches.SelectMany(t => t.Select(a => a)).Distinct();
            var support = new Dictionary<T, int>();
            foreach (var it in brunches)
            {
                foreach (var ing in it)
                {
                    if (support.Any(t => Node<T>.CompareTo(t.Key, ing)))
                    {
                        support[ing] += 1;
                    }
                    else
                    {
                        support.Add(ing, 1);
                    }
                }
            }

            var frequent = brunches.Where(t => brunches.Any(a => a != t && a.ContainsAllItems(t)) && t.Count() > 1).ToList();
            frequent.AddRange(support.Where(t => t.Value >= minSupp).Select(t => new List<T>() { t.Key }));
            return frequent;
        }      
        private Node<T> GetFirstNode(T value)
        {
            return FirstInserted.First(t => Node<T>.CompareTo(t.Symbol, value));
        }
        private Node<T> GetLastNode(T value)
        {
            return LastInserted.First(t => Node<T>.CompareTo(t.Symbol, value));
        }      
    }
    public static class LinqExt
    {
        public static bool ContainsAllItems<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return !b.Except(a).Any();
        }
    }
}
