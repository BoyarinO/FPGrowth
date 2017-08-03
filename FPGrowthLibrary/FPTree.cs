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
        private static List<Node<T>> LastInserted = new List<Node<T>>();
        private static List<Node<T>> FirstInserted = new List<Node<T>>();


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
                    var x = Convert.ToInt32(item[i]);
                    var exsist = false;
                    if (currElement.Children!=null && currElement.Children.Any() && currElement.Children.Any(t => Node<T>.CompareTo(t.Symbol, item[i])))
                    {
                        currElement = currElement.Children.First(t => Node<T>.CompareTo(t.Symbol, item[i]));
                        currElement.Weight += 1;
                        exsist = true;                    
                     
                    }
                    else
                    {
                        var node = new Node<T>();
                        //does link changes?
                        node.Parent = currElement;
                        currElement.Children.Add(node);
                        node.Symbol = item[i];
                        currElement = node;                        
                    }                
                    if(!exsist)
                    {
                        if (LastInserted.Any(t => Node<T>.CompareTo(t.Symbol, item[i])))
                        {
                            var lastInserted = LastInserted.First(t => Node<T>.CompareTo(t.Symbol, item[i]));                           
                            lastInserted.RightSibling = currElement;
                            currElement.LeftSibling = lastInserted;
                            LastInserted.Remove(lastInserted);
                        }
                        LastInserted.Add(currElement);

                        if(!FirstInserted.Any(t => Node<T>.CompareTo(t.Symbol, item[i])))
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
            if(currElement!=null)
            {
                while(currElement.RightSibling!=null)
                {
                    while (currElement.Parent.Symbol != null)
                    {
                        //Wrong -> next element will not have Parent.Children of currElement
                        if (currElement.Parent.Children.Count > 1)
                        {
                            currElement.Parent.Children = new List<Node<T>>();
                            currElement.Parent.Children.Add(currElement);
                            currElement.Weight = 1;
                        }
                        currElement = currElement.Parent;
                    }

                    while(currElement.Children.Count>0)
                    {
                        var parent = currElement.Parent.Symbol != null ? currElement.Parent : subTree.Root;
                        if(parent==subTree.Root)
                        {
                            parent.Children.Add(currElement);
                        }                     
                        currElement.Parent = parent;
                     






                        if(currElement.Children.Count>1)
                        {
                            //ERROR
                            var xdaad = 1;
                        }
                        currElement = currElement.Children.First();
                    }


                    currElement = currElement.RightSibling;                    
                }

            }
            return null;
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
