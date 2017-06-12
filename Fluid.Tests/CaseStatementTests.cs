﻿using System.IO;
using System.Text.Encodings.Web;
using Fluid.Ast;
using Fluid.Values;
using Xunit;

namespace Fluid.Tests
{
    public class CaseStatementTests
    {
        private LiteralExpression A = new LiteralExpression(new StringValue("a"));
        private LiteralExpression B = new LiteralExpression(new StringValue("b"));
        private LiteralExpression C = new LiteralExpression(new StringValue("c"));
        private LiteralExpression D = new LiteralExpression(new StringValue("d"));

        private Statement[] TEXT(string text)
        {
            return new Statement[] { new TextStatement(text) };
        }

        [Fact]
        public void CaseCanProcessWhenMatch()
        {
            var e = new CaseStatement(
                A,
                null,
                new[] {
                    new WhenStatement(new [] { A }, TEXT("x"))
                }
            );

            var sw = new StringWriter();
            e.WriteTo(sw, HtmlEncoder.Default, new TemplateContext());

            Assert.Equal("x", sw.ToString());
        }

        [Fact]
        public void CaseCanProcessWhenMatchMultiple()
        {
            var e = new CaseStatement(
                A,
                null,
                new[] {
                    new WhenStatement(new [] { A, B, C }, TEXT("x"))
                }
            );

            var sw = new StringWriter();
            e.WriteTo(sw, HtmlEncoder.Default, new TemplateContext());

            Assert.Equal("x", sw.ToString());
        }

        [Fact]
        public void CaseDoesntProcessWhenNoMatch()
        {
            var e = new CaseStatement(
                A,
                null,
                new[] {
                    new WhenStatement(new [] { B, C, D }, TEXT("x"))
                }
            );

            var sw = new StringWriter();
            e.WriteTo(sw, HtmlEncoder.Default, new TemplateContext());

            Assert.Equal("", sw.ToString());
        }

        [Fact]
        public void CaseDoesntProcessElseWhenMatch()
        {
            var e = new CaseStatement(
                A,
                new ElseStatement(new[] { new TextStatement("y")}),
                new[] {
                    new WhenStatement(new [] { A }, TEXT("x"))
                }
            );

            var sw = new StringWriter();
            e.WriteTo(sw, HtmlEncoder.Default, new TemplateContext());

            Assert.Equal("x", sw.ToString());
        }

        [Fact]
        public void CaseProcessElseWhenNoMatch()
        {
            var e = new CaseStatement(
                A,
                new ElseStatement(new[] { new TextStatement("y") }),
                new[] {
                    new WhenStatement(new [] { B, C }, TEXT("x"))
                }
            );

            var sw = new StringWriter();
            e.WriteTo(sw, HtmlEncoder.Default, new TemplateContext());

            Assert.Equal("y", sw.ToString());
        }
        

        [Fact]
        public void CaseProcessFirstWhen()
        {
            var e = new CaseStatement(
                B,
                new ElseStatement(new[] { new TextStatement("y") }),
                new[] {
                    new WhenStatement(new [] { A, C }, TEXT("1")),
                    new WhenStatement(new [] { B, C }, TEXT("2")),
                    new WhenStatement(new [] { C }, TEXT("3"))
                }
            );

            var sw = new StringWriter();
            e.WriteTo(sw, HtmlEncoder.Default, new TemplateContext());

            Assert.Equal("2", sw.ToString());
        }

        [Fact]
        public void CaseProcessNoMatchWhen()
        {
            var e = new CaseStatement(
                A,
                null,
                new[] {
                    new WhenStatement(new [] { B }, TEXT("2")),
                    new WhenStatement(new [] { C }, TEXT("3"))
                }
            );

            var sw = new StringWriter();
            e.WriteTo(sw, HtmlEncoder.Default, new TemplateContext());

            Assert.Equal("", sw.ToString());
        }
    }
}
