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
using System.Diagnostics;
using DBBench.Postgres;
using DBBench.Oracle;
using DBBench.Mysql;


namespace DBBench
{
    public partial class DBBench : Form
    {
        private const int DEFAULT_NUMREGS = 1000;
        private const int DEFAULT_NUMTHREADS = 1;

        public DBBench()
        {
            InitializeComponent();
        }

        private void btnTestPG_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads==1)
            {
                TestPG1(iRegistries, 1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {
                       
                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestPG1(iRegistries,iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }            
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());

        }

        private void btnTestPG2_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestPG2(iRegistries,1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestPG2(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());

        }



        private void btnOra1_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestOra1(iRegistries,1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestOra1(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());

        }

        private void btnORA2_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestOra2(iRegistries,1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestOra2(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());

        }

        private void btnMy1_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestMy1(iRegistries,1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestMy1(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());



        }

        private void btnMy2_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestMy2(iRegistries,1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestMy2(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());


        }

        private void btnTestPG3_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestPG3(iRegistries, 1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestPG3(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
        }

        private void btnORA3_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestOra3(iRegistries, 1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestOra3(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
        }

        private void btnMy3_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestMy3(iRegistries, 1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestMy3(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());

        }


        private void btnTestPG4_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestPG4(iRegistries, 1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestPG4(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
        }

        private void btnTestOra4_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestOra4(iRegistries, 1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestOra4(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
        }

        private void btnTestMy4_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            int iRegistries = numRegistries();
            int iThreads = numThreads();
            stopwatch.Start();

            if (iThreads == 1)
            {
                TestMy4(iRegistries, 1);
            }
            else
            {
                Task[] taskArray = new Task[iThreads];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(() => {

                        int iThread = Thread.CurrentThread.ManagedThreadId;
                        TestMy4(iRegistries, iThread);

                    });
                }
                Task.WaitAll(taskArray);
            }
            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
        }


        void TestPG1(int numRegistries, int iThread)
        {
            DBBenchPostgresRepository oRep = new DBBenchPostgresRepository(null, false);
            
            for (int i = 0; i < numRegistries; i++)
            {
                oRep.InsertTestTable(new Postgres.PgTestTable()
                {
                    PgStringField = iThread.ToString() + "_" + i.ToString(),
                });
            }
        }


        void TestOra1(int numRegistries, int iThread)
        {
            DBBenchOracleRepository oRep = new DBBenchOracleRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.InsertTestTable(new Oracle.OraTestTable()
                {
                    OraStringField = iThread.ToString() + "_" + i.ToString(),
                });
            }
        }



        void TestMy1(int numRegistries, int iThread)
        {
            DBBenchMysqlRepository oRep = new DBBenchMysqlRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.InsertTestTable(new Mysql.MyTestTable()
                {
                    MyStringField = iThread.ToString() + "_" + i.ToString(),
                });
            }
        }

        void TestPG2(int numRegistries, int iThread)
        {
            DBBenchPostgresRepository oRep = new DBBenchPostgresRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.InsertTestTableADO(iThread.ToString() + "_"+i.ToString());
            }
        }


        void TestOra2(int numRegistries, int iThread)
        {
            DBBenchOracleRepository oRep = new DBBenchOracleRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.InsertTestTableADO(iThread.ToString() + "_"+i.ToString());
            }
        }


        void TestMy2(int numRegistries, int iThread)
        {
            DBBenchMysqlRepository oRep = new DBBenchMysqlRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.InsertTestTableADO(iThread.ToString()+"_"+i.ToString());
            }
        }



        void TestPG3(int numRegistries, int iThread)
        {
            DBBenchPostgresRepository oRep = new DBBenchPostgresRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.Select_Update_TestTable(i, iThread.ToString() + "_" + i.ToString());
            }
        }


        void TestOra3(int numRegistries, int iThread)
        {
            DBBenchOracleRepository oRep = new DBBenchOracleRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.Select_Update_TestTable(i, iThread.ToString() + "_" + i.ToString());
            }
        }



        void TestMy3(int numRegistries, int iThread)
        {
            DBBenchMysqlRepository oRep = new DBBenchMysqlRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                oRep.Select_Update_TestTable(i, iThread.ToString() + "_" + i.ToString());
            }
        }


        void TestPG4(int numRegistries, int iThread)
        {
            DBBenchPostgresRepository oRep = new DBBenchPostgresRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                if (i % 2 == 0)
                {
                    oRep.InsertTestTable(new Postgres.PgTestTable()
                    {
                        PgStringField = iThread.ToString() + "_" + i.ToString(),
                    });
                }
                else
                {
                    oRep.Select_Update_TestTable(i, iThread.ToString() + "_" + i.ToString());
                }
                
            }
        }


        void TestOra4(int numRegistries, int iThread)
        {
            DBBenchOracleRepository oRep = new DBBenchOracleRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                if (i % 2 == 0)
                {
                    oRep.InsertTestTable(new Oracle.OraTestTable()
                    {
                        OraStringField = iThread.ToString() + "_" + i.ToString(),
                    });
                }
                else
                {
                    oRep.Select_Update_TestTable(i, iThread.ToString() + "_" + i.ToString());
                }
            }
        }



        void TestMy4(int numRegistries, int iThread)
        {
            DBBenchMysqlRepository oRep = new DBBenchMysqlRepository(null, false);

            for (int i = 0; i < numRegistries; i++)
            {
                if (i % 2 == 0)
                {
                    oRep.InsertTestTable(new Mysql.MyTestTable()
                    {
                        MyStringField = iThread.ToString() + "_" + i.ToString(),
                    });
                }
                else
                {
                    oRep.Select_Update_TestTable(i, iThread.ToString() + "_" + i.ToString());
                }
            }
        }

        private int numRegistries()
        {
            int iRet = DEFAULT_NUMREGS;
            try
            {
                iRet = Convert.ToInt32(txtNumRegistries.Text);
                if (iRet < 1)
                {
                    iRet = DEFAULT_NUMREGS;
                    txtNumRegistries.Text = iRet.ToString();
                }
            }
            catch
            {
                txtNumRegistries.Text = iRet.ToString();
            }


            return iRet;
        }


        private int numThreads()
        {
            int iRet = DEFAULT_NUMTHREADS;
            try
            {
                iRet = Convert.ToInt32(txtThreads.Text);
                if (iRet < 1)
                {
                    iRet = DEFAULT_NUMTHREADS;
                    txtThreads.Text = iRet.ToString();
                }

            }
            catch
            {
                txtThreads.Text = iRet.ToString();
            }


            return iRet;
        }

     
    }

}
