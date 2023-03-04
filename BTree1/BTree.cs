
using System.Windows.Forms;

namespace BTree1
{
    public class BTree
    {
        private int T;
        public BTree(int t)
        {
            T = t;
            Node.T = t;
            root = null;
        }

        public Node root;

        public void Clear()
        {

            root.Clear(ref root);
        }

        private Node Search(Node x, int key)
        {
            int i;
            if (x == null)
                return x;
            for (i = 0; i < x.n; i++)
            {
                if (key < x.key[i])
                {
                    break;
                }
                if (key == x.key[i])
                {
                    return x;
                }
            }
            if (x.leaf)
            {
                return null;
            }
            else
            {
                return Search(x.child[i], key);
            }
        }

        private void Split(Node node)
        {   
            Node child1 = new Node();
            int j;
            for (j = 0; j <= T - 2; j++)
            {
                child1.key[j] = node.key[j];
            }
            child1.n = T - 1;

            if (node.nChildren != 0)
            {
                for (j = 0; j <= T - 1; j++)
                {
                    child1.child[j] = node.child[j];
                    child1.child[j].parent = child1;
                }
                child1.leaf = false;
                child1.nChildren = child1.n +1;
            }
            else
            {
                child1.leaf = true;
                child1.nChildren = 0;
            }

            Node child2 = new Node();
            for (j = 0; j <= T - 1; j++)
            {
                child2.key[j] = node.key[j + T];
            }

            child2.n = T;

            if (node.nChildren != 0)
            {
                for (j = 0; j <= T; j++)
                {
                    child2.child[j] = node.child[j + T];
                    child2.child[j].parent = child2;
                }
                child2.leaf = false;
                child2.nChildren = child2.n + 1;
            }
            else
            {
                child2.leaf = true;
                child2.nChildren = 0;
            }

            if (node.parent == null)
            {
                node.key[0] = node.key[T - 1];
                for (j = 1; j <= (2 * T - 1); j++)
                {
                    node.key[j] = 0;
                }
                node.child[0] = child1;
                node.child[1] = child2;
                for (int i = 2; i <= (2 * T); i++)
                {
                    node.child[i] = null;
                }
                node.leaf = false;
                node.n = 1;
                node.nChildren = node.n + 1;
                child1.parent = node;
                child2.parent = node;
            }
            else
            {
                int i = 0;
                bool isNode = false;
                InsertNode(node.key[T - 1], node.parent);
                while (i <= 2 * T && !isNode)
                {
                    isNode = node.parent.child[i] == node;
                    if (isNode)
                    {
                        node.parent.child[i] = null;
                    }
                    i++;
                }

                i = 0;
                bool isNull = false;
                while (i <= 2 * T && !isNull)
                {
                    isNull = node.parent.child[i] == null;
                    if (isNull)
                    {
                        for (j = 2 * T; j > (i + 1); j--)
                        {
                            node.parent.child[j] = node.parent.child[j - 1];
                        }
                        node.parent.child[i + 1] = child2;
                        node.parent.child[i] = child1;
                    }
                    i++;
                }
                node.parent.nChildren = node.parent.n + 1;
                child1.parent = node.parent;
                child2.parent = node.parent;
                node.parent.leaf = false;
            }
        }

        private void InsertNode(int key, Node node)
        {
            node.key[node.n] = key;
            node.n++;
            Sort(node);
        }
        
        public void Insert(int key)
        {
            if (root == null)
            {
                Node newRoot = new Node();
                newRoot.key[0] = key;
                newRoot.n = 1;
                newRoot.nChildren = 0;
                newRoot.leaf = true;
                newRoot.parent = null;
                root = newRoot;
            }
            else
            {
                Node tmp = root;
                while (tmp.leaf == false)
                {
                    for (int i = 0; i <= tmp.n; i++)
                    {
                        if (key == tmp.key[i])
                        {
                            MessageBox.Show("This element already exist");
                            return;
                        }
                        if (key < tmp.key[i])
                        {
                            tmp = tmp.child[i];
                            break;
                        }
                        if ((tmp.key[i + 1] == 0) && (key > tmp.key[i]))
                        {
                            tmp = tmp.child[i + 1]; 
                            break;
                        }
                    }
                }
                int j = 0;
                bool isIdent = false;
                while (j < tmp.n && !isIdent)
                {
                    isIdent = key == tmp.key[j];
                    j++;
                }
                if (isIdent)
                {
                    MessageBox.Show("This element already exist");
                    return;
                }
                InsertNode(key, tmp);

                while (tmp.n == 2 * T)
                {
                    if (tmp == root)
                    {
                        Split(tmp);
                        break;
                    }
                    else
                    {
                        Split(tmp);
                        tmp = tmp.parent;
                    }
                }
            }
        }
        
