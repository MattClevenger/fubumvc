using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FubuMVC.UI.Tables;
using FubuMVC.UI.Tags;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.UI.Tables
{
    [TestFixture]
    public class TableGeneratorTester
    {
        private ITagGenerator<MyModel> _tags;
        private TableGenerator<MyModel> _generator;
        private MyModel[] _data;
        private List<MyModel> _capturedModels;
        private MyModel _currentModel;
        private List<Expression<Func<MyModel, object>>> _capturedExpressions;
        private Expression<Func<MyModel, object>> _barExp = (m) => m.Bar;
        private Expression<Func<MyModel, object>> _fooExp = (m) => m.Foo;
        private Expression<Func<MyModel, object>> _wooExp = (m) => m.Woo;
        private Expression<Func<MyModel, object>> _numberExp = (m) => m.Number;

        [SetUp]
        public void SetUp()
        {
            _tags = MockRepository.GenerateStub<ITagGenerator<MyModel>>();
            _generator = new TableGenerator<MyModel>(_tags);
 
            _data = new MyModel[]
                           {
                               new MyModel {Foo = new FooClass(), Bar = new BarClass(), Woo = "woo", Number = 1, NotUsed = 2},
                               new MyModel {Foo = new FooClass(), Bar = new BarClass(), Woo = "woohoo", Number = 2, NotUsed = 3},
                               new MyModel {Foo = new FooClass(), Bar = new BarClass(), Woo = "woodeedoo", Number = 3, NotUsed = 4}
                           };

            _generator.AddColumnWithDisplayFor(_barExp);
            _generator.AddColumnWithDisplayFor(_fooExp, "this is some foo");
            _generator.AddColumnWithDisplayFor(_wooExp, new LinkTag("a link to nowhere", "http://nowhere"));
            _generator.AddColumnWithDefaultHeaderAndDisplayFor(_numberExp, tr => tr.AddClass("number"));

            _capturedModels = new List<MyModel>();
            Action<object> modelCapture = m =>
                                              {
                                                  _currentModel = m as MyModel;
                                                  _capturedModels.Add(_currentModel);
                                              };
            _capturedExpressions = new List<Expression<Func<MyModel, object>>>();
            _tags.Stub(t => t.SetModel(null)).IgnoreArguments().Do(modelCapture);
            Func<Expression<Func<MyModel, object>>, HtmlTag> df = (e) =>
                                                                      {
                                                                          _capturedExpressions.Add(e);
                                                                          var data = e.Compile().Invoke(_currentModel);
                                                                          var tag = new HtmlTag("span").Text(data.ToString());
                                                                          return tag;
                                                                      };
            Expression<Func<MyModel, object>> exp = null;
            _tags.Stub(t => t.DisplayFor(exp)).IgnoreArguments().Do(df);
        }

        [Test]
        public void should_have_a_header_row_with_a_column_for_each_added_expression()
        {
            var table = _generator.GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());

            var headerTags = table.Children.Where(c => c.TagName() == "thead");
            headerTags.ShouldHaveCount(1);
            var headerRows = headerTags.First().Children.Where(c => c.TagName() == "tr");
            headerRows.ShouldHaveCount(1);
            var headerCols = headerRows.First().Children;
            headerCols.ShouldHaveCount(4);
        }

        [Test]
        public void should_not_have_a_header_row_when_NoHeader_called()
        {
            var table = _generator.NoHeader().GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());

            var headerTags = table.Children.Where(c => c.TagName() == "thead");
            headerTags.ShouldHaveCount(1);
            headerTags.First().Children.ShouldHaveCount(0);
        }

        [Test]
        public void should_have_a_row_for_each_item_in_data_enumeration()
        {
            var table = _generator.NoHeader().GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());

            var rows = table.Children.Where(c => c.TagName() == "tbody").First().Children;
            rows.ShouldHaveCount(_data.Length);
        }

        [Test]
        public void for_each_expression_added_as_column_should_have_a_column_in_each_table_row()
        {
            var table = _generator.NoHeader().GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());

            var rows = table.Children.Where(c => c.TagName() == "tbody").First().Children;
            rows.ToList().ForEach(r => r.Children.ShouldHaveCount(4));
        }

        [Test]
        public void should_process_each_table_columns_data_through_tag_generator()
        {
            var table = _generator.NoHeader().GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());

            _data.ToList().ForEach(m => _capturedModels.ShouldContain(m));
            _capturedExpressions.Where(e => e == _barExp).ShouldHaveCount(_data.Count());
            _capturedExpressions.Where(e => e == _fooExp).ShouldHaveCount(_data.Count());
            _capturedExpressions.Where(e => e == _wooExp).ShouldHaveCount(_data.Count());
            
        }

        [Test]
        public void should_transform_header_row()
        {
            _generator.AddHeaderRowTransformer(t => t.AddClass("header"));

            var table = _generator.GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());


            var headerTag = table.Children.Where(c => c.TagName() == "thead").First();
            var headerRow = headerTag.Children.Where(c => c.TagName() == "tr").First();
            headerRow.HasClass("header").ShouldBeTrue();
        }

        [Test]
        public void should_transform_all_body_rows()
        {
            _generator.AddRowTransformer(t => t.AddClass("body"));

            var table = _generator.GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());

            var body = table.Children.Where(c => c.TagName() == "tbody").First();
            var bodyRows = body.Children.Where(c => c.TagName() == "tr");
            bodyRows.ToList().ForEach(r => r.HasClass("body").ShouldBeTrue());
        }

        [Test]
        public void should_transform_all_body_columns()
        {
            var table = _generator.GenerateTable(_data);
//            Console.WriteLine(table.ToPrettyString());

            var body = table.Children.Where(c => c.TagName() == "tbody").First();
            var bodyRows = body.Children.Where(c => c.TagName() == "tr");
            bodyRows.ToList().ForEach(r =>
                                          {
                                              var lastColumn = r.Children.Where(c => c.TagName() == "td").Last();
                                              lastColumn.HasClass("number").ShouldBeTrue();
                                          });
        }
    }


    public class MyModel
    {
        public FooClass Foo { get; set; }
        public BarClass Bar { get; set; }
        public string Woo { get; set; }
        public int Number { get; set; }
        public int NotUsed { get; set; }     
    }

    public class BarClass
    {
        public override string ToString()
        {
            return "Bar!";
        }
    }

    public class FooClass
    {
        public override string ToString()
        {
            return "Foo!";
        }
    }
}
