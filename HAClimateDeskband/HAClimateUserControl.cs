﻿using HAClimateDeskband.Models;
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAClimateDeskband
{
    public partial class HAClimateUserControl : UserControl
    {
        private readonly PlotModel PlotModelTemperature = new PlotModel();
        private LineSeries LineSeries { get; set; } = new LineSeries();
        private PlotViewTransparent PlotViewTemperature { get; set; }
        private HttpClient HttpClient { get; set; }
        private HAClimateDeskBandSettings HAClimateDeskBandSettings { get; set; }
        private Label LblMeasurePowerWidth;

        const int WindowsTaskbarSmallIconsSingleRow = 30;
        const int WindowsTaskbarSmallIconsDoubleRow = WindowsTaskbarSmallIconsSingleRow * 2;
        const int WindowsTaskbarBigIconsSingleRow = 40;
        const int WindowsTaskbarBigIconsDoubleRow = WindowsTaskbarBigIconsSingleRow * 2;

        public HAClimateUserControl()
        {
            InitializeComponent();

            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                LblTemperature.MouseWheel += LblTemperature_MouseWheel;

                LineSeries.Color = OxyColors.White;
                LineSeries.StrokeThickness = 1;
                LineSeries.LineStyle = LineStyle.Solid;
                LineSeries.Background = OxyColors.Transparent;

                PlotModelTemperature.Series.Add(LineSeries);
                PlotModelTemperature.IsLegendVisible = false;
                PlotModelTemperature.Padding = new OxyThickness(0);
                PlotModelTemperature.PlotMargins = new OxyThickness(0);

                PlotModelTemperature.Background = OxyColors.Transparent;
                PlotModelTemperature.PlotAreaBackground = OxyColors.Transparent;
                PlotModelTemperature.PlotAreaBorderColor = OxyColors.Transparent;
                PlotModelTemperature.PlotAreaBorderThickness = new OxyThickness(0);
                PlotModelTemperature.Legends.Clear();
                PlotModelTemperature.Axes.Clear();

                // Must be done programmatically, else it will not work with the transparency
                // https://github.com/oxyplot/oxyplot/issues/1670
                PlotViewTemperature = new PlotViewTransparent()
                {
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                    Left = 0,
                    Top = ClientSize.Height - 23,
                    Height = 20,
                    Width = ClientSize.Width - LblTemperature.Width - 4,
                    Model = PlotModelTemperature,
                    BackColor = Color.Transparent,
                    Enabled = false
                };

                //PlotViewTemperature.MouseHover += PlotViewTemperature_MouseHover;

                Controls.Add(PlotViewTemperature);
                PlotViewTemperature.BringToFront();

                LblMeasurePowerWidth = new Label
                {
                    AutoSize = true,
                    Font = LblInfo.Font
                };
                Controls.Add(LblMeasurePowerWidth);

                LoadSettings();

                Timer.Start();
            }
            catch (Exception ex)
            {
                SetErrorState(ex.Message);
            }
        }

        private void PlotViewTemperature_MouseHover(object sender, EventArgs e)
        {
            // Show 'popup' screen with bigger graph, including axis
            // Maybe more info, if possible
            // Show info if some controls are not shown
            MessageBox.Show("TEST");
        }

        public void LoadSettings()
        {
            try
            {
                HAClimateDeskBandSettings = Library.LoadSettings();

                HttpClient = new HttpClient
                {
                    BaseAddress = new Uri(HAClimateDeskBandSettings.ApiBaseUrl)
                };

                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HAClimateDeskBandSettings.ApiKey);

                PlotViewTemperature.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.TemperatureEntityId);
                LblTemperature.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.TemperatureEntityId);
                PictureFire.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId);
                PictureOff.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId);
                PicturePause.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId);

                if (SettingsOK())
                {
                    ResizeControls();
                    UpdateValues();
                }
                else
                {
                    LblInfo.Text = "Click here!";
                }
            }
            catch (Exception ex)
            {
                SetErrorState(ex.Message);
            }
        }

        private void SetTemperature(double temperatureDelta)
        {
            string json = HttpClient.GetStringAsync($"states/{HAClimateDeskBandSettings.ClimateEntityId}").GetAwaiter().GetResult();
            JObject climateGarageState = JObject.Parse(json);

            double currentSetTemperature = climateGarageState.SelectToken(".attributes.temperature").Value<double>();
            double newSetTemperature = currentSetTemperature += temperatureDelta;

            HttpResponseMessage httpResponseMessage = HttpClient.PostAsync($"services/climate/set_temperature", new StringContent($"{{ \"entity_id\": \"{HAClimateDeskBandSettings.ClimateEntityId}\", \"temperature\": {newSetTemperature} }}")).GetAwaiter().GetResult();
            httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private void UpdateValues()
        {
            try
            {
                if (!SettingsOK())
                {
                    return;
                }

                decimal temperature = 0;
                string temperatureUOM = string.Empty;
                decimal setTemperature = 0;
                DateTime lastChanged = DateTime.Now;

                decimal powerUsageToday = 0;
                string powerUsageUOM = string.Empty;

                if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.TemperatureEntityId))
                {
                    string json = HttpClient.GetStringAsync($"states/{HAClimateDeskBandSettings.TemperatureEntityId}").GetAwaiter().GetResult();
                    JObject jObject = JObject.Parse(json);
                    temperatureUOM = jObject.SelectToken(".attributes.unit_of_measurement").Value<string>();

                    string historyJson = HttpClient.GetStringAsync($"history/period/{DateTime.Now.AddHours(-3):O}?minimal_response&filter_entity_id={HAClimateDeskBandSettings.TemperatureEntityId}").GetAwaiter().GetResult();
                    JArray jArray = JArray.Parse(historyJson.Substring(1, historyJson.Length - 2));

                    LineSeries.Points.Clear();

                    foreach (JToken history in jArray)
                    {
                        if (history.Value<string>("state") != "unavailable")
                        {
                            LineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(history.Value<DateTime>("last_changed")), history.Value<double>("state")));
                        }
                    }

                    PlotModelTemperature.InvalidatePlot(true);

                    temperature = Math.Round(jObject.SelectToken(".state").Value<decimal>(), 1);
                    lastChanged = jObject.SelectToken(".last_changed").Value<DateTime>();
                }

                if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.PowerUsageEntityId) && ClientSize.Height >= WindowsTaskbarBigIconsSingleRow || HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage)
                //if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.PowerUsageEntityId) && ClientSize.Height >= WindowsTaskbarBigIconsSingleRow && !HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage)
                {
                    string json = HttpClient.GetStringAsync($"states/{HAClimateDeskBandSettings.PowerUsageEntityId}").GetAwaiter().GetResult();
                    JObject jObject = JObject.Parse(json);

                    powerUsageToday = jObject.SelectToken(".state").Value<decimal>();
                    powerUsageUOM = jObject.SelectToken(".attributes.unit_of_measurement").Value<string>();
                }

                if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId))
                {
                    string json = HttpClient.GetStringAsync($"states/{HAClimateDeskBandSettings.ClimateEntityId}").GetAwaiter().GetResult();
                    JObject jObject = JObject.Parse(json);

                    string hvacState = jObject.SelectToken(".state").Value<string>();
                    string hvacAction = jObject.SelectToken(".attributes.hvac_action").Value<string>();
                    setTemperature = jObject.SelectToken(".attributes.temperature").Value<decimal>();

                    ControlsHelper.SyncBeginInvoke(this, () =>
                    {
                        switch (hvacAction)
                        {
                            case "idle":
                                PicturePause.BringToFront();
                                break;
                            case "off":
                                PictureOff.BringToFront();
                                break;
                            case "heating":
                                PictureFire.BringToFront();
                                break;
                        }
                    });
                }

                ControlsHelper.SyncBeginInvoke(this, () =>
                {
                    List<string> lines = new List<string>();

                    if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.TemperatureEntityId))
                    {
                        lines.Add($"{temperature:G29}{temperatureUOM}");

                        if (ClientSize.Height > WindowsTaskbarBigIconsSingleRow || HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage)
                        {
                            lines.Add($"{lastChanged:HH:mm}");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.PowerUsageEntityId) && ClientSize.Height >= WindowsTaskbarBigIconsSingleRow || HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage)
                    {
                        bool showPowerUOM =
                            ClientSize.Height >= WindowsTaskbarSmallIconsDoubleRow ||
                            (ClientSize.Height > WindowsTaskbarBigIconsSingleRow && HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage);

                        if (!showPowerUOM)
                        {
                            LblMeasurePowerWidth.Text = $"{powerUsageToday:F2} {powerUsageUOM}";
                            showPowerUOM = (LblInfo.Left + LblMeasurePowerWidth.Width - 2) <= PictureFire.Left;
                        }

                        lines.Add($"{powerUsageToday:F2}{(showPowerUOM ? $" {powerUsageUOM}" : string.Empty)}");
                    }

                    LblInfo.Text = string.Join("\r\n", lines);
                    LblInfo.ForeColor = Color.White;

                    ToolTip.ToolTipIcon = ToolTipIcon.Info;
                    ToolTip.ToolTipTitle = "Info";

                    LblTemperature.Text = $"{setTemperature:G29}°";
                    LblTemperature.Left = Width - LblTemperature.Width + 3;
                    PlotViewTemperature.Width = LblTemperature.Left - 2;
                });
            }
            catch (Exception ex)
            {
                SetErrorState(ex.Message);
            }
        }

        private bool SettingsOK()
        {
            if (!Uri.IsWellFormedUriString(HAClimateDeskBandSettings.ApiBaseUrl, UriKind.Absolute))
            {
                SetErrorState("Error, API Base Url is OK.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ApiKey))
            {
                SetErrorState("Error, API Key is OK.");
                return false;
            }

            return true;
        }

        private void SetErrorState(string message)
        {
            ControlsHelper.SyncBeginInvoke(this, () =>
            {
                LblInfo.ForeColor = Color.Red;

                ToolTip.ToolTipIcon = ToolTipIcon.Error;
                ToolTip.SetToolTip(LblInfo, message);
                ToolTip.ToolTipTitle = "Error";
            });
        }

        private void HomeAssistantUserControl_Resize(object sender, EventArgs e)
        {
            ResizeControls();

            UpdateValues();
        }

        public void ResizeControls()
        {
            // It's a mess, because of fonts and positions when the contents of the LblInfo changes..
            if (ClientSize.Height <= WindowsTaskbarSmallIconsSingleRow)
            {
                LblInfo.Top = HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? 0 : -2;

                PictureFire.Top = LblInfo.Top - (HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? -2 : -4);
                PictureOff.Top = LblInfo.Top - (HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? -2 : -4);
                PicturePause.Top = LblInfo.Top - (HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? -2 : -4);
                PlotViewTemperature.Visible = !HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage;
                PlotViewTemperature.Top = ClientSize.Height - 21;
            }
            else if (ClientSize.Height <= WindowsTaskbarBigIconsSingleRow)
            {
                LblInfo.Top = 0;
                PictureFire.Top = LblInfo.Top + 2;
                PictureOff.Top = LblInfo.Top + 2;
                PicturePause.Top = LblInfo.Top + 2;
                PlotViewTemperature.Visible = !HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage;
                PlotViewTemperature.Top = ClientSize.Height - 21;
            }
            else
            {
                LblInfo.Top = 3;
                PictureFire.Top = LblInfo.Top;
                PictureOff.Top = LblInfo.Top;
                PicturePause.Top = LblInfo.Top;
                PlotViewTemperature.Top = ClientSize.Height - 23;
                PlotViewTemperature.Visible = true;
            }

            LblTemperature.Top = PlotViewTemperature.Bottom - 10;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateValues();
        }

        private void Picture_Click(object sender, EventArgs e)
        {
            SetClimate(sender == PictureOff);
            UpdateValues();
        }

        private string SetClimate(bool on)
        {
            string command = on ? "turn_on" : "turn_off";

            HttpResponseMessage httpResponseMessage = HttpClient.PostAsync($"services/climate/{command}", new StringContent($"{{ \"entity_id\": \"{HAClimateDeskBandSettings.ClimateEntityId}\" }}")).GetAwaiter().GetResult();
            return httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private void HomeAssistantUserControl_Load(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                UpdateValues();
            });

            task.GetAwaiter();
        }

        private void LblTemperature_MouseWheel(object sender, MouseEventArgs e)
        {
            ChangeTemperature(increase: e.Delta > 0);
            UpdateValues();
        }

        private void LblTemperature_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeTemperature(increase: e.Button == MouseButtons.Left && !ModifierKeys.HasFlag(Keys.Control));
            UpdateValues();
        }

        private void ChangeTemperature(bool increase)
        {
            if (increase)
            {
                SetTemperature(0.1);
            }
            else
            {
                SetTemperature(-0.1);
            }
        }

        private void LblInfo_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = Application.OpenForms.OfType<FormSettings>().FirstOrDefault() ?? new FormSettings();
            formSettings.HAClimateDeskband = this;
            formSettings.Show();
            formSettings.BringToFront();
        }
    }
}