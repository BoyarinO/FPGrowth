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
        private  List<Node<T>> LastInserted = new List<Node<T>>();
        private  List<Node<T>> FirstInserted = new List<Node<T>>();


        private FPTree()
        {
            var rootNode = new Node<T>();
            rootNode.Parent = null;
            Root= rootNode;                                              
        }
        
        public FPTree(IEnumerable<IList<T>> DataList):this()
        {           
            foreach (var item in DataList)
            {
                var currElement = Root;
                for (int i = 0; i < item.Count(); i++)
                {                   
                   
                    if (currElement.Children!=null && currElement.Children.Any() && currElement.Children.Any(t => Node<T>.CompareTo(t.Symbol, item[i])))
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

        public FPTree<T> GetSubTree(T item)
        {
            var currElement = FirstInserted.FirstOrDefault(t => Node<T>.CompareTo(t.Symbol, item));
            var subTree = new FPTree<T>();

            if (currElement != null)
            {
                while (currElement.RightSibling != null)
                {
                  
                    var brunch = new List<Node<T>>();
                    currElement.Children = null;
                    while (currElement.Parent != null)
                    {
                        if (currElement.Parent.Children.Count > 1)
                        {
                            //     currElement.Weight = 1;
                            currElement.Parent.Children = currElement.Parent.Children.Where(t => Node<T>.CompareTo(t.Symbol, currElement.Symbol)).ToList();
                        }
                        brunch.Add(currElement);
                        currElement = currElement.Parent;
                    }
                    brunch.Reverse();                    
                    subTree.InsertBrunch(brunch);
                    var lastbr = brunch.Last();
                    currElement = brunch.Last().RightSibling;
                }
                
            }
            return subTree;
        }        

        public void InsertBrunch(List<Node<T>> brunch)
        {
            var currElement = this.Root;
            for(int i=0;i<brunch.Count;i++)
            {
                if (currElement.Children != null && currElement.Children.Any() && currElement.Children.Any(t => Node<T>.CompareTo(t.Symbol, brunch[i].Symbol)))
                {
                    currElement = currElement.Children.First(t => Node<T>.CompareTo(t.Symbol, brunch[i].Symbol));
                    currElement.Weight += 1;
                }
                else
                {
                    //Or use brunch[i] instead creting new node
                    var node = new Node<T>();
                    //does link changes?
                    node.Parent = currElement;
                    currElement.Children.Add(node);
                    node.Symbol = brunch[i].Symbol;
                    currElement = node;

                    if (LastInserted.Any(t => Node<T>.CompareTo(t.Symbol, brunch[i].Symbol)))
                    {
                        var lastInserted = LastInserted.First(t => Node<T>.CompareTo(t.Symbol, brunch[i].Symbol));
                        lastInserted.RightSibling = currElement;
                        currElement.LeftSibling = lastInserted;
                        LastInserted.Remove(lastInserted);
                    }
                    LastInserted.Add(currElement);

                    if (!FirstInserted.Any(t => Node<T>.CompareTo(t.Symbol, brunch[i].Symbol)))
                    {
                        FirstInserted.Add(currElement);
                    }
                }
            }
        }


      


       
        public Node<T> GetFirstNode(T value)
        {
            return FirstInserted.First(t=> Node<T>.CompareTo(t.Symbol,value));
        }
        public Node<T> GetLastNode(T value)
        {
            return LastInserted.First(t => Node<T>.CompareTo(t.Symbol, value));
        }

        //public List<Node<T>> GetSiblings(T value)
        //{
            
            
        //}

        //public int TestDep(FPTree<T> tree,Node<T> node,int deep=0)
        //{
        //    var currnode = node;           
        //    while(currnode.Children.Count()>0)
        //    {
        //        deep += currnode.Children.Count();
        //        foreach(var item in currnode.Children)
        //        {
        //            TestDep(tree, item,deep);
        //        }
        //    }
        //    return deep;
        //}

    }
}
