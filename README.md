# How to display custom summary using DataTable expression column in WinForms DataGrid (SfDataGrid)?

In [WinForms DataGrid](https://www.syncfusion.com/winforms-ui-controls/datagrid) (SfDataGrid), when using a **DataTable** as the data source, you can utilize the **Expression column** to display values based on other column values. In this article, we demonstrate how to display a **custom summary** using an **Expression column** along with a **custom aggregate**. Since the data source is a DataTable, the Expression column can be used to calculate values derived from other columns.

**Code snippet for setting a DataTable as the data source:**

```csharp
// Create data table
 DataTable table = new DataTable();

 // Define columns
 // Add a column for total minutes
 table.Columns.Add(
     "TOTAL_MINUTO", 
     typeof(int));

 // Add an expression column to calculate formatted time based on total minutes
 table.Columns.Add(
     "TOTAL_FORMATADO", 
     typeof(string), 
     "((TOTAL_MINUTO - (TOTAL_MINUTO % 60)) / 60) + 'h ' + (TOTAL_MINUTO % 60) + 'm'");

 // Add rows
 table.Rows.Add(71);
 table.Rows.Add(21);
 table.Rows.Add(120);
 table.Rows.Add(180);
 table.Rows.Add(30);
 table.Rows.Add(90);
 table.Rows.Add(60);

 // Set data source
 sfDataGrid1.DataSource = table;
```
**Code snippet for creating a custom aggregate:**

```csharp
public class TotalMinutesAggregate : ISummaryAggregate
{
    public string FormattedTotal { get; set; }

    public Action<IEnumerable, string, PropertyDescriptor> CalculateAggregateFunc()
    {
        return (items, property, pd) =>
        {
            // Ensure we process only our property
            if (pd != null && pd.Name == nameof(FormattedTotal))
            {
                int totalMinutes = 0;

                foreach (var item in items)
                {
                    // Check if the item is a DataRowView and has a valid TOTAL_MINUTO value
                    if (item is DataRowView rowView &&
                        rowView.Row["TOTAL_MINUTO"] != DBNull.Value)
                    {
                        totalMinutes += Convert.ToInt32(rowView.Row["TOTAL_MINUTO"]);
                    }
                }

                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                FormattedTotal = $"{hours}h {minutes} min";
            }
        };
    }
}
```

**Code snippet for assigning the custom aggregate for summary row:**

```csharp
 // Create table summary row
 GridTableSummaryRow tableSummaryRow = new GridTableSummaryRow
 {
     Name = "TimeSummary",
     ShowSummaryInRow = false,
     Position = Syncfusion.WinForms.DataGrid.Enums.VerticalPosition.Bottom
 };

 // Create summary column
 GridSummaryColumn summaryColumn1 = new GridSummaryColumn
 {
     Name = "TotalMinutes",
     SummaryType = SummaryType.DoubleAggregate,

     // Where the summary appears
     MappingName = "TOTAL_MINUTO",

     // Format uses property from custom aggregate
     Format = "{Sum}"
 };

 GridSummaryColumn summaryColumn2 = new GridSummaryColumn
 {
     Name = "FormattedTimeSummary",

     // Use custom summary type to calculate formatted total time
     SummaryType = SummaryType.Custom,

     // Use custom aggregate to calculate total time in hours and minutes
     CustomAggregate = new TotalMinutesAggregate(),

     // Where the summary appears
     MappingName = "TOTAL_FORMATADO",

     // Format uses property from custom aggregate
     Format = "{FormattedTotal}"
 };

 // Add summary column
 tableSummaryRow.SummaryColumns.Add(summaryColumn1);
 tableSummaryRow.SummaryColumns.Add(summaryColumn2);

 // Add summary row to grid
 sfDataGrid1.TableSummaryRows.Add(tableSummaryRow);
```

![Custom summary using expression column](Custom%20Summary%20Using%20Expression%20Column.gif)

Take a moment to peruse the [WinForms DataGrid - Summaries](https://help.syncfusion.com/windowsforms/datagrid/summaries) documentation, to learn more about summaries with example.