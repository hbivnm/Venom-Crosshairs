using System.Collections;
using System.Windows.Forms;

namespace VenomCrosshairs
{
    internal class ListViewItemComparer : IComparer
    {
        private int col;

        public ListViewItemComparer(int column)
        {
            this.col = column;
        }

        public int Compare(object x, object y)
        {
            ListViewItem itemX = (ListViewItem)x;
            ListViewItem itemY = (ListViewItem)y;

            return string.Compare(itemX.SubItems[col].Text, itemY.SubItems[col].Text);
        }
    }
}
