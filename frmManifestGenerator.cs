using MobileDeliveryMVVM.Models;
using MobileDeliveryMVVM.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UMDGeneral.Data;
using static UMDGeneral.Definitions.MsgTypes;
using System.Collections.Generic;
using MobileDeliveryLogger;
using UMDGeneral.BaseClasses;
using UMDGeneral.Settings;

namespace ManifestGenerator
{
    public partial class frmManifestGenerator : Form
    {
        DataTable table = new DataTable();
        Logger logger;
        ManifestVM mvm;
        UMDAppConfig config;

        public frmManifestGenerator(UMDAppConfig config, Logger log)
        {
            logger = log;
            Init(config);
            InitializeComponent();
            // ViewModel Instance
            mvm = new ManifestVM(config);
            
            //dvm = new DriverVM();
            //this.dataGridView2.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView2_DataBindingComplete);
        }

        public void Init(UMDAppConfig config)
        {
            this.config = config;
        }
        void InitDGView()
        {
            //dvm.OnDriversLoad(dte);
           // Monitor.Enter(waitForLoad);
            dataGridView2.AutoGenerateColumns = false;

            
            DataGridViewComboBoxColumn cmbcolName = new DataGridViewComboBoxColumn();
            //cmbcolName.DataPropertyName = "Userid";
            //cmbcolName.HeaderText = "Driver";
            //cmbcolName.Name = cmbcolName.HeaderText;
            ////var DriverVM = dvm;
            ////cmbcolName.DataSource = GetDrivers();
            //cmbcolName.Visible = true;
            //dataGridView2.Columns.Add(cmbcolName);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "TRK_CDE";
            colName.HeaderText = "Truck Code";
            colName.Name = colName.HeaderText;
            dataGridView2.Columns.Add(colName);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "COUNT";
            colName.HeaderText = "Count";
            colName.Name = colName.HeaderText;
            colName.Visible = false;
            dataGridView2.Columns.Add(colName);

            DataGridViewComboBoxColumn cmbcolCmd = new DataGridViewComboBoxColumn();
            cmbcolCmd.DataPropertyName = "COMMAND";
            cmbcolCmd.HeaderText = "Command";
            cmbcolCmd.Name = cmbcolName.HeaderText;
            cmbcolCmd.DataSource = Enum.GetValues(typeof(eCommand));
            cmbcolCmd.ValueType = typeof(eCommand);
            cmbcolCmd.Visible = false;
            dataGridView2.Columns.Add(cmbcolCmd);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "Desc";
            colName.HeaderText = "Route";
            colName.Name = colName.HeaderText;
            dataGridView2.Columns.Add(colName);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "NOTES";
            colName.HeaderText = "Notes";
            colName.Name = colName.HeaderText;
            dataGridView2.Columns.Add(colName);

            DataGridViewCheckBoxColumn chcolName = new DataGridViewCheckBoxColumn();
            chcolName.DataPropertyName = "IsSelected";
            chcolName.HeaderText = "Release";
            chcolName.Name = chcolName.HeaderText;
            dataGridView2.Columns.Add(chcolName);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "LINK";
            colName.HeaderText = "Link";
            colName.Name = colName.HeaderText;
            colName.Visible = true;
            dataGridView2.Columns.Add(colName);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "Manifestid";
            colName.HeaderText = "Manifest Id";
            colName.Name = colName.DataPropertyName;
            colName.Visible = true;
            dataGridView2.Columns.Add(colName);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "SEAL_DTE";
            colName.HeaderText = "Seal Date";
            colName.Name = colName.HeaderText;
            colName.Visible = false;
            dataGridView2.Columns.Add(colName);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "SHIP_DTE";
            colName.HeaderText = "Ship Date";
            colName.Visible = false;
            colName.Name = colName.HeaderText;
            dataGridView2.Columns.Add(colName);

            colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "SHP_QTY";
            colName.HeaderText = "Ship Qty";
            colName.Visible = false;
            colName.Name = colName.HeaderText;
            dataGridView2.Columns.Add(colName);

            chcolName = new DataGridViewCheckBoxColumn();
            chcolName.DataPropertyName = "TRUCKISCLOSED";
            chcolName.HeaderText = "Truck Closed";
            chcolName.Name = chcolName.HeaderText;
            chcolName.Visible = false;

            dataGridView2.Columns.Add(chcolName);
        }
        void LoadManifest() {
            Logger.Info($"Load Manifest - Manifest Generator");
            mvm.LoadData(
                new manifestRequest()
                {
                    command = eCommand.GenerateManifest,
                    date = dateTimePicker1.Value.ToString("yyyy-MM-dd")
                });
        }
        Task GetManifest()
        {
            LoadManifest();

            return Task.CompletedTask;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 7) // column of the enum
                {
                    try
                    {
                        e.Value = DateTime.Today;
                    }
                    catch (Exception ex)
                    {
                        e.Value = ex.Message;
                    }
                }
                else if (e.ColumnIndex == 8) // column of the enum
                {
                    try
                    {
                        e.Value = "";
                    }
                    catch (Exception ex)
                    {
                        e.Value = ex.Message;
                    }
                }
            }
            catch (Exception ex) { }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "Release" && dataGridView2.CurrentCell is DataGridViewCheckBoxCell)
                {
                    bool isChecked = (bool)dataGridView2[e.ColumnIndex, e.RowIndex].EditedFormattedValue;
                    if (isChecked == false)
                    {
                        dataGridView2.BeginEdit(true);
                        dataGridView2.Rows[e.RowIndex].Selected = true;
                        dataGridView2.EndEdit();
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void dataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell is DataGridViewCheckBoxCell)
            {
                dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dataGridView2.Invalidate();
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit); 
        }

        private void dataGridView2_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //if (e.Row.DataBoundItem != null)
            //{
            //    e.Row.Cells["Command"].Value = (eCommand)((int)e.Row.Cells["Command"].Value);
            //}
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }
        void bindToMVVMObservable() { }
        void observableToBindingList(ObservableCollection<ManifestMasterData> mmd) { }

        private int PopulateGrid(string val="")
        {
            try
            {
                var mmc = dataGridView2.DataSource as WinformObservableCollection<ManifestMasterData>;

                if (ManifestIds != null && ManifestIds.DataSource != null)
                {
                    BindingList<String> lstmmid = new BindingList<String>();
                   // lstmmid.Add(new Binding("DataSource", mvm, "ManifestMaster"));
                    
                    var mmid = ManifestIds.DataSource as ObservableCollection<String>;
                    if (mmid != null)
                    {
                        foreach (var it in mmid)
                            lstmmid.Add(it);
                        ManifestIds.DataSource = lstmmid;
                    }
                }
                if (mmc == null)
                    return 0;

                List<ManifestMasterData> lstMmd;

                if (val.Length > 0)
                    lstMmd = mmc.Where(s => val.CompareTo(s.LINK.ToString()) == 0 || val.CompareTo(s.ManifestId.ToString()) == 0).ToList();
                else
                    lstMmd = mmc.Select(x => x).ToList();

                if (lstMmd == null || lstMmd.Count == 0)
                    return 0;

                BindingList<ManifestMasterData> lstmm = new BindingList<ManifestMasterData>();
                foreach (var it in mmc)
                {
                    if (it.LINK == lstMmd[0].LINK)
                        lstmm.Add(lstMmd[0]);
                    else
                        lstmm.Add(new ManifestMasterData(it));
                }
                //if (lstmm != null && lstmm.Count>0)
                //{
                dataGridView2.DataSource = lstmm;
                return lstmm.Count;
                //}
                //else
                //    return mmc.Count;
                //return 0;
            }
            catch (Exception ex)
            { }
            return 0;
        }
        void ClearGrid() {
            //dataGridView2.Rows.Clear();  
            dataGridView2.Refresh();
        }
        //BindingList<String> GetDrivers()
        //{
        //    //BindingList<String> Drivers = new BindingList<string>();

        //    //foreach (var it in dvm.DriverData)
        //    //    Drivers.Add(it.LastName + ", " + it.FirstName);
        //    //return Drivers;

        //}
        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
        private void LoadForm()
        {
            
            // Generic MVVM Bindings
            InitDGView();

            // ObservableCollection <ManifestMastyerData> Bindings
            var bs = new Binding("DataSource", mvm, "ManifestMaster");
            bs.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            dataGridView2.DataBindings.Add(bs);

            // iCommand Bindings
            //BindingList bl = 
            textBox1.DataBindings.Add(new Binding("Text", mvm, "ROUTECOUNT"));
            ManifestIds.DataBindings.Add(new Binding("Text", mvm, "UploadManifestIdComplete"));
            //button1.DataBindings.Add(new Binding("Visible", mvm, "LoadManifestIdComplete"));
            // textBox1.DataBindings.Add(new Binding("Text", mvm, "ROUTECOUNT"));
            txtManifestId.DataBindings.Add(new Binding("Text", mvm, "LoadManifestRequestComplete"));
            //driverButton.DataBindings.Add(new Binding("Visible", dvm, "LoadDriversComplete"));
            dateTimePicker1.ValueChanged += (s, e) => mvm.LoadManifestCommand.Execute(dateTimePicker1.Value);
            //dateTimePicker1.ValueChanged += (s, e) => dvm.LoadDriversCommand.Execute(dateTimePicker1.Value);

            //ManifestIds.DataBindings.CollectionChanged += ManifestId_DataBindings_CollectionChanged;
            //ManifestIds.DataBindings.CollectionChanging += ManifestId_DataBindings_CollectionChanging;


            //this.dataGridView2.DataSource = table;

        }

        //ManualResetEvent[] LoadCompleteEvents =
        //    new ManualResetEvent[2];

        //WaitHandle[] waitForLoad = new WaitHandle[2];
        private void driverButton_VisibleChanged(object sender, EventArgs e)
        {
            //wait in both driver and manifest to come back then populate the grid with the cmbDrivers
            //if (driverButton.Visible)
            //{
            //    LoadCompleteEvents[0].Set();
            //}
            //else
            //{
            //    //LoadCompleteEvents[0] = new ManualResetEvent(false);
                
            //}
        }

        private void button1_VisibleChanged(object sender, EventArgs e)
        {
            //if (button1.Visible)
            //{
            //    if (PopulateGrid() == 0)
            //       button1.Visible = false;
            //}
            //else
            //{
            //   // ClearGrid();
            //}
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            PopulateGrid();
        }
        static bool bInit = true;
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtURL.Text.Length > 5)
                mvm.InitConnections(txtURL.Text, 81, txtURL.Text, 8181);
            else
                mvm.InitConnections(config.srvSet.url, config.srvSet.port,
                    config.srvSet.WinSysUrl, config.srvSet.WinSysPort);
            //dvm.InitConnections(txtURL.Text, "81", txtURL.Text, "8181");
            //dvm.LoadDriversCommand.Execute(dateTimePicker1.Value);

           // ManifestIds.DataBindings.Clear();
            //var bslist = new BindingList();
            //var bs = new Binding("DataSource", mvm, "LoadManifestIdComplete", false, DataSourceUpdateMode.OnPropertyChanged);

           // ManifestIds.DataBindings.Add(bs);

            mvm.LoadManifestCommand.Execute(dateTimePicker1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var mmm = dataGridView2.DataSource as BindingList<ManifestMasterModel>;

                int idx = dataGridView2.Columns["Release"].Index;

                foreach (var it in dataGridView2.Rows)
                {
                    DataGridViewCheckBoxCell chk = ((DataGridViewRow)it).Cells[idx] as DataGridViewCheckBoxCell;

                    if (Convert.ToBoolean(chk.Value))
                    {
                        var mm = (ManifestMasterData)((DataGridViewRow)it).DataBoundItem;
                        mvm.ReleaseManifest(mm);
                    }
                }
            } catch (Exception ex){ }
        }

        private void cmbURL_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtURL.Text = cmbURL.GetItemText(this.cmbURL.SelectedItem);
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0)
                {
                    DataGridView dgv = sender as DataGridView;
                    if (dgv.Rows.Count < e.RowIndex)
                        return;
                    ManifestMasterData data = dgv.Rows[e.RowIndex].DataBoundItem as ManifestMasterData;

                    switch (data.Command)
                    {
                        case eCommand.ManifestLoadComplete:
                            e.CellStyle.BackColor = Color.Green;
                            break;
                        case eCommand.Manifest:
                            e.CellStyle.BackColor = Color.Blue;
                            break;
                        case eCommand.GenerateManifest:
                            e.CellStyle.BackColor = Color.Red;
                            break;
                    }
                }
            }
            catch (Exception ex) { Logger.Debug($"Manifest Generator Error dataGridView2_CellFormatting Exce= {ex.Message}"); }
        }

        private void txtManifestId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Logger.Debug("Manfest Generator");
                if (txtManifestId.Text.Length != 0)// && txtManifestId.Text != 0)
                {
                    button1.Visible = true;
                    //PopulateGrid(((Control)sender).Text);
                }
                else
                {
                    button1.Visible = false;
                    ClearGrid();
                }
            }
            catch (Exception ex) { Logger.Debug($"Manifest Generator Error txtManifestId Exce= {ex.Message}"); }
        }

        private void ManifestIds_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = ManifestIds.Rows[e.RowIndex].DataBoundItem.ToString();
        }

        private void ManifestIds_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Logger.Info($"Manifest Generator Row Added");
            button1.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (bInit)
                LoadForm();
            bInit = false;
        }
    }
}
