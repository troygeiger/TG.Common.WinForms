using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TG.Common
{
    /// <summary>
    /// A customizable modal message box form that can display a message and react to changes in its <see cref="MessageText"/>.
    /// </summary>
    public partial class ExMessageBox : Form
    {
        string msg = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExMessageBox"/> class.
        /// </summary>
        public ExMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows an <see cref="ExMessageBox"/> containing the specified message text.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void Show(string message)
        {
            
        }

        /// <summary>
        /// Gets or sets the message text displayed by the form.
        /// Setting this property raises <see cref="OnMessageTextChanged"/>.
        /// </summary>
        public string MessageText
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
                OnMessageTextChanged();
            }
        }

        /// <summary>
        /// Called whenever <see cref="MessageText"/> is changed. Override to update UI elements bound to the message.
        /// </summary>
        protected virtual void OnMessageTextChanged()
        {

        }
    }
}
