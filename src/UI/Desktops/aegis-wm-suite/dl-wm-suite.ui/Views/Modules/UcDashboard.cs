using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using dl.wm.models.DTOs.Dashboards;
using dl.wm.presenter.ViewModel.Dashboards;
using dl.wm.presenter.ViewModel.Maps;
using dl.wm.suite.ui.Controls;
using dl.wm.view.Controls.Dashboards;
using dl.wm.view.Controls.Dashboards.Maps;
using DevExpress.Map;
using DevExpress.Utils.Menu;
using DevExpress.XtraMap;
using DevExpress.XtraSplashScreen;

namespace dl.wm.suite.ui.Views.Modules
{
    public partial class UcDashboard : BaseModule, IDashboardManagementView, IMapManagementView
    {
        public override string ModuleCaption => "Dashboard";
        public override bool AllowWaitDialog => true;

        private ColorListLegend Legend => (ColorListLegend)(mpCntrlOpenDashboard.Legends[0]);
        private MapClustererBase _clusterer;
        static string LocationLegendHeader = "Tree location";

        #region Presenters

        private DashboardManagementPresenter _dashboardManagementPresenter;
        private MapManagementPresenter _mapManagementPresenter;

        #endregion

        internal override void InitModule(IDXMenuManager manager, object data)
        {
            IsInitialized = true;
            base.InitModule(manager, data);
        }

        internal override void ShowModule(object item)
        {
            if (!IsInitialized)
                return;
            IsShown = true;
            base.ShowModule(item);

            OnShowModuleLocal();
        }

        private void OnShowModuleLocal()
        {
        }

        internal override void HideModule()
        {
            IsShown = false;
            base.HideModule();
        }

        public UcDashboard()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("el-EL");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("el-EL");
            InitializeComponent();
            InitializePresenters();
        }

        private void InitializePresenters()
        {
            _dashboardManagementPresenter = new DashboardManagementPresenter(this);
            _mapManagementPresenter = new MapManagementPresenter(this);
        }

        private void UcDashboard_Load(object sender, System.EventArgs e)
        {
            OnLoaded();
            _mapManagementPresenter.LoadDashboardGeofence();
        }

        private void OnLoaded()
        {
            // Menemeni Center 40.6562959, 22.9092506
            mpCntrlOpenDashboard.CenterPoint = new GeoPoint(40.6562959, 22.9092506);

            // Create a layer. 
            ImageLayer layerOpen = new ImageLayer();
            
            mpCntrlOpenDashboard.Layers.Add(layerOpen);
            
            // Create a data provider. 
            OpenStreetMapDataProvider providerOpen = new OpenStreetMapDataProvider
            {
                Kind = OpenStreetMapKind.Hot
            };
            layerOpen.DataProvider = providerOpen;
        }


        MapPolygon Poly;

        MapPushpin Pin { set; get; }


        private void TggCmdAddRemoveDumpsterToggled(object sender, EventArgs e)
        {
            _mapManagementPresenter.ToggleAddRemoveDumpsterWasClicked();
        }

