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
    public partial class Form0 : Form
    {
        private readonly BackgroundWorker _getCustomersWorker;

        public Form0()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;

            PagingInfo pagingInfo = new PagingInfo
            {
                PageNumber = 1,
                PageSize = 5,
            };

            // this loop simulates some delays in processing
            for (int iter = 0; iter < 100; iter += pagingInfo.PageSize) // just using pageSize to randomize the effec a bit
            {
                Thread.Sleep(700);
            }



            CustomerRepository cr = new CustomerRepository("connection string");
            CustomerResult res = cr.GetCustomers(pagingInfo);

            this.button1.Enabled = true;

            string serializedCustomers = JsonConvert.SerializeObject(res, Formatting.Indented);

            this.richTextBox1.Text += $"{serializedCustomers} {Environment.NewLine}{Environment.NewLine}";
        }

    }
}