        private void Sort(Node node)
        {
            int m;
            for (int i = 0; i < node.n; i++)
            {
                for (int j = i + 1; j < node.n; j++)
                {
                    if (node.key[i] > node.key[j])
                    {
                        m = node.key[i];
                        node.key[i] = node.key[j];
                        node.key[j] = m;
                    }
                }
            }
        }

        public void Show(TreeView treeView)
        {
            treeView.Nodes.Clear();
            string str = "";
            if (root == null)
            {
                return;
            }
            for (int j = 0; j < root.n; j++)
            {
                str += root.key[j] + " ";
            }
            treeView.Nodes.Add(str);
            Show(root, treeView.Nodes[0]);
            treeView.ExpandAll();
        }

        private void Show(Node x, TreeNode treeNode)
        {
            if (x == null)
            {
                return;
            }

            if (!x.leaf)
            {
                int i = 0;
                foreach (Node node in x.child)
                {
                    if (node == null)
                    {
                        return;
                    }
                    string str = "";
                    for (int j = 0; j < node.n; j++)
                    {
                        str += node.key[j] + " ";
                    }
                    treeNode.Nodes.Add(str);
                    Show(node, treeNode.Nodes[i]);
                    i++;
                }
            }
        }

        public bool Contain(int k)
        {
            return (this.Search(root, k) != null);
        }

        private void RemoveFromNode(int key, Node node)
        {
            int i = 0;
            bool isKey = false;
            while (!isKey && i < node.n)
            {
                isKey = node.key[i] == key;
                if (isKey)
                {
                    for (int j = i; j < node.n; j++)
                    {
                        node.key[j] = node.key[j + 1];
                        node.child[j] = node.child[j + 1];
                    }
                    node.key[node.n - 1] = 0;
                    node.child[node.n - 1] = node.child[node.n];
                    node.child[node.n] = null;
                }
                i++;
            }
            node.n--;
        }

        private void LConnect (Node node, Node node1)
        {
            if (node == null)
            {
                return;
            }

            for (int i = 0; i < node1.n; i++)
            {
                node.key[node.n] = node1.key[i];
                node.child[node.n] = node1.child[i];
                node.n++;
            }
            node.child[node.n] = node1.child[node1.n];
            int j = 0;
            while (j <= node.n && node.child[j] != null)
            {
                node.child[j].parent = node;
                j++;
            }
        }

        private void RConnect(Node node, Node node1)
        {
            if (node == null) return;
            for (int i = 0; i <= (node1.n - 1); i++)
            {
                node.key[node.n] = node1.key[i];
                node.child[node.n + 1] = node1.child[i + 1];
                node.n++;
            }
            int j = 0;
            while (j <= node.n && node.child[j] != null)
            {
                node.child[j].parent = node;
                j++;
            }
        }

        private void Repair (Node node)
        {
            if (node == root && node.n == 0)
            {
                if (root.child[0] != null)
                {
                    root.child[0].parent = null;
                    root = root.child[0];
                }
                return;
            }

            Node ptr = node;
            int positionSon = -1;
            Node parent = ptr.parent;

            for (int j = 0; j <= parent.n; j++)
            {
                if (parent.child[j] == ptr)
                {
                    positionSon = j;
                    break;
                }
            }

            if (positionSon == parent.n)
            {
                InsertNode(parent.key[positionSon - 1], parent.child[positionSon - 1]);
                LConnect(parent.child[positionSon - 1], ptr);
                parent.child[positionSon] = parent.child[positionSon - 1];
                parent.child[positionSon - 1] = null;
                RemoveFromNode(parent.key[positionSon - 1], parent);

                Node tmp = parent.child[positionSon];
                if (ptr.n == 2 * T)
                {
                    while (tmp.n == 2 * T)
                    {
                        if (tmp == root)
                        {
                            Split(tmp);
                            break;
                        }
                        else
                        {
                            Split(tmp);
                            tmp = tmp.parent;
                        }
                    }
                }
                else
                {
                    if (parent.n <= T - 2)
                        Repair(parent);
                } 
            }
            else
            {
                InsertNode(parent.key[positionSon], ptr);
                LConnect(ptr, parent.child[positionSon + 1]);
                parent.child[positionSon + 1] = ptr;
                parent.child[positionSon] = null;
                RemoveFromNode(parent.key[positionSon], parent);
                if (ptr.n == 2 * T)
                {
                    while (ptr.n == 2 * T)
                    {
                        if (ptr == root)
                        {
                            Split(ptr);
                            break;
                        }
                        else
                        {
                            Split(ptr);
                            ptr = ptr.parent;
                        }
                    }
                }
                else
                {
                    if (parent.n <= T - 2)
                        Repair(parent);
                }
            }
        }

