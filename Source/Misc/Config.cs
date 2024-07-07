using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace squad_dma
{
    public class Config
    {
        #region Json Properties
        [JsonPropertyName("aimviewEnabled")]
        public bool AimviewEnabled { get; set; }

        [JsonPropertyName("defaultZoom")]
        public int DefaultZoom { get; set; }

        [JsonPropertyName("enemyCount")]
        public bool EnemyCount { get; set; }

        [JsonPropertyName("font")]
        public int Font { get; set; }

        [JsonPropertyName("fontSize")]
        public int FontSize { get; set; }

        [JsonPropertyName("paintColors")]
        public Dictionary<string, PaintColor.Colors> PaintColors { get; set; }

        [JsonPropertyName("playerAimLine")]
        public int PlayerAimLineLength { get; set; }

        [JsonPropertyName("showNames")]
        public bool ShowNames { get; set; }

        [JsonPropertyName("showRadarStats")]
        public bool ShowRadarStats { get; set; }

        [JsonPropertyName("uiScale")]
        public int UIScale { get; set; }

        [JsonPropertyName("zoomSensitivity")]
        public int ZoomSensitivity { get; set; }

        [JsonPropertyName("vsync")]
        public bool VSync { get; set; }

        // Adding KMBox properties
        [JsonPropertyName("kmboxIp")]
        public string KmboxIp { get; set; }

        [JsonPropertyName("kmboxPort")]
        public string KmboxPort { get; set; }

        [JsonPropertyName("kmboxMac")]
        public string KmboxMac { get; set; }
        #endregion

        #region Json Ignore
        [JsonIgnore]
        public Dictionary<string, PaintColor.Colors> DefaultPaintColors = new Dictionary<string, PaintColor.Colors>()
        {
            ["Primary"] = new PaintColor.Colors { A = 255, R = 80, G = 80, B = 80 },
            ["PrimaryDark"] = new PaintColor.Colors { A = 255, R = 50, G = 50, B = 50 },
            ["PrimaryLight"] = new PaintColor.Colors { A = 255, R = 130, G = 130, B = 130 },
            ["Accent"] = new PaintColor.Colors { A = 255, R = 255, G = 128, B = 0 }
        };

        [JsonIgnore]
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        [JsonIgnore]
        private static readonly object _lock = new object();

        [JsonIgnore]
        private const string SettingsDirectory = "Configuration\\";
        #endregion

        public Config()
        {
            // Initialize default values for all settings, including KMBox
            AimviewEnabled = false;
            DefaultZoom = 100;
            EnemyCount = false;
            Font = 0;
            FontSize = 13;
            PaintColors = DefaultPaintColors;
            PlayerAimLineLength = 1000;
            ShowNames = false;
            ShowRadarStats = false;
            UIScale = 100;
            ZoomSensitivity = 25;
            VSync = false;
            KmboxIp = "1.1.1.1";
            KmboxPort = "8888";
            KmboxMac = "123456";
        }

        public static bool TryLoadConfig(out Config config)
        {
            lock (_lock)
            {
                if (!Directory.Exists(SettingsDirectory))
                    Directory.CreateDirectory(SettingsDirectory);

                try
                {
                    string filePath = $"{SettingsDirectory}Settings.json";
                    if (!File.Exists(filePath))
                        throw new FileNotFoundException("Settings.json does not exist!");

                    var json = File.ReadAllText(filePath);
                    config = JsonSerializer.Deserialize<Config>(json);
                    return true;
                }
                catch (Exception ex)
                {
                    Program.Log($"TryLoadConfig - {ex.Message}\n{ex.StackTrace}");
                    config = null;
                    return false;
                }
            }
        }

        public static void SaveConfig(Config config)
        {
            lock (_lock)
            {
                if (!Directory.Exists(SettingsDirectory))
                    Directory.CreateDirectory(SettingsDirectory);

                string filePath = $"{SettingsDirectory}Settings.json";
                var json = JsonSerializer.Serialize<Config>(config, _jsonOptions);
                File.WriteAllText(filePath, json);
            }
        }
    }
}