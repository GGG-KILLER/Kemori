namespace GUtils.Forms.DataGridView
{
    // Source: https://social.msdn.microsoft.com/Forums/windows/en-US/769ca9d6-1e9d-4d76-8c23-db535b2f19c2/sample-code-datagridview-progress-bar-column?forum=winformsdatacontrols
    public class DataGridViewProgressColumn : System.Windows.Forms.DataGridViewImageColumn
    {
        public DataGridViewProgressColumn ( )
        {
            CellTemplate = new DataGridViewProgressCell ( );
        }
    }
}