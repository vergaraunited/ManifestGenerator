using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.Xaml;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace ManifestGenerator
{
    public partial class uctlManifest : UserControl
    {

    //    public static readonly DependencyProperty SelectedManifestProperty =
    //DependencyProperty.Register("SelectedManifest", typeof(isaCommand), typeof(manifestMaster), new PropertyMetadata(null));

        public uctlManifest()
        {
            InitializeComponent();

        }

        //public isaCommand SelectedManifest
        //{
        //    get { return (isaCommand)GetValue(); }
        //    set { SetValue( SelectedManifestProperty)}
        //}
    }
}
