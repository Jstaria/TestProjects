using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse___After
{
    internal class BinaryTree<T>
    {
        public int Count { get; set; }
        public BinaryTreeNode<T> Root { get; set; }

        public BinaryTreeNode<T> GetRightMost(BinaryTreeNode<T> node)
        {
            if (node == null) { return null; }

            if (node.Right != null)
            {
                return GetRightMost(node.Right);
            }
            else if (node.Left != null) 
            {
                return GetRightMost(node.Left);
            }

            return node;
        }

        public void Insert(int sort, T data)
        {
            Count++;

            if (Root == null)
            {
                Root = new BinaryTreeNode<T>();
                Root.Value = data;

                return;
            }

            Insert(sort, data, Root);
        }

        private void Insert(int sort, T data, BinaryTreeNode<T> node)
        {
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // TODO: Implement this method!

            if (node.Right != null && sort > node.Sort)
            {
                Insert(sort, data, node.Right);
            }

            if (node.Left != null && sort < node.Sort)
            {
                Insert(sort, data, node.Left);
            }

            if (node.Left == null && sort < node.Sort)
            {
                node.Left = new BinaryTreeNode<T>();
                node.Left.Value = data;
                node.Left.Sort = sort;
                node.Left.Previous = node;
            }

            else if (node.Right == null && sort > node.Sort)
            {
                node.Right = new BinaryTreeNode<T>();
                node.Right.Value = data;
                node.Right.Sort = sort;
                node.Right.Previous = node;
            }

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        }
    }
}
