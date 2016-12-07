using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public abstract class PropertyBuilder : ICodeGen
    {
        protected PropertyBuilder(Visibilities visibility, Type type, string name, bool setInConstructor = false)
        {
            Visibility = visibility;
            Type = type;
            Name = name;
            SetInConstructor = setInConstructor;
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public Visibilities Visibility { get; set; }

        public bool SetInConstructor { get; set; }

        public string Summary { get; set; }

        public virtual void CodeGen(CodeBuilder builder)
        {
            if (Summary != null)
                builder.AppendSummary(Summary);
        }

        public string GenerateParameterName()
        {
            if (string.IsNullOrEmpty(Name))
                return "";

            var sb = new StringBuilder();
            sb.Append(char.ToLower(Name[0]));
            sb.Append(Name.Substring(1));

            return sb.ToString();
        }
    }

    public class AutoPropertyBuilder : PropertyBuilder
    {
        public AutoPropertyBuilder(Visibilities visibility, Type type, string name, ExpressionBuilder defaultExpression = null)
            : base(visibility, type, name)
        {
            DefaultExpression = defaultExpression;
        }

        public bool Getter { get; set; } = true;

        public bool Setter { get; set; } = true;

        public ExpressionBuilder DefaultExpression { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            base.CodeGen(builder);

            if (!Getter && !Setter)
                throw new InvalidOperationException("Property muss einen Getter oder Setter haben");

            Visibility.CodeGen(builder);
            Type.CodeGen(builder);

            builder.AppendToken(Name);

            builder.AppendToken("{");
            if (Getter)
                builder.AppendToken("get;");
            if (Setter)
                builder.AppendToken("set;");
            builder.AppendToken("}");

            if (DefaultExpression != null)
            {
                builder.AppendToken("=");
                DefaultExpression.CodeGen(builder);

                builder.EndOfStatement();
                builder.EndOfLineBlock();
            }
            else
            {
                builder.EndOfLineBlock();
            }
        }
    }

    public class ExpressionPropertyBuilder : PropertyBuilder
    {
        public ExpressionPropertyBuilder(Visibilities visibility, Type type, string name, ExpressionBuilder expression)
            : base(visibility, type, name)
        {
            Expression = expression;
        }

        public ExpressionBuilder Expression { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            base.CodeGen(builder);

            Visibility.CodeGen(builder);
            Type.CodeGen(builder);

            builder.AppendToken(Name);

            builder.AppendToken("=>");

            Expression.CodeGen(builder);

            builder.EndOfStatement();
            builder.EndOfLineBlock();
        }
    }

    public class FullPropertyBuilder : PropertyBuilder
    {
        public FullPropertyBuilder(Visibilities visibility,
                                   Type type,
                                   string name,
                                   StatementBuilder getterStatement,
                                   StatementBuilder setterStatement)
            : base(visibility, type, name)
        {
            GetterStatement = getterStatement;
            SetterStatement = setterStatement;
        }

        public StatementBuilder GetterStatement { get; set; }

        public StatementBuilder SetterStatement { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            base.CodeGen(builder);

            Visibility.CodeGen(builder);
            Type.CodeGen(builder);

            builder.AppendToken(Name);

            builder.BeginBlock();

            // Getter
            builder.AppendToken("get");
            builder.BeginBlock();
            GetterStatement.CodeGen(builder);
            builder.EndBlock();

            builder.EndOfLine(); // EndBlock-Verhalten überschreiben, sodass keine Leerzeile entsteht

            // Setter
            builder.AppendToken("set");
            builder.BeginBlock();
            SetterStatement.CodeGen(builder);
            builder.EndBlock();

            builder.EndBlock();
        }
    }
}
