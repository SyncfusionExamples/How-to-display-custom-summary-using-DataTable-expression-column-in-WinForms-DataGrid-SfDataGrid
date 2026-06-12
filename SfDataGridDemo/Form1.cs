using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SfDataGridDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

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

            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "TOTAL_MINUTO",
                HeaderText = "TOTAL_MINUTO",
            });

            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "TOTAL_FORMATADO",
                HeaderText = "TOTAL_FORMATADO"
            });

            // Set LiveDataUpdateMode to allow data shaping so that summaries are updated when data changes
            sfDataGrid1.LiveDataUpdateMode = LiveDataUpdateMode.AllowDataShaping;

            // Set AutoSizeColumnsMode to ColumnHeader to ensure columns are sized based on header text
            sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.ColumnHeader;

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
        }
    }
}