using System;
using System.Windows.Forms;

namespace TG.Common
{
    /// <summary>
    /// Base class for search dialogs that provide a value back to an <see cref="InputBox"/>.
    /// Override the provided virtual properties to control how values are returned.
    /// </summary>
    public partial class SearchFormBase : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFormBase"/> class.
        /// </summary>
        public SearchFormBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Repositions the OK / Cancel button panel when the form changes size.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            tableLayoutPanel1.Location =
                new System.Drawing.Point(
                    this.ClientSize.Width - tableLayoutPanel1.Width - tableLayoutPanel1.Margin.Right
             , this.ClientSize.Height - tableLayoutPanel1.Height - tableLayoutPanel1.Margin.Bottom);

        }

        /// <summary>
        /// Gets or sets the way the selected value should be applied back to an <see cref="InputBox"/>.
        /// </summary>
        public virtual ValueOptions ValueReplaceOption { get; set; }

        /// <summary>
        /// Gets or sets the initial value provided to the search form (for example, the text currently in the input box).
        /// </summary>
        public virtual string InitialValue { get; set; }

        /// <summary>
        /// Gets or sets the resulting value selected or constructed by the user.
        /// </summary>
        public virtual string ResultValue { get; set; }
    }
}
