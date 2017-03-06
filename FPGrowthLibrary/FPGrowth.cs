using FPGrowthLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPGrowthLibrary
{
   public class FPGrowth<T>
    {
        FPTree<T> Tree { get; set; }


        public void GetSubTrees(T value)
        {
            var root = Tree.Root;
            var newTree = new FPTree<T>();
            while (!Node<T>.CompareTo(root.Symbol, value))
            {
              //  newTree.Add()
            }

            root.Children.FirstOrDefault(t => Node<T>.CompareTo(t.Symbol, value));
        }

        public void GetSubTrees(Node<T> node, T value)
        {
            while(node.Children.Count>0)
            {
                if(Node<T>.CompareTo(node.Symbol, value))
                {

                }                
                else
                {
                    foreach(var item in node.Children)
                    {
                        GetSubTrees(item, value);
                    }
                }
            }
        }

       
    }
}
