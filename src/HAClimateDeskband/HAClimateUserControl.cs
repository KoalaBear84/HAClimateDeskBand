﻿using CSDeskBand;
using HAClimateDeskband.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAClimateDeskband
{
    public partial class HAClimateUserControl : UserControl
    {
        private readonly BackgroundWorker BackgroundWorker = new BackgroundWorker();

        private readonly PlotModel PlotModelTemperature = new PlotModel();
        private LineSeries LineSeries { get; set; } = new LineSeries();
        private PlotViewTransparent PlotViewTemperature { get; set; }
        private HttpClient HttpClient { get; set; }
        private HAClimateDeskBandSettings HAClimateDeskBandSettings { get; set; }
        private readonly Label LblMeasurePowerWidth;
        private readonly bool Initialized;
        private readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings { Culture = new CultureInfo("en-US") };

        static readonly int TaskbarHorizontalHeightSmallDoubleRow = CSDeskBandOptions.TaskbarHorizontalHeightSmall * 2;

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

                LblMeasurePowerWidth = new Label
                {
                    AutoSize = true,
                    Font = LblInfo.Font
                };
                Controls.Add(LblMeasurePowerWidth);

                BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
                BackgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                SetErrorState($"Error loading control: {ex.Message}");
            }

            Initialized = true;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                UpdateValues();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }

        public void LoadSettings()
        {
            try
            {
                HAClimateDeskBandSettings = Library.LoadSettings();

                bool settingsOK = SettingsOK();

                ControlsHelper.SyncBeginInvoke(this, () =>
                {
                    PlotViewTemperature.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.TemperatureEntityId) && settingsOK;
                    LblTemperature.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.TemperatureEntityId) && settingsOK;
                    PictureFire.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId) && settingsOK;
                    PictureOff.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId) && settingsOK;
                    PicturePause.Visible = !string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId) && settingsOK;
                    PictureHA.Visible = !settingsOK;
                });

                ResizeControls();

                if (settingsOK)
                {
                    HttpClient = new HttpClient
                    {
                        BaseAddress = new Uri(HAClimateDeskBandSettings.ApiBaseUrl)
                    };

                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HAClimateDeskBandSettings.ApiKey);

                    UpdateValues();
                }
                else
                {
                    ControlsHelper.SyncBeginInvoke(this, () =>
                    {
                        LblInfo.Text = string.Empty;
                    });
                }
            }
            catch (Exception ex)
            {
                SetErrorState($"Error loading settings: {ex.Message}");
            }
        }

        private void OpenSettings()
        {
            FormSettings formSettings = Application.OpenForms.OfType<FormSettings>().FirstOrDefault() ?? new FormSettings();
            formSettings.HAClimateDeskband = this;
            formSettings.Show();
            formSettings.BringToFront();
        }

        private bool SettingsOK()
        {
            if (!Initialized)
            {
                return false;
            }

            try
            {
                if (HAClimateDeskBandSettings == null)
                {
                    SetErrorState("Empty settings");
                    return false;
                }
                else
                {
                    SetErrorState(null);
                }

                if (!Uri.IsWellFormedUriString(HAClimateDeskBandSettings.ApiBaseUrl, UriKind.Absolute))
                {
                    SetErrorState("Error settings: API Base Url is NOT OK.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ApiKey))
                {
                    SetErrorState("Error settings: API Key is NOT OK.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SettingsOK: {ex.Message}");
                return false;
            }
        }

        private void SetTemperature(bool increase)
        {
            if (!SettingsOK())
            {
                return;
            }

            try
            {
                string json = HttpClient.GetStringAsync($"states/{HAClimateDeskBandSettings.ClimateEntityId}").GetAwaiter().GetResult();
                JObject climateGarageState = JObject.Parse(json);

                decimal currentSetTemperature = climateGarageState.SelectToken(".attributes.temperature").Value<decimal>();
                decimal targetTempStep = climateGarageState.SelectToken(".attributes.target_temp_step")?.Value<decimal>() ?? 0.1m;

                decimal newSetTemperature = currentSetTemperature += increase ? targetTempStep : targetTempStep * -1;

                HttpResponseMessage httpResponseMessage = HttpClient.PostAsync($"services/climate/set_temperature", new StringContent(JsonConvert.SerializeObject(new SetTemperatureModel
                {
                    ClimateEntityId = HAClimateDeskBandSettings.ClimateEntityId,
                    Temperature = newSetTemperature
                }, JsonSerializerSettings))).GetAwaiter().GetResult();

                httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SetErrorState($"Error setting temperature: {ex.Message}");
            }
        }

        private void ChangeTemperature(bool increase)
        {
            SetTemperature(increase);
        }

        private void UpdateValues()
        {
            if (!SettingsOK() || HttpClient == null)
            {
                return;
            }

            try
            {
                decimal? currentTemperature = null;
                string temperatureUOM = string.Empty;
                decimal setTemperature = 0;
                DateTime lastChanged = DateTime.Now;

                decimal powerUsageToday = 0;
                string powerUsageUOM = string.Empty;

                if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.PowerUsageEntityId) && ClientSize.Height >= CSDeskBandOptions.TaskbarHorizontalHeightLarge || HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage)
                {
                    string json = HttpClient.GetStringAsync($"states/{HAClimateDeskBandSettings.PowerUsageEntityId}").GetAwaiter().GetResult();
                    JObject jObject = JObject.Parse(json);

                    if (jObject.Value<string>("state") != "unavailable" && jObject.Value<string>("state") != "unknown")
                    {
                        powerUsageToday = jObject.Value<decimal>("state");
                    }
                    powerUsageUOM = jObject.SelectToken(".attributes.unit_of_measurement").Value<string>();
                }

                if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.ClimateEntityId))
                {
                    string json = HttpClient.GetStringAsync($"states/{HAClimateDeskBandSettings.ClimateEntityId}").GetAwaiter().GetResult();
                    JObject jObject = JObject.Parse(json);

                    string climateState = jObject.SelectToken(".state")?.Value<string>();
                    string hvacAction = jObject.SelectToken(".attributes.hvac_action")?.Value<string>();
                    setTemperature = jObject.SelectToken(".attributes.temperature").Value<decimal>();
                    currentTemperature = jObject.SelectToken(".attributes.current_temperature")?.Value<decimal>();

                    ControlsHelper.SyncBeginInvoke(this, () =>
                    {
                        if (hvacAction != null)
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
                        }
                        else
                        {
                            if (climateState == "off")
                            {
                                PictureOff.BringToFront();
                            }
                            else if (currentTemperature < setTemperature)
                            {
                                PictureFire.BringToFront();
                            }
                            else
                            {
                                PicturePause.BringToFront();
                            }
                        }
                    });
                }

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
                        if (history.Value<string>("state") != "unavailable" && history.Value<string>("state") != "unknown")
                        {
                            LineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(history.Value<DateTime>("last_changed")), history.Value<double>("state")));
                        }
                    }

                    PlotModelTemperature.InvalidatePlot(true);

                    if (jObject.SelectToken(".state").Value<string>() != "unavailable" && jObject.SelectToken(".state").Value<string>() != "unknown")
                    {
                        currentTemperature = Math.Round(jObject.SelectToken(".state").Value<decimal>(), 1);
                    }

                    lastChanged = jObject.SelectToken(".last_changed").Value<DateTime>();
                }

                ControlsHelper.SyncBeginInvoke(this, () =>
                {
                    List<string> lines = new List<string>();

                    if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.TemperatureEntityId))
                    {
                        if (currentTemperature.HasValue)
                        {
                            lines.Add($"{currentTemperature:G29}{temperatureUOM}");
                        }
                        else
                        {
                            lines.Add($"? {temperatureUOM}");
                        }

                        if (ClientSize.Height > CSDeskBandOptions.TaskbarHorizontalHeightLarge || HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage)
                        {
                            lines.Add($"{lastChanged:HH:mm}");
                        }
                    }
                    else if (currentTemperature.HasValue)
                    {
                        lines.Add($"{currentTemperature:G29}{temperatureUOM}");
                    }

                    if (!string.IsNullOrWhiteSpace(HAClimateDeskBandSettings.PowerUsageEntityId) && ClientSize.Height >= CSDeskBandOptions.TaskbarHorizontalHeightLarge || HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage)
                    {
                        bool showPowerUOM =
                            ClientSize.Height >= TaskbarHorizontalHeightSmallDoubleRow ||
                            (ClientSize.Height > CSDeskBandOptions.TaskbarHorizontalHeightLarge && HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage);

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
                SetErrorState($"Error retrieving/updating values:{Environment.NewLine}{GetExceptionWithInner(ex)}");
            }
        }

        private static string GetExceptionWithInner(Exception ex)
        {
            string errorMessage = ex.Message;
            Exception exInner = ex;

            while (exInner.InnerException != null)
            {
                errorMessage += $"{Environment.NewLine} -> {exInner.InnerException.Message}";
                exInner = exInner.InnerException;
            }

            return errorMessage;
        }

        private void SetErrorState(string message)
        {
            try
            {
                ControlsHelper.SyncBeginInvoke(this, () =>
                {
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        LblInfo.ForeColor = Color.Red;

                        ToolTip.ToolTipIcon = ToolTipIcon.Error;
                        ToolTip.SetToolTip(LblInfo, message);
                        ToolTip.ToolTipTitle = "Error";
                    }
                    else
                    {
                        LblInfo.ForeColor = Color.White;

                        ToolTip.ToolTipIcon = ToolTipIcon.None;
                        ToolTip.SetToolTip(LblInfo, message);
                        ToolTip.ToolTipTitle = string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SetErrorState: {ex.Message}");
            }
        }

        public void ResizeControls()
        {
            if (!Initialized)
            {
                return;
            }

            try
            {
                ControlsHelper.SyncBeginInvoke(this, () =>
                {
                    // It's a mess, because of fonts and positions when the contents of the LblInfo changes..
                    if (ClientSize.Height <= CSDeskBandOptions.TaskbarHorizontalHeightSmall)
                    {
                        LblInfo.Top = HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? 0 : -2;

                        PictureFire.Top = LblInfo.Top - (HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? -2 : -4);
                        PictureOff.Top = LblInfo.Top - (HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? -2 : -4);
                        PicturePause.Top = LblInfo.Top - (HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage ? -2 : -4);
                        PlotViewTemperature.Visible = !HAClimateDeskBandSettings.PreferLastChangeAndPowerUsage;
                        PlotViewTemperature.Top = ClientSize.Height - 21;
                    }
                    else if (ClientSize.Height <= CSDeskBandOptions.TaskbarHorizontalHeightLarge)
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
                    PictureHA.Left = (ClientSize.Width / 2) - PictureHA.Width / 2;
                    PictureHA.Top = (ClientSize.Height / 2) - PictureHA.Height / 2;
                });
            }
            catch (Exception ex)
            {
                SetErrorState($"Error in ResizeControls: {ex.Message}");
            }
        }

        private void SetClimate(bool on)
        {
            if (!SettingsOK())
            {
                return;
            }

            try
            {
                string command = on ? "turn_on" : "turn_off";

                HttpResponseMessage httpResponseMessage = HttpClient.PostAsync($"services/climate/{command}", new StringContent(JsonConvert.SerializeObject(new EntityModel
                {
                    ClimateEntityId = HAClimateDeskBandSettings.ClimateEntityId
                }, JsonSerializerSettings))).GetAwaiter().GetResult();
                httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SetErrorState($"Error setting climate state to {(on ? "On" : "Off")}: {ex.Message}");
            }
        }

        #region Form Event Handlers
        private void HomeAssistantUserControl_Resize(object sender, EventArgs e)
        {
            ResizeControls();

            UpdateValues();
        }

        private void Picture_Click(object sender, EventArgs e)
        {
            SetClimate(sender == PictureOff);
            UpdateValues();
        }

        private void HomeAssistantUserControl_Load(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                LoadSettings();
                ResizeControls();
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

        private void LblInfo_Click(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void PictureHA_Click(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void PlotViewTemperature_MouseHover(object sender, EventArgs e)
        {
            // Show 'popup' screen with bigger graph, including axis
            // Maybe more info, if possible
            // Show info if some controls are not shown
            MessageBox.Show("TEST");
        }
        #endregion
    }
}