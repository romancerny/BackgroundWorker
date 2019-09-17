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
    public partial class Form2 : Form
    {
        private readonly BackgroundWorker _getCustomersWorker1;
        private readonly BackgroundWorker _getCustomersWorker2;
        private readonly BackgroundWorker _getCustomersWorker3;
        private int _workersRunning = 0;

        public Form2()
        {
            InitializeComponent();

            this._getCustomersWorker1 = new BackgroundWorker();
            this._getCustomersWorker1.WorkerSupportsCancellation = false;
            this._getCustomersWorker1.WorkerReportsProgress = true;
            this._getCustomersWorker1.DoWork += _getCustomersWorker1_DoWork;
            this._getCustomersWorker1.RunWorkerCompleted += _getCustomersWorker1_RunWorkerCompleted;

            this._getCustomersWorker2 = new BackgroundWorker();
            this._getCustomersWorker2.WorkerSupportsCancellation = false;
            this._getCustomersWorker2.WorkerReportsProgress = true;
            this._getCustomersWorker2.DoWork += _getCustomersWorker2_DoWork;
            this._getCustomersWorker2.RunWorkerCompleted += _getCustomersWorker2_RunWorkerCompleted;

            this._getCustomersWorker3 = new BackgroundWorker();
            this._getCustomersWorker3.WorkerSupportsCancellation = false;
            this._getCustomersWorker3.WorkerReportsProgress = true;
            this._getCustomersWorker3.DoWork += _getCustomersWorker3_DoWork;
            this._getCustomersWorker3.RunWorkerCompleted += _getCustomersWorker3_RunWorkerCompleted;

            this.progressBar1.Visible = false;
            this.progressBar1.Style = ProgressBarStyle.Marquee;
            this.progressBar1.MarqueeAnimationSpeed = 20;
            this.progressBar1.Value = 100;
        }

        private void _getCustomersWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            PagingInfo pagingInfo = e.Argument as PagingInfo;

            e.Result = GetCustomers(pagingInfo);
        }

        private void _getCustomersWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            PagingInfo pagingInfo = e.Argument as PagingInfo;

            e.Result = GetCustomers(pagingInfo);
        }

        private void _getCustomersWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            PagingInfo pagingInfo = e.Argument as PagingInfo;

            e.Result = GetCustomers(pagingInfo);
        }

        private void _getCustomersWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool inError = (e.Error != null);

            CustomerResult res = (e.Result as CustomerResult) ?? new CustomerResult { Customers = new List<Customer>() };
            DisplayWorkResult(res, inError);
        }

        private void _getCustomersWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool inError = (e.Error != null);

            CustomerResult res = (e.Result as CustomerResult) ?? new CustomerResult { Customers = new List<Customer>() };
            DisplayWorkResult(res, inError);
        }

        private void _getCustomersWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool inError = (e.Error != null);

            CustomerResult res = (e.Result as CustomerResult) ?? new CustomerResult { Customers = new List<Customer>() };
            DisplayWorkResult(res, inError);
        }

        private CustomerResult GetCustomers(PagingInfo pagingInfo)
        {
            ++_workersRunning;


            // this loop simulates some delays in processing
            // reportProgress is optional, and in case of DB data retrieval we should probably use marquee progress bar
            for (int iter = 0; iter < 100; iter += pagingInfo.PageSize) // just using pageSize to randomize the effec a bit
            {
                Thread.Sleep(700);
                _getCustomersWorker1.ReportProgress(iter);
            }



            CustomerRepository cr = new CustomerRepository("connection string");
            return cr.GetCustomers(pagingInfo);
        }

        private void DisplayWorkResult(CustomerResult res, bool inError)
        {
            --_workersRunning;


            if (_workersRunning == 0)
            {
                this.progressBar1.Visible = false;
            }

            if (inError)
            {
                MessageBox.Show("We're sorry, an error occurred while attempting to retrieve Customers' information from the database." +
                    Environment.NewLine + Environment.NewLine +
                    "Please try again, alternatively contact system administrator.",
                    "Data Retrieval", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string serializedCustomers = JsonConvert.SerializeObject(res, Formatting.Indented);

                this.richTextBox1.Text += $"{serializedCustomers} {Environment.NewLine}{Environment.NewLine}";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;

            PagingInfo pagingInfo = new PagingInfo
            {
                PageNumber = 1,
                PageSize = 5,
            };

            this.progressBar1.Visible = true;
            this._getCustomersWorker1.RunWorkerAsync(pagingInfo);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = false;

            PagingInfo pagingInfo = new PagingInfo
            {
                PageNumber = 1,
                PageSize = 10,
            };

            this.progressBar1.Visible = true;
            this._getCustomersWorker2.RunWorkerAsync(pagingInfo);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = false;

            PagingInfo pagingInfo = new PagingInfo
            {
                PageNumber = 1,
                PageSize = 25,
            };

            this.progressBar1.Visible = true;
            this._getCustomersWorker3.RunWorkerAsync(pagingInfo);
        }

    }
}