        private void RemoveLeaf(int key, Node node)
        {
            if (node == root && node.n == 1)
            {
                RemoveFromNode(key, node);
                root.child[0] = null;
                root = null;
                return;
            }
            if (node == root || node.n > T - 1)
            {
                RemoveFromNode(key, node);
                return;
            }

            Node ptr = node;
            int k1;
            int k2;
            int positionSon = -1;
            Node parent = ptr.parent;
            int j = 0;
            bool isIdent = false;
            while (!isIdent && j <= parent.n)
            {
                isIdent = parent.child[j] == ptr;
                if (isIdent)
                {
                    positionSon = j;
                }
                j++;
            }

            if (positionSon == 0)
            {
                if (parent.child[positionSon + 1].n > (T - 1))
                { 
                    k1 = parent.child[positionSon + 1].key[0];          
                    k2 = parent.key[positionSon];     
                    InsertNode(k2, ptr);
                    RemoveFromNode(key, ptr);
                    parent.key[positionSon] = k1;                     
                    RemoveFromNode(k1, parent.child[positionSon + 1]);  
                }
                else
                {	
                    RemoveFromNode(key, ptr);
                    if (ptr.n <= T - 2) 
                        Repair(ptr);
                }
            }
            else
            {
                if (positionSon == parent.n)
                {
                    //если у левого брата больше, чем t-1 ключей
                    if (parent.child[positionSon - 1].n > (T - 1))
                    {
                        Node tmp = parent.child[positionSon - 1];
                        k1 = tmp.key[tmp.n - 1];           
                        k2 = parent.key[positionSon - 1];    
                        InsertNode(k2, ptr);
                        RemoveFromNode(key, ptr);
                        parent.key[positionSon - 1] = k1;
                        RemoveFromNode(k1, tmp);
                    }
                    else
                    {
                        RemoveFromNode(key, ptr);
                        if (ptr.n <= T - 2)
                            Repair(ptr);
                    }
                }
                else
                {
                    if (parent.child[positionSon + 1].n > (T - 1))
                    {
                        k1 = parent.child[positionSon + 1].key[0];
                        k2 = parent.key[positionSon];              
                        InsertNode(k2, ptr);
                        RemoveFromNode(key, ptr);
                        parent.key[positionSon] = k1; 
                        RemoveFromNode(k1, parent.child[positionSon + 1]);
                    }
                    else
                    {
                        if (parent.child[positionSon - 1].n > (T - 1))
                        {
                            Node temp = parent.child[positionSon - 1];
                            k1 = temp.key[temp.n - 1]; 
                            k2 = parent.key[positionSon - 1];
                            InsertNode(k2, ptr);
                            RemoveFromNode(key, ptr);
                            parent.key[positionSon - 1] = k1;
                            RemoveFromNode(k1, temp);
                        }
                        else
                        { 
                            RemoveFromNode(key, ptr);
                            if (ptr.n <= T - 2)
                                Repair(ptr);
                        }
                    }
                }
            }
        }

        private void Remove(int key, Node node)
        {
            Node ptr = node;
            int position = -1;

            for (int i = 0; i <= node.n - 1; i++)
            {
                if (key == node.key[i])
                {
                    position = i;
                    break;
                }
            }
            ptr = ptr.child[position + 1];
            int newkey = ptr.key[0];
            while (!ptr.leaf) ptr = ptr.child[0];
            if (ptr.n > (T - 1))
            {
                newkey = ptr.key[0];
                RemoveFromNode(newkey, ptr);
                node.key[position] = newkey;
            }
            else
            {
                ptr = node;
                ptr = ptr.child[position];
                newkey = ptr.key[ptr.n - 1];
                while (ptr.leaf == false) ptr = ptr.child[ptr.n];
                newkey = ptr.key[ptr.n - 1];
                node.key[position] = newkey;
                if (ptr.n > (T - 1)) RemoveFromNode(newkey, ptr);
                else
                {
                    RemoveLeaf(newkey, ptr);
                }
            }
        }

        public void Remove(int key)
        {
            if (this.root == null) return;
            Node ptr = Search(this.root, key);

            if (ptr == null) 
            {
                MessageBox.Show("This element does not exist");
                return;
            }
            if (ptr.leaf)
            {
                if (ptr.n > T - 1) 
                    RemoveFromNode(key, ptr);
                else 
                    RemoveLeaf(key, ptr);
            }
            else 
                Remove(key, ptr);
        }
    }
}
