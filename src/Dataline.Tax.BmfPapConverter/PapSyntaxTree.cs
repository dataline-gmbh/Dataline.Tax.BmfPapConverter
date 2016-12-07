using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dataline.Tax.BmfPapConverter
{
    public interface IPapSyntaxTreeNode
    {
        TResult Accept<TResult>(PapSyntaxTreeVisitor<TResult> visitor);
    }

    public abstract class PapSyntaxTreeVisitor<TResult>
    {
        public virtual TResult Visit(IPapSyntaxTreeNode node)
        {
            return node.Accept(this);
        }

        public abstract TResult VisitStatementList(PapSyntaxTreeNodeStatementList node);
        public abstract TResult VisitIf(PapSyntaxTreeNodeIf node);
        public abstract TResult VisitEval(PapSyntaxTreeNodeEval node);
        public abstract TResult VisitExecute(PapSyntaxTreeNodeExecute node);
    }

    public class PapSyntaxTreeNodeStatementList : IPapSyntaxTreeNode
    {
        public PapSyntaxTreeNodeStatementList(XElement element)
        {
            foreach (var el in element.Elements())
            {
                switch (el.Name.LocalName)
                {
                    case "IF":
                        Nodes.Add(new PapSyntaxTreeNodeIf(el));
                        break;
                    case "EVAL":
                        Nodes.Add(new PapSyntaxTreeNodeEval(el));
                        break;
                    case "EXECUTE":
                        Nodes.Add(new PapSyntaxTreeNodeExecute(el));
                        break;
                    default:
                        throw new InvalidPapException($"Im aktuellen Kontext unbekannte Anweisung {el.Name.LocalName}");
                }
            }
        }

        public PapSyntaxTreeNodeStatementList()
        {
        }

        public List<IPapSyntaxTreeNode> Nodes { get; set; } = new List<IPapSyntaxTreeNode>();

        public TResult Accept<TResult>(PapSyntaxTreeVisitor<TResult> visitor)
        {
            return visitor.VisitStatementList(this);
        }
    }

    public class PapSyntaxTreeNodeIf : IPapSyntaxTreeNode
    {
        public PapSyntaxTreeNodeIf(XElement element)
        {
            Condition = element.Attribute("expr")?.Value;
            if (Condition == null)
                throw new InvalidPapException("IF-Element enthält kein expr-Attribut");

            var then = element.Element("THEN");
            var els = element.Element("ELSE");

            if (then == null)
                throw new InvalidPapException("IF-Element enthält kein THEN-Unterelement");

            ThenStatements = new PapSyntaxTreeNodeStatementList(then);
            if (els != null)
                ElseStatements = new PapSyntaxTreeNodeStatementList(els);
        }

        public PapSyntaxTreeNodeIf()
        {
        }

        public PapSyntaxTreeNodeStatementList ThenStatements { get; set; }

        public PapSyntaxTreeNodeStatementList ElseStatements { get; set; }

        public string Condition { get; set; }

        public TResult Accept<TResult>(PapSyntaxTreeVisitor<TResult> visitor)
        {
            return visitor.VisitIf(this);
        }
    }

    public class PapSyntaxTreeNodeEval : IPapSyntaxTreeNode
    {
        public PapSyntaxTreeNodeEval(XElement element)
        {
            Expression = element.Attribute("exec")?.Value;
            if (Expression == null)
                throw new InvalidPapException("EVAL-Element enthält kein exec-Attribut");
        }

        public PapSyntaxTreeNodeEval()
        {
        }

        public string Expression { get; set; }

        public TResult Accept<TResult>(PapSyntaxTreeVisitor<TResult> visitor)
        {
            return visitor.VisitEval(this);
        }
    }

    public class PapSyntaxTreeNodeExecute : IPapSyntaxTreeNode
    {
        public PapSyntaxTreeNodeExecute(XElement element)
        {
            MethodName = element.Attribute("method")?.Value;
            if (MethodName == null)
                throw new InvalidPapException("EXECUTE-Element enthält kein method-Attribut");
        }

        public PapSyntaxTreeNodeExecute()
        {
        }

        public string MethodName { get; set; }

        public TResult Accept<TResult>(PapSyntaxTreeVisitor<TResult> visitor)
        {
            return visitor.VisitExecute(this);
        }
    }
}
