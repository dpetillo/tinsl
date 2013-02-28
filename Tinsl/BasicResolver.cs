using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CodePraxis.Tinsl
{
    using Infrastructure;
    using System.Collections.ObjectModel;

    #region Data structures
    
    internal enum ResolverNodeType
    {
        Root,
        Type,
        Instance
    }

    internal interface IResolverNode
    {
        ResolverNodeType NodeType
        {
            get;
        }
    }

    internal struct RootNode : IResolverNode
    {
        public ResolverNodeType NodeType
        {
            get { return ResolverNodeType.Root; }
        }
    }

    internal struct TypeNode : IResolverNode
    {
        readonly Type _type;

        public TypeNode(Type type)
        {
            _type = type;
        }

        public Type Type
        {
            get { return _type; }
        }

        public ResolverNodeType NodeType
        {
            get { return ResolverNodeType.Type; }
        }
    }

    internal struct InstanceNode : IResolverNode
    {
        public static readonly object SINGLETON = new object();

        readonly object _identifier;
        readonly object _instance;

        public InstanceNode(object identifier, object instance)
        {
            _identifier = identifier;
            _instance = instance;
        }

        public object Identifier
        {
            get { return _identifier; }
        }

        public object Instance
        {
            get { return _instance; }
        }


        public ResolverNodeType NodeType
        {
            get { return ResolverNodeType.Instance; }
        }
    }

    #endregion

    public class BasicResolver<T> : IBindingResolver<T>
    {
        private INAryTree<IResolverNode> _store = new NAryTree<IResolverNode>(new RootNode(), null);

        public R Resolve<R>(object identifier = null) where R : T
        {
            Type type = typeof(R);
            identifier = identifier ?? InstanceNode.SINGLETON;

            List<R> validInstances = new List<R>();

            foreach (INAryTree<IResolverNode> treeNode in _store.Children)
            {
                Debug.Assert(treeNode.Value.NodeType == ResolverNodeType.Type);

                TypeNode typeNode = (TypeNode)treeNode.Value;

                if (type.IsAssignableFrom(typeNode.Type))
                {
                    foreach (INAryTree<IResolverNode> childTreeNode in treeNode.Children)
                    {
                        Debug.Assert(childTreeNode.Value.NodeType == ResolverNodeType.Instance);

                        InstanceNode instanceNode = (InstanceNode)childTreeNode.Value;

                        if (instanceNode.Identifier.Equals(identifier))
                        {
                            validInstances.Add((R)instanceNode.Instance);
                        }
                    }
                }
            }

            Debug.Assert(validInstances.Count <= 1);

            /*if (validInstances.Count > 1)
            {
                throw new ResolverException("More than one matching instance found.", type);
            }*/
            if (validInstances.Count == 0)
            {
                //throw new ResolverException("No matching instances found.", type);
                return default(R);
            }

            return validInstances[0];
        }

        public void Bind(T instance, object identifier = null)
        {
            identifier = identifier ?? InstanceNode.SINGLETON;

            var typeQuery = from tn in _store.Children
                            where instance.GetType().IsAssignableFrom(((TypeNode)tn.Value).Type)
                            select tn;
            INAryTree<IResolverNode> typeNode = typeQuery.FirstOrDefault();
            INAryTree<IResolverNode> instanceNode = null;
            if (typeNode != null)
            {
                var instanceQuery = from @in in typeNode.Children
                                    where identifier.Equals(((InstanceNode)@in.Value).Identifier)
                                    select @in;

                instanceNode = instanceQuery.FirstOrDefault();
            }

            if (typeNode != null && instanceNode != null)
            {
                //replace instance child
                _store = _store.ReplaceChild(
                    typeNode, typeNode.ReplaceChild(instanceNode,
                        new NAryTree<IResolverNode>(new InstanceNode(identifier, instance))));
            }
            /*else if (typeNode != null && instanceNode == null)
            {
                //add instanceNode
                _store = _store.ReplaceChild(
                    typeNode, typeNode.AddChild(
                    new NAryTree<IResolverNode>(new InstanceNode(identifier, instance))));
            }*/
            else
            {
                Debug.Assert(typeNode == null);
                Debug.Assert(instanceNode == null);

                //we have no typeNode or instanceNode
                _store = _store.AddChild(
                    new NAryTree<IResolverNode>(new TypeNode(instance.GetType()))
                        .AddChild(new NAryTree<IResolverNode>(new InstanceNode(identifier, instance))));
            }
        }
    }
}
