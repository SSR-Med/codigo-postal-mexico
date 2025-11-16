namespace Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class FluentIncludeExtension<TRoot>
    {
        private readonly List<List<LambdaExpression>> _includeChains = new();

        public IEnumerable<IReadOnlyList<LambdaExpression>> IncludeChains => _includeChains;

        public FluentIncludeExtension() { }

        internal FluentIncludeExtension(List<LambdaExpression> chain)
        {
            _includeChains.Add(chain);
        }

        public FluentIncludeExtension<TRoot, TProperty> Include<TProperty>(Expression<Func<TRoot, TProperty>> expression)
        {
            var chain = new List<LambdaExpression> { expression };
            _includeChains.Add(chain);
            return new FluentIncludeExtension<TRoot, TProperty>(chain, this);
        }

        public FluentIncludeExtension<TRoot, TElement> Include<TElement>(Expression<Func<TRoot, ICollection<TElement>>> expression)
        {
            var chain = new List<LambdaExpression> { expression };
            _includeChains.Add(chain);
            return new FluentIncludeExtension<TRoot, TElement>(chain, this);
        }
    }

    public class FluentIncludeExtension<TRoot, TParent>
    {
        private readonly List<LambdaExpression> _chain;
        private readonly FluentIncludeExtension<TRoot> _root;

        internal FluentIncludeExtension(List<LambdaExpression> chain, FluentIncludeExtension<TRoot> root)
        {
            _chain = chain;
            _root = root;
        }

        public FluentIncludeExtension<TRoot, TProperty> ThenInclude<TProperty>(Expression<Func<TParent, TProperty>> expression)
        {
            _chain.Add(expression);
            return new FluentIncludeExtension<TRoot, TProperty>(_chain, _root);
        }

        public FluentIncludeExtension<TRoot, TElement> ThenInclude<TElement>(Expression<Func<TParent, ICollection<TElement>>> expression)
        {
            _chain.Add(expression);
            return new FluentIncludeExtension<TRoot, TElement>(_chain, _root);
        }

        public static implicit operator FluentIncludeExtension<TRoot>(FluentIncludeExtension<TRoot, TParent> include)
        {
            return include._root;
        }
    }
}