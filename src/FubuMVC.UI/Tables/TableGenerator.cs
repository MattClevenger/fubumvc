using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FubuCore.Reflection;
using FubuMVC.UI.Tags;
using HtmlTags;

namespace FubuMVC.UI.Tables
{
    public interface ITableGenerator<TTableData>
        where TTableData : class
    {
        HtmlTag GenerateTable(IEnumerable<TTableData> data);
    }

    /// <summary>
    /// Generates a table given an array of objects as table data. 
    /// Specify the columns and headers using the 'AddColumnXXX' methods.
    /// Optionally, specify funcitons for transforming the header row, and/or, body rows.
    /// Using the 'AddColumnWithDisplayFor' methods will run resulting column data through the 'DisplayFor' mechanism.
    /// Using the 'AddColumnUsingTransformation' methods will NOT run the resulting colunn data through the 'DisplayFor' mechanism.
    /// </summary>
    /// <typeparam name="TTableData">The type of the data used for generating each row in the table.</typeparam>
    public class TableGenerator<TTableData> : ITableGenerator<TTableData>
        where TTableData : class
    {
        private readonly ITagGenerator<TTableData> _tags;
        private bool _showHeader = true;
        private List<ColumnDef<TTableData>> _columnDefs = new List<ColumnDef<TTableData>>();
        private List<Action<TableRowTag>> _headerRowTransformers = new List<Action<TableRowTag>>();
        private List<Action<TableRowTag>> _rowTransformers = new List<Action<TableRowTag>>();

        /// <summary>The constructor for the TableGenerator instance</summary>
        /// <param name="tags">The ITagGenerator to be used when running an expression 
        /// for a row (which will be used as the model of the ITagGenerator and is of type TTableData) 
        /// and column (via the expression) through the 'DisplayFor' mechanism.
        /// </param>
        public TableGenerator(ITagGenerator<TTableData> tags)
        {
            _tags = tags;
        }


        /// <summary>Generates the Html representation of the table data based on the 
        /// columns/headers/transforations to be used (see the 'Add___' methods). </summary>
        /// <param name="data">The enumerable collection of data objects to be used for generating the table.
        /// The table will have one row of data per array element.</param>
        public HtmlTag GenerateTable(IEnumerable<TTableData> data)
        {
            var table = new TableTag();
            if (_showHeader) AddHeaderRow(table);
            data.ToList().ForEach(dataItem => AddRow(table, dataItem));
            return table;
        }

        /// <summary>Specify that the generator will not generate a header row.</summary>
        public TableGenerator<TTableData> NoHeader()
        {
            _showHeader = false;
            return this;
        }

        /// <summary>Transform the table header row.</summary>
        /// <param name="transformer">The transformation action delegate.</param>
        public TableGenerator<TTableData> AddHeaderRowTransformer(Action<TableRowTag> transformer)
        {
            _headerRowTransformers.Add(transformer);
            return this;
        }

        /// <summary>Add a column to the table using the given ColumnDef object.</summary>
        /// <param name="columnDef">The definition of the column.</param>
        public TableGenerator<TTableData> AddColumn(ColumnDef<TTableData> columnDef)
        {
            _columnDefs.Add(columnDef);
            return this;
        }

        /// <summary>Add a column to the table using the property expression supplied to specify the default header text.
        /// Get the data used for the column by evaluating the expression for each row and processing it through the 'DisplayFor' mechanism.
        /// </summary>
        /// <param name="dataPropertyExpression">An expression for evalutating the column.</param>
        /// <param name="columnTransformers">Zero or more transformation fuctions to be applied to the column.</param>
        public TableGenerator<TTableData> AddColumnWithDefaultHeaderAndDisplayFor(Expression<Func<TTableData, object>> dataPropertyExpression, params Action<HtmlTag>[] columnTransformers)
        {
            _columnDefs.Add(new ColumnDef<TTableData>(dataPropertyExpression, columnTransformers));
            return this;
        }

        /// <summary>Add a column with an empty header to the table using the property expression supplied.
        /// Get the data used for the column by evaluating the expression for each row and processing it through the 'DisplayFor' mechanism.
        /// </summary>
        /// <param name="dataPropertyExpression">An expression for evalutating the column.</param>
        /// <param name="columnTransformers">Zero or more transformation fuctions to be applied to the column.</param>
        public TableGenerator<TTableData> AddColumnWithDisplayFor(Expression<Func<TTableData, object>> dataPropertyExpression, params Action<HtmlTag>[] columnTransformers)
        {
            _columnDefs.Add(new ColumnDef<TTableData>(dataPropertyExpression, string.Empty, columnTransformers));
            return this;
        }

        /// <summary>Add a column with the given header text to the table using the property expression supplied.
        /// Get the data used for the column by evaluating the expression for each row and processing it through the 'DisplayFor' mechanism.
        /// </summary>
        /// <param name="dataPropertyExpression">An expression for evalutating the column.</param>
        /// <param name="header">The text to be used for the column header.</param>
        /// <param name="columnTransformers">Zero or more transformation fuctions to be applied to the column.</param>
        public TableGenerator<TTableData> AddColumnWithDisplayFor(Expression<Func<TTableData, object>> dataPropertyExpression, string header, params Action<HtmlTag>[] columnTransformers)
        {
            _columnDefs.Add(new ColumnDef<TTableData>(dataPropertyExpression, header, columnTransformers));
            return this;
        }

        /// <summary>Add a column with the given header HtmlTag to the table using the property expression supplied.
        /// Get the data used for the column by evaluating the expression for each row and processing it through the 'DisplayFor' mechanism.
        /// </summary>
        /// <param name="dataPropertyExpression">An expression for evalutating the column.</param>
        /// <param name="header">The HtmlTag to be used for the column header.</param>
        /// <param name="columnTransformers">Zero or more transformation fuctions to be applied to the column.</param>
        public TableGenerator<TTableData> AddColumnWithDisplayFor(Expression<Func<TTableData, object>> dataPropertyExpression, HtmlTag header, params Action<HtmlTag>[] columnTransformers)
        {
            _columnDefs.Add(new ColumnDef<TTableData>(dataPropertyExpression, header, columnTransformers));
            return this;
        }

        /// <summary>Add a column with no header to the table using the transformation function supplied.
        /// The resulting object will NOT be run through the 'DisplayFor' mechanism.</summary>
        /// <param name="dataTransformer">An function applied to the table data for the column.</param>
        public TableGenerator<TTableData> AddColumnUsingTransformation(Func<TTableData, object> dataTransformer)
        {
            _columnDefs.Add(new ColumnDef<TTableData>(dataTransformer, string.Empty));
            return this;
        }

        /// <summary>Add a column with the given header text to the table using the transformation function supplied.
        /// The resulting object will NOT be run through the 'DisplayFor' mechanism.
        /// </summary>
        /// <param name="dataTransformer">An function applied to the table data for the column.</param>
        /// <param name="header">The text to be used for the column header.</param>
        public TableGenerator<TTableData> AddColumnUsingTransformation(Func<TTableData, object> dataTransformer, string header)
        {
            _columnDefs.Add(new ColumnDef<TTableData>(dataTransformer, header));
            return this;
        }

        /// <summary>Add a column with the given header HtmlTag to the table using the transformation function supplied.
        /// The resulting object will NOT be run through the 'DisplayFor' mechanism.
        /// </summary>
        /// <param name="dataTransformer">An function applied to the table data for the column.</param>
        /// <param name="header">The HtmlTag to be used for the column header.</param>
        public TableGenerator<TTableData> AddColumnUsingTransformation(Func<TTableData, object> dataTransformer, HtmlTag header)
        {
            _columnDefs.Add(new ColumnDef<TTableData>(dataTransformer, header));
            return this;
        }

        /// <summary>Specify the transformer to be used on each table row.</summary>
        /// <param name="transformer">The transformation action delegate to be applied to each row.</param>
        public TableGenerator<TTableData> AddRowTransformer(Action<TableRowTag> transformer)
        {
            _rowTransformers.Add(transformer);
            return this;
        }

        private void AddHeaderRow(TableTag table)
        {
            var headerRow = table.AddHeaderRow();
            _columnDefs.ForEach(cd => AddHeaderColumn(headerRow, cd));
            _headerRowTransformers.ForEach(t => t(headerRow));
        }

        private static void AddHeaderColumn(TableRowTag headerRow, ColumnDef<TTableData> columnDef)
        {
            var column = columnDef.ColumnHeaderText == null
                        ? headerRow.Header().AddChildren(columnDef.ColumnHeaderTag) // either have a tag inside the td
                        : headerRow.Header(columnDef.ColumnHeaderText); // or some text in the td element
            columnDef.HeaderColumnTransformers.ToList().ForEach(t => t(column));
        }

        private void AddRow(TableTag table, TTableData rowItem)
        {
            var row = table.AddBodyRow();
            _columnDefs.ForEach(cd => AddRowColumn(row, rowItem, cd));
            _rowTransformers.ForEach(t => t(row));
        }

        private void AddRowColumn(TableRowTag row, TTableData rowItem, ColumnDef<TTableData> columnDef)
        {
            _tags.SetModel(rowItem); // could have called setter, but this is testable
            var column = ShouldUseDisplayFor(columnDef) 
                            ? BuildColumnWithDisplayFor(columnDef, row)
                            : BuildColumnUsingTransformation(rowItem, columnDef, row);
        }

        private static HtmlTag BuildColumnUsingTransformation(TTableData rowItem, ColumnDef<TTableData> columnDef, TableRowTag row)
        {
            var data = columnDef.PropertyTransformation(rowItem);
            var tag = data as HtmlTag;
            if (tag == null)
            {
                return row.Cell(data.ToString());
            }

            return row.Cell().AddChildren(tag);
        }

        private HtmlTag BuildColumnWithDisplayFor(ColumnDef<TTableData> columnDef, TableRowTag row)
        {
            var column = row.Cell();
            var columnData = _tags.DisplayFor(columnDef.PropertyExpression);
            column.AddChildren(columnData);
            columnDef.ColumnTransformers.ToList().ForEach(t => t(column));
            return column;
        }

        private static bool ShouldUseDisplayFor(ColumnDef<TTableData> columnDef)
        {
            return columnDef.PropertyExpression != null;
        }
    }

