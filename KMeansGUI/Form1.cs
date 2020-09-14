using KMeansProject;
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

namespace KMeansGUI
{
    public partial class Form1 : Form
    {
        private KMeans objKMeans;     
        private List<double[]> dataSetList;
        private BackgroundWorker objBackgroundWorker;
        KMeansEventArgs kmeansEA;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            Random objRandom = new Random();
            dataSetList = new List<double[]>();
            for(int i=0;i<(int)numericUpDown1.Value;i++)
            {
                double[] point = new double[2];
                for(int j = 0; j < 2; j++)
                {
                    point[j] = Misc.GenerateRandomDouble(objRandom, 20, 600);
                }
                dataSetList.Add(point);
            }

            
           

            objKMeans = new KMeans(2, new EuclideanDistance());

            picImage.Invalidate();

            objBackgroundWorker = new BackgroundWorker();
            objBackgroundWorker.WorkerReportsProgress = true;
            objBackgroundWorker.DoWork += ObjBackgroundWorker_DoWork;
            objBackgroundWorker.RunWorkerCompleted += ObjBackgroundWorker_RunWorkerCompleted;
            objBackgroundWorker.ProgressChanged += ObjBackgroundWorker_ProgressChanged;

            objBackgroundWorker.RunWorkerAsync(dataSetList.ToArray());
        }

        private void ObjBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Console.WriteLine(">>> Progress Changed");
            kmeansEA = e.UserState as KMeansEventArgs;
            if (kmeansEA != null)
            {
                foreach (Centroid centroid in kmeansEA.CentroidList)
                {
                    System.Console.WriteLine("Centroid: " + centroid.ToString());
                    picImage.Invalidate();
                    
                }
            }
        }
        
        private void ObjBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Centroid[] result = e.Result as Centroid[];
            Console.WriteLine("Sınıflandırma Yapıldı");
            button1.Enabled = true;
        }

        private void ObjBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            double[][] inputDataset = e.Argument as double[][];
            objKMeans.UpdateProgress +=(x,y)=> {
                objBackgroundWorker.ReportProgress(0,y); };
            e.Result = objKMeans.Run(inputDataset);
        }

        private void picImage_Paint(object sender, PaintEventArgs e)
        {
            if (kmeansEA == null || kmeansEA.CentroidList == null) return;

            foreach(Centroid centroid in kmeansEA.CentroidList)
                centroid.DrawMe(e);

            if (kmeansEA.Dataset == null) return;

            Graphics g = e.Graphics;
            foreach(double[] point in kmeansEA.Dataset)
            {
                g.DrawRectangle(new Pen(Color.Gray, 2.0f), (float)point[0], (float)point[1], 10, 10);
            }
        }
    }
}
