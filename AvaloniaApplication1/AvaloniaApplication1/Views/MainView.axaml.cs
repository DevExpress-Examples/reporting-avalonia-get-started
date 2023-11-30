using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Platform;
using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.UI;
using System.IO;
using System;
using DevExpress.XtraPrinting.Native;

namespace AvaloniaApplication1.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public async void ButtonClick(object sender, RoutedEventArgs e) {
        var topLevel = TopLevel.GetTopLevel(this);
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions {
            Title = "Save a PDF"
        });

        XtraReport report = new XtraReport();
        var reportFile = AssetLoader.Open(new Uri("avares://AvaloniaApplication1/Assets/ReportJson.repx"));



        report.LoadLayoutFromXml(reportFile, true);
        report.ReplaceService<IJsonSourceCustomizationService>(new MyJsonCustomizationService());


        using (MemoryStream ms = new MemoryStream()) {
            await report.ExportToPdfAsync(ms);
            ms.Position = 0;
            if (file != null) {
                await using (var stream = await file.OpenWriteAsync()) {
                    await stream.WriteAsync(ms.ToArray());
                }
            }
        }
    }

    public class MyJsonCustomizationService : IJsonSourceCustomizationService {
        public JsonSourceBase CustomizeJsonSource(JsonDataSource jsonDataSource) {
            return new DevExpress.DataAccess.Json.UriJsonSource(new Uri("https://northwind.netcore.io/customers.json"));
        }
    }
}
