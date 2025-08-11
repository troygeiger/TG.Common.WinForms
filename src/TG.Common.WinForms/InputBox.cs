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
    /// A reusable modal dialog prompting the user for a single textual value with optional search integration.
    /// </summary>
    public partial class InputBox : Form
    {
        Type searchForm = null;
        bool _allowBlank = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="InputBox"/> class.
    /// </summary>
        public InputBox()
        {
            InitializeComponent();
            this.Load += InputBox_Load; 
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            txtValue.Select();
        }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputBox"/> class using the specified title, description, and value.
    /// </summary>
        /// <param name="title">The title of the input box.</param>
        /// <param name="description">A brief description of the information being requested.</param>
        /// <param name="value">A value to initialize the <see cref="InputBox"/> with.</param>
        public InputBox(string title, string description, string value) : this()
        {
            this.Text = title;
            Description = description;
            Value = value;
        }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputBox"/> class with optional search form integration.
    /// </summary>
        /// <param name="title">The title of the input box.</param>
        /// <param name="description">A brief description of the information being requested.</param>
        /// <param name="value">A value to initialize the <see cref="InputBox"/> with.</param>
        /// <param name="searchFormType">A type that is a base type of <see cref="SearchFormBase"/> that an instance will be created when the search button is clicked.</param>
        public InputBox(string title, string description, string value, Type searchFormType): this(title, description, value)
        {
            this.SearchForm = searchFormType;
        }

        /// <summary>
        /// The title of the <see cref="InputBox"/>.
        /// </summary>
        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        /// <summary>
        /// A brief description of the information being requested.
        /// </summary>
        public string Description
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }

        /// <summary>
        /// The value of the text box.
        /// </summary>
        public string Value
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }

        /// <summary>
        /// Get or set whether the value can be blank.
        /// </summary>
        public bool AllowBlankValue
        {
            get => _allowBlank;
            set
            {
                _allowBlank = value;
                UpdateOkButton();
            }
        }

        /// <summary>
        /// Get or set whether to accept multi-line text.
        /// </summary>
        public bool MultiLine
        {
            get => txtValue.Multiline;
            set
            {
                txtValue.Multiline = value;
                if (value)
                {
                    txtValue.Margin = new Padding(3);
                    this.Height = 320;
                    this.AcceptButton = null;
                }
                else
                {
                    txtValue.Margin = new Padding(3, 5, 3, 3);
                    this.Height = 160;
                    this.AcceptButton = AcceptButton;
                }
            }
        }

    /// <summary>
    /// Gets or sets a search form type derived from <see cref="SearchFormBase"/> that will be instantiated when the user clicks the search button.
    /// </summary>
        public Type SearchForm
        {
            get
            {
                return searchForm;
            }
            set
            {
                if (value != null && value.BaseType != typeof(SearchFormBase))
                    throw new Exception("Type must be a base type of SearchFormBase.");
                searchForm = value;
                btnSearch.Visible = value != null;
            }
        }

        /// <summary>
        /// Get or set if the value should show the password characters.
        /// </summary>
        public bool IsPassword
        {
            get { return txtValue.UseSystemPasswordChar; }
            set { txtValue.UseSystemPasswordChar = value; }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            UpdateOkButton();
        }

        private void UpdateOkButton()
        {
            if (AllowBlankValue)
            {
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = txtValue.TextLength > 0;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (searchForm == null)
                    return;
                SearchFormBase form = Activator.CreateInstance(searchForm) as SearchFormBase;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    switch (form.ValueReplaceOption)
                    {
                        case ValueOptions.Replace:
                            txtValue.Text = form.ResultValue;
                            break;
                        case ValueOptions.Append:
                            txtValue.Text += form.ResultValue;
                            break;
                        case ValueOptions.AppendSemiColonSeparated:
                            if (!string.IsNullOrEmpty(txtValue.Text))
                                txtValue.Text = form.ResultValue;
                            else
                                txtValue.Text += (";" + form.ResultValue);
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            catch (Exception)
            {
                
            }
        }

    /// <summary>
    /// Gets a value indicating whether the OK button is currently enabled.
    /// This reflects the evaluation performed in <see cref="UpdateOkButton"/>,
    /// based on <see cref="AllowBlankValue"/> and the current text length.
    /// </summary>
    public bool OkButtonEnabled => btnOk.Enabled;
    }
}
