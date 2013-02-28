using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl.Infrastructure
{
    public interface INAryTree<V>
    {
        V Value { get; }
        IReadOnlyCollection<INAryTree<V>> Children { get; }

        INAryTree<V> AddChild(INAryTree<V> child);
        INAryTree<V> ReplaceChild(INAryTree<V> toReplace, INAryTree<V> replacement);
    }

    public sealed class NAryTree<V> : INAryTree<V>
    {
        private readonly V value;
        private readonly IReadOnlyCollection<INAryTree<V>> children;

        public bool IsEmpty { get { return false; } }
        public V Value { get { return value; } }

        public IReadOnlyCollection<INAryTree<V>> Children { get { return children; } }

        public NAryTree(V value, IReadOnlyCollection<INAryTree<V>> children = null)
        {
            this.value = value;
            this.children = children ?? new ReadOnlyCollection<INAryTree<V>>(new List<INAryTree<V>>());
        }

        public INAryTree<V> AddChild(INAryTree<V> child)
        {
            List<INAryTree<V>> newChildren = new List<INAryTree<V>>(this.children);
            newChildren.Add(child);
            return new NAryTree<V>(value, new ReadOnlyCollection<INAryTree<V>>(newChildren));
        }
        public INAryTree<V> ReplaceChild(INAryTree<V> toReplace, INAryTree<V> replacement)
        {
            List<INAryTree<V>> newChildren = new List<INAryTree<V>>(this.children);
            newChildren[newChildren.FindIndex( f => f.Equals(toReplace))] = replacement;
            return new NAryTree<V>(value, new ReadOnlyCollection<INAryTree<V>>(newChildren));
        }
    } 
}
