using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FubuCore.Reflection;
using FubuMVC.Core.View;
using FubuMVC.UI.Tags;
using HtmlTags;

namespace FubuMVC.UI.Tables
{
    public static class TableGeneratorExtensions
    {
        /// <summary>
        /// Generates a table using the model of the given view, an expression that evaluates as a list of data points
        /// (one for each row in the table), and one or more expressions for evaluting the data displayed in each column.
        /// </summary>
        /// <param name="page">The page where the table will appear.</param>
        /// <param name="itemsExpression">The expression which results in the data point items, used for each row of the table.</param>
        /// <param name="columnExpressions">
        /// One or more expression to be used to evaluate the column in the table.  
        /// Resulting column data will be run through the 'DisplayFor' mechanism.
        /// </param>
        /// <returns>An HtmlTag that represents the table.</returns>
        public static HtmlTag TableFor<TModel, TItem>(this IFubuPage<TModel> page,
                                                      Expression<Func<TModel, IEnumerable<TItem>>> itemsExpression,
                                                      params Expression<Func<TItem, object>>[] columnExpressions)
            where TModel : class
            where TItem : class
        {
            var model = page.Model;
            var items = itemsExpression.Compile().Invoke(model);
            var generator = TableGeneratorFor(page, itemsExpression, columnExpressions);
            var table = generator.GenerateTable(items);
            return table;
        }

        /// <summary>
        /// Create a table generator using the model of the given view, an expression that evaluates as a list of data points
        /// (one for each row in the table), and one or more expressions for evaluting the data displayed in each column.
        /// </summary>
        /// <param name="page">The page used to obtain the object to be used to evaluate the items.</param>
        /// <param name="itemsExpression">The expression which results in the items to be used for each row of the table.</param>
        /// <param name="columnExpressions">
        /// One or more expression to be used to evaluate the column in the table.  
        /// Resulting column data will be run through the 'DisplayFor' mechanism.
        /// </param>
        /// <returns>A TableGenerator object that may be used to generate an Html table.</returns>
        public static TableGenerator<TItem> TableGeneratorFor<TModel, TItem>(this IFubuPage<TModel> page,
                                                      Expression<Func<TModel, IEnumerable<TItem>>> itemsExpression,
                                                      params Expression<Func<TItem, object>>[] columnExpressions)
            where TModel : class
            where TItem : class
        {
            var tagGen = page.Get<ITagGenerator<TItem>>();
            var generator = new TableGenerator<TItem>(tagGen);
            columnExpressions.ToList().ForEach(ce => generator.AddColumnWithDisplayFor(ce, ce.GetName()));
            return generator;
        }
    }
}
