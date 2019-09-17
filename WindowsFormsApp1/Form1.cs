using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Repositories;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly BackgroundWorker _getCustomersWorker;

        public Form1()
        {
            InitializeComponent();

            this._getCustomersWorker = new BackgroundWorker();
            this._getCustomersWorker.WorkerSupportsCancellation = false;
            this._getCustomersWorker.WorkerReportsProgress = true;
            this._getCustomersWorker.DoWork += _getCustomersWorker_DoWork;
            this._getCustomersWorker.RunWorkerCompleted += _getCustomersWorker_RunWorkerCompleted;
            this._getCustomersWorker.ProgressChanged += _getCustomersWorker_ProgressChanged;
        }

        void _getCustomersWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PagingInfo pagingInfo = e.Argument as PagingInfo;



            // this loop simulates some delays in processing
            // reportProgress is optional, and in case of DB data retrieval we should probably use marquee progress bar
            for (int iter = 0; iter < 100; iter += pagingInfo.PageSize) // just using pageSize to randomize the effec a bit
            {
                Thread.Sleep(700);
                _getCustomersWorker.ReportProgress(iter);
            }



            CustomerRepository cr = new CustomerRepository("connection string");
            e.Result = cr.GetCustomers(pagingInfo);
        }

        void _getCustomersWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button1.Enabled = true;
            this.progressBar1.Visible = false;
            this.progressBar1.Value = 0;

            if (e.Error != null)
            {
                MessageBox.Show("We're sorry, an error occurred while attempting to retrieve Customers' information from the database." +
                    Environment.NewLine + Environment.NewLine +
                    "Please try again, alternatively contact system administrator.",
                    "Data Retrieval", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                CustomerResult res = (e.Result as CustomerResult) ?? new CustomerResult { Customers = new List<Customer>() };
                string serializedCustomers = JsonConvert.SerializeObject(res, Formatting.Indented);

                this.richTextBox1.Text += $"{serializedCustomers} {Environment.NewLine}{Environment.NewLine}";
            }
        }

        void _getCustomersWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;

            PagingInfo pagingInfo = new PagingInfo
            {
                PageNumber = 1,
                PageSize = 25,
            };

            this.progressBar1.Visible = true;
            this._getCustomersWorker.RunWorkerAsync(pagingInfo);
        }

    }
}