    public class ColumnDef<T>
        where T : class
    {
        public Expression<Func<T, object>> PropertyExpression { get; private set; }
        public Func<T, object> PropertyTransformation { get; private set; }
        public string ColumnHeaderText { get; private set; }
        public HtmlTag ColumnHeaderTag { get; private set; }
        public Action<HtmlTag>[] ColumnTransformers { get; private set; }
        public Action<HtmlTag>[] HeaderColumnTransformers { get; set; }

        public ColumnDef(Expression<Func<T, object>> expression, params Action<HtmlTag>[] transformers)
        {
            PropertyExpression = expression;
            ColumnHeaderText = expression.GetName();
            ColumnTransformers = transformers;
            Init();
        }

        public ColumnDef(Expression<Func<T, object>> expression, string columnHeaderText, params Action<HtmlTag>[] transformers)
        {
            PropertyExpression = expression;
            ColumnHeaderText = columnHeaderText;
            ColumnTransformers = transformers;
            Init();
        }

        public ColumnDef(Expression<Func<T, object>> expression, HtmlTag columnHeaderTag, params Action<HtmlTag>[] transformers)
        {
            PropertyExpression = expression;
            ColumnHeaderTag = columnHeaderTag;
            ColumnTransformers = transformers;
            Init();
        }

        public ColumnDef(Func<T, object> transformer, string columnHeaderText)
        {
            PropertyTransformation = transformer;
            ColumnHeaderText = columnHeaderText;
            Init();
        }

        public ColumnDef(Func<T, object> transformer, HtmlTag columnHeaderTag)
        {
            PropertyTransformation = transformer;
            ColumnHeaderTag = columnHeaderTag;
            Init();
        }

        private void Init()
        {
            HeaderColumnTransformers = new Action<HtmlTag>[0]; 
        }
    }

}