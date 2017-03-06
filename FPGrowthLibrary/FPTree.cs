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
       
        
        public FPTree()
        {
            Root = new Node<T>();          
        }
    
        public void Add(T value, Node<T> node = null)
        {
            if(node==null)
            {
                node = Root;
            }
            if (node.Children.Count > 0 && node.Children.Any(t => Node<T>.CompareTo(t.Symbol, value)))
            {
                var CurrentNode = node.Children.FirstOrDefault(t => Node<T>.CompareTo(t.Symbol, value));
                CurrentNode.Weight += 1;
            }
            else
            {
                var childNode = new Node<T>(value);
                childNode.Parent = node;               
                node.Children.Add(childNode);
            }
        }
    }
}
