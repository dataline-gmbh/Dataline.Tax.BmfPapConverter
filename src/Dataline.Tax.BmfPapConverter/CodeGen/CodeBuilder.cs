// <copyright file="CodeBuilder.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class CodeBuilder
    {
        private enum CodePositions
        {
            LineStart,
            BlockStart,
            TokenEnd,
            TokenEndNoWhitespace,
            OpenParenEnd,
            StatementEnd,
            BlockEnd
        }

        private static readonly char[] NeedsWhitespaceChars =
        {
            '_', ':', '{', '}', '=', '*', '/', '+', '-', '%', '>',
            '<', '&', '|'
        };

        private readonly StringBuilder _code = new StringBuilder();
        private readonly Stack<string> _separators = new Stack<string>();
        private CodePositions _position = CodePositions.LineStart;
        private int _blockLevel = 0;
        private bool _insertSeparator = false;

        public int IndentationWidth { get; set; } = 4;

        public Func<string, string> StaticCodeModifier { get; set; }

        public void AppendToken(string token, bool trim = true)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            if (trim)
                token = token.Trim();

            if (_insertSeparator)
            {
                _insertSeparator = false;
                AppendToken(_separators.Peek());
            }

            PrepareSpace(token);

            _code.Append(token);

            _position = token == "(" ? CodePositions.OpenParenEnd : CodePositions.TokenEnd;
        }

        public void ForceNoWhitespace()
        {
            _position = CodePositions.TokenEndNoWhitespace;
        }

        public void EndOfStatement()
        {
            _code.Append(";");

            _position = CodePositions.StatementEnd;
        }

        public void EndOfLine()
        {
            if (_position != CodePositions.LineStart)
                _position = CodePositions.StatementEnd;
        }

        public void EndOfLineBlock()
        {
            if (_position != CodePositions.LineStart && _position != CodePositions.BlockStart)
                _position = CodePositions.BlockEnd;
        }

        public void BeginBlock()
        {
            if (_position != CodePositions.LineStart)
                _code.AppendLine();

            _position = CodePositions.LineStart;

            AppendToken("{");
            _blockLevel++;

            _position = CodePositions.BlockStart;
        }

        public void EndBlock()
        {
            if (_blockLevel == 0)
                throw new InvalidOperationException("Blocklevel ist 0");

            if (_position != CodePositions.LineStart)
                _code.AppendLine();

            _position = CodePositions.LineStart;

            _blockLevel--;
            AppendToken("}");

            _position = CodePositions.BlockEnd;
        }

        public void BeginSeparatedList(string separatorToken)
        {
            _separators.Push(separatorToken);
        }

        public void EndOfSeparatedListItem()
        {
            if (!_separators.Any())
                throw new InvalidOperationException("Keine Separatorliste gestartet");

            _insertSeparator = true;
        }

        public void EndOfSeparatedList()
        {
            _separators.Pop();
            _insertSeparator = false;
        }

        public void AppendMultiLineComment(string comment)
        {
            EndOfLineBlock();

            var lines = comment.Split('\n').Select(c => c.TrimEnd().Replace("*/", "* /"));

            AppendToken("/*");
            EndOfLine();

            foreach (string line in lines)
            {
                AppendToken(" * ", trim: false);
                _code.Append(line);
                EndOfLine();
            }

            AppendToken(" */", trim: false);

            EndOfLineBlock();
        }

        public void AppendSummary(string summary)
        {
            EndOfLineBlock();

            var lines = summary.Split('\n').Select(c => c.Trim().Replace("<", "&lt;").Replace(">", "&gt;"));

            AppendToken("/// <summary>");
            EndOfLine();

            foreach (string line in lines)
            {
                AppendToken("/// ", trim: false);
                _code.Append(line);
                EndOfLine();
            }

            AppendToken("/// </summary>");

            EndOfLine();
        }

        public void AppendStaticCode(string code)
        {
            EndOfLineBlock();

            if (StaticCodeModifier != null)
                code = StaticCodeModifier(code);

            var lines = code.Split('\n').Select(c => c.TrimEnd());

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    EndOfLineBlock();
                }
                else
                {
                    AppendToken(line, trim: false);
                    EndOfLine();
                }
            }

            EndOfLineBlock();
        }

        public override string ToString()
        {
            return _code.ToString();
        }

        private void Indent()
        {
            for (int i = _blockLevel * IndentationWidth; i > 0; i--)
                _code.Append(' ');
        }

        private bool NeedsWhitespace(string token)
        {
            if (token.StartsWith("(") && _position == CodePositions.TokenEnd)
                return true;

            char first = token[0];
            return char.IsLetterOrDigit(first) || NeedsWhitespaceChars.Contains(first);
        }

        private void PrepareSpace(string token = null)
        {
            switch (_position)
            {
                case CodePositions.LineStart:
                    Indent();
                    break;
                case CodePositions.TokenEnd:
                    if (token == null || NeedsWhitespace(token))
                        _code.Append(' ');
                    break;
                case CodePositions.TokenEndNoWhitespace:
                    break;
                case CodePositions.BlockStart:
                case CodePositions.StatementEnd:
                    _code.AppendLine();
                    Indent();
                    break;
                case CodePositions.BlockEnd:
                    _code.AppendLine();
                    _code.AppendLine();
                    Indent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

