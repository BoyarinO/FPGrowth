using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPGrowthLibrary
{
    public class Node<T>
    {
       public int Weight { get; set; }
        public T Symbol {get;set;}      
        public List<Node<T>> Children { get; set; }
        public Node<T> Parent { get; set; }
        public Node<T> Sibling { get; set; }
        public Node()
        {
            Weight = 0;
            Children = new List<Node<T>>();         
        }

        public Node(T symbol):this()
        {
            Symbol = symbol;
            Weight = 1;
        }
      

        public static bool CompareTo(T value1, T value2)
        {
            return EqualityComparer<T>.Default.Equals(value1, value2);
        }
    }
}
