using Syncfusion.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace SfDataGridDemo
{
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
}
