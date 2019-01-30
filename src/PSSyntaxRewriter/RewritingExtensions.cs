using System;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public static class RewritingExtensions
    {
        public static TAst Rewrite<TAst>(
            this TAst ast,
            RewritingVisitor visitor,
            SyntaxKind expectingCategory = SyntaxKind.Any)
            where TAst : Ast
        {
            var oldCategory = visitor.CurrentExpectedKind;
            try
            {
                visitor.CurrentExpectedKind = expectingCategory;
                return (TAst)ast.Visit(visitor);
            }
            finally
            {
                visitor.CurrentExpectedKind = oldCategory;
            }
        }

        public static IEnumerable<Tuple<TAst, TAst2>> RewriteAllTuples<TAst, TAst2>(
            this IEnumerable<Tuple<TAst, TAst2>> source,
            RewritingVisitor visitor,
            SyntaxKind expectedKind0 = SyntaxKind.Any,
            SyntaxKind expectedKind1 = SyntaxKind.Any)
            where TAst : Ast
            where TAst2 : Ast
        {
            foreach (Tuple<TAst, TAst2> tuple in source)
            {
                yield return tuple.Rewrite(visitor, expectedKind0, expectedKind1);
            }
        }

        public static Tuple<TAst, TAst2> Rewrite<TAst, TAst2>(
            this Tuple<TAst, TAst2> source,
            RewritingVisitor visitor,
            SyntaxKind expectedKind0 = SyntaxKind.Any,
            SyntaxKind expectedKind1 = SyntaxKind.Any)
            where TAst : Ast
            where TAst2 : Ast
        {
            return Tuple.Create(
                source.Item1.Rewrite(visitor, expectedKind0),
                source.Item2.Rewrite(visitor, expectedKind1));
        }

        public static IList<TAst> RewriteAll<TAst>(
            this IList<TAst> asts,
            RewritingVisitor visitor,
            SyntaxKind expectingCategory = SyntaxKind.Any)
            where TAst : Ast
        {
            if (asts == null)
            {
                return Array.Empty<TAst>();
            }

            if (asts is TAst[] asArray)
            {
                return asArray.RewriteAll(visitor, expectingCategory);
            }

            var results = new List<TAst>(asts.Count);
            foreach (TAst ast in asts)
            {
                results.Add(ast.Rewrite(visitor, expectingCategory));
            }

            return results;
        }

        public static TAst[] RewriteAll<TAst>(
            this TAst[] asts,
            RewritingVisitor visitor,
            SyntaxKind expectingCategory = SyntaxKind.Any)
            where TAst : Ast
        {
            if (asts == null || asts.Length == 0)
            {
                return Array.Empty<TAst>();
            }

            var results = new TAst[asts.Length];
            for (int i = 0; i < asts.Length; i++)
            {
                results[i] = asts[i].Rewrite(visitor, expectingCategory);
            }

            return results;
        }

        public static IEnumerable<TAst> RewriteAll<TAst>(
            this IEnumerable<TAst> asts,
            RewritingVisitor visitor,
            SyntaxKind expectingCategory = SyntaxKind.Any)
            where TAst : Ast
        {
            if (asts is IList<TAst> asList)
            {
                return asList.RewriteAll(visitor, expectingCategory);
            }

            return Enumerate();
            IEnumerable<TAst> Enumerate()
            {
                foreach (TAst ast in asts)
                {
                    yield return ast.Rewrite(visitor, expectingCategory);
                }
            }
        }
    }
}
