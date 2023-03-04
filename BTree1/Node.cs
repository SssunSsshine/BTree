using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTree1
{
    public class Node
    {
        public int n;
        static public int T;
        public int[] key = new int[2 * T];
        public Node[] child = new Node[2 * T + 1];
        public Node parent;
        public int nChildren;
        public bool leaf = true;

        public void Clear( ref Node node)
        {
            if (node == null)
            {
                return;
            }
            for (int i = 0; i < node.nChildren; i++)
            {
                Clear(ref node.child[i]);
            }
            node = null;
        }
    }
}