        private void BtnPopulatePointsGeofenceClick(object sender, EventArgs e)
        {
            try
            {
                VectorItemsLayer vectorLayerGeofencePoints = mpCntrlOpenDashboard.Layers[2] as VectorItemsLayer;

                foreach (var mapUiModel in Geofence)
                {
                    GeoPoint GP = new GeoPoint()
                    {
                        Latitude = mapUiModel.Latitude,
                        Longitude = mapUiModel.Longitude,
                    };

                    Pin = new MapPushpin
                    {
                        Location = GP
                    };

                    ((MapItemStorage) vectorLayerGeofencePoints.Data).Items.Add(Pin);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPopulateGeofenceClick(object sender, EventArgs e)
        {
            try
            {
                VectorItemsLayer vectorLayerGeofence = mpCntrlOpenDashboard.Layers[1] as VectorItemsLayer;

                var vectorLayerGeofencePoints = ChkPopulateGeofenceOnDemand
                    ? mpCntrlOpenDashboard.Layers[3] as VectorItemsLayer
                    : mpCntrlOpenDashboard.Layers[2] as VectorItemsLayer;

                foreach (var mapUiModel in ChangedGeofence)
                {
                    GeoPoint GP = new GeoPoint()
                    {
                        Latitude = mapUiModel.Latitude,
                        Longitude = mapUiModel.Longitude,
                    };

                    Pin = new MapPushpin
                    {
                        Location = GP
                    };

                    ((MapItemStorage) vectorLayerGeofencePoints.Data).Items.Add(Pin);
                }

                Poly = new MapPolygon
                {
                    Fill = Color.FromArgb(20, 0, 0, 255)
                };

                foreach (MapPushpin mp in ((MapItemStorage)vectorLayerGeofencePoints.Data).Items)
                {
                    Poly.Points.Add(mp.Location);
                }

                ((MapItemStorage)vectorLayerGeofence.Data).Items.Add(Poly);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<MapUiModel> Geofence { get; set; }
        public List<MapUiModel> ChangedGeofence { get; set; } = new List<MapUiModel>();

        public bool NoneGeofencePointWasRetrieved
        {
            set
            {
                if (value)
                {

                }
            }

        }

        public bool CanAddPointToMap { get; set; }

        public bool ToggleCanAddPointToMap
        {
            get => (bool)tggCmdAddRemoveGeofencePoint.EditValue;
            set => tggCmdAddRemoveGeofencePoint.EditValue = value;
        }

        public bool ChkPopulateGeofenceOnDemand
        {
            get => (bool) chckEdtCmdGeofencePopulationFromSelection.EditValue;
            set => chckEdtCmdGeofencePopulationFromSelection.EditValue = value;
        }

        private void MpCntrlOpenDashboardMouseDown(object sender, MouseEventArgs e)
        {
            if (ToggleCanAddPointToMap)
            {
                VectorItemsLayer vectorLayerGeofencePoints = mpCntrlOpenDashboard.Layers[2] as VectorItemsLayer;

                MapPoint pressedPoint = new MapPoint(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);

                CoordPoint p = mpCntrlOpenDashboard.ScreenPointToCoordPoint(pressedPoint);  

                GeoPoint Gp = new GeoPoint()
                {
                    Latitude = p.GetY(),
                    Longitude = p.GetX(),
                };

                Pin = new MapPushpin
                {
                    Location = Gp
                };

                ((MapItemStorage) vectorLayerGeofencePoints.Data).Items.Add(Pin);

                return;
            }

            MapHitInfo info = this.mpCntrlOpenDashboard.CalcHitInfo(e.Location);  
            if (info.InMapPushpin) {  
                MapPushpin pin = (MapPushpin)info.MapPushpin;

                ChangedGeofence.Add(new MapUiModel()
                {
                    Longitude = pin.Location.GetX(),
                    Latitude = pin.Location.GetY(),
                });
            }  
        }

        private void BtnClearGeofenceAndSelectedPointsClick(object sender, EventArgs e)
        {
            VectorItemsLayer vectorLayerOpenGeofence = mpCntrlOpenDashboard.Layers[1] as VectorItemsLayer;
            VectorItemsLayer vectorLayerOpenGeofencePoints = mpCntrlOpenDashboard.Layers[3] as VectorItemsLayer;

            ((MapItemStorage)vectorLayerOpenGeofencePoints.Data).Items.Clear();
            ((MapItemStorage)vectorLayerOpenGeofence.Data).Items.Clear();
        }

        private void BtnStoreGeofenceClick(object sender, EventArgs e)
        {
            _mapManagementPresenter.StoreGeofenceWasClicked();
        }

        private void TggCmdAddRemoveGeofencePointToggled(object sender, EventArgs e)
        {
            _mapManagementPresenter.AddOrNotGeofencepoint();
        }

        private void btnClearPointsGeofence_Click(object sender, EventArgs e)
        {
            VectorItemsLayer vectorLayerOpenPointsGeofence = mpCntrlOpenDashboard.Layers[2] as VectorItemsLayer;
            ((MapItemStorage)vectorLayerOpenPointsGeofence.Data).Items.Clear();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _mapManagementPresenter.LoadDashboardGeofence();
        }


    }
}
