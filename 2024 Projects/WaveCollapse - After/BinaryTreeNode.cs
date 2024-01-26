using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse___After
{
    internal class BinaryTreeNode<T>
    {
        public BinaryTreeNode<T> Previous { get; set; }
        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }
        public T Value { get; set; }
        public int Sort { get; set; }
    }
}
