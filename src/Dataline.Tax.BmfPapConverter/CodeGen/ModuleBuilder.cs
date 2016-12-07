using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class ModuleBuilder : ICodeGen
    {
        public enum ModuleTypes
        {
            Class, Struct
        }

        public ModuleBuilder(ModuleTypes type, Visibilities visibility, string name)
        {
            ModuleType = type;
            Visibility = visibility;
            Name = name;
        }

        public string Name { get; set; }

        public ModuleTypes ModuleType { get; set; }

        public Visibilities Visibility { get; set; }

        public List<ConstantBuilder> Constants { get; set; } = new List<ConstantBuilder>();

        public List<PropertyBuilder> Properties { get; set; } = new List<PropertyBuilder>();

        public List<MethodBuilder> Methods { get; set; } = new List<MethodBuilder>();

        public List<Type> Inheritations { get; set; } = new List<Type>();

        public List<string> StaticCode { get; set; } = new List<string>();


        public void CodeGen(CodeBuilder builder)
        {
            Visibility.CodeGen(builder);

            switch (ModuleType)
            {
                case ModuleTypes.Class:
                    builder.AppendToken("class");
                    break;
                case ModuleTypes.Struct:
                    builder.AppendToken("struct");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.AppendToken(Name);

            if (Inheritations.Any())
            {
                builder.AppendToken(":");
                builder.BeginSeparatedList(",");

                foreach (var inheritation in Inheritations)
                {
                    inheritation.CodeGen(builder);
                    builder.EndOfSeparatedListItem();
                }

                builder.EndOfSeparatedList();
            }

            builder.BeginBlock();

            foreach (var constant in Constants)
            {
                constant.CodeGen(builder);
            }

            // Leerzeile nach den Konstanten
            if (Constants.Any())
                builder.EndOfLineBlock();

            // Konstruktor erzeugen
            var constructorProperties = Properties.Where(p => p.SetInConstructor).ToList();
            if (constructorProperties.Any())
            {
                CodeGenConstructor(builder, constructorProperties);
            }

            foreach (var property in Properties)
            {
                property.CodeGen(builder);
            }

            foreach (var method in Methods)
            {
                method.CodeGen(builder);
            }

            foreach (string code in StaticCode)
            {
                builder.AppendStaticCode(code);
            }

            builder.EndBlock();
        }

        private void CodeGenConstructor(CodeBuilder builder, IReadOnlyList<PropertyBuilder> constructorProperties)
        {
            builder.AppendToken("public");
            builder.AppendToken(Name);
            builder.ForceNoWhitespace();
            builder.AppendToken("(");
            builder.ForceNoWhitespace();
            builder.BeginSeparatedList(",");

            foreach (var property in constructorProperties)
            {
                property.Type.CodeGen(builder);
                builder.AppendToken(property.GenerateParameterName());
                builder.EndOfSeparatedListItem();
            }

            builder.EndOfSeparatedList();
            builder.AppendToken(")");

            builder.BeginBlock();

            foreach (var property in constructorProperties)
            {
                builder.AppendToken(property.Name);
                builder.AppendToken("=");
                builder.AppendToken(property.GenerateParameterName());
                builder.EndOfStatement();
            }

            builder.EndBlock();
        }
    }
}
