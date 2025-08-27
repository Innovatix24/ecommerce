using Microsoft.FluentUI.AspNetCore.Components;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace BongoEcom.Components;
public class IconProvider
{
    static Dictionary<string, Icon> Icons = new();
    static IconProvider? _iconProvider;
    public static IconProvider Instance
    {
        get
        {
            if (_iconProvider is null)
            {
                _iconProvider = new IconProvider();
            }

            return _iconProvider;
        }
    }
    IconProvider()
    {
        Icons.Clear();

        Icons["Home"] = new Icons.Regular.Size20.Home();
        Icons["Order"] = new Icons.Regular.Size20.ReOrderVertical();
        Icons["Cart"] = new Icons.Regular.Size20.Cart();
        Icons["Settings"] = new Icons.Regular.Size20.Settings();
        Icons["Equation"] = new Icons.Regular.Size20.MathFormatProfessional();
        Icons["BuildingShop"] = new Icons.Regular.Size20.BuildingShop();
        Icons["Airplane"] = new Icons.Regular.Size20.Airplane();
        Icons["BarcodeScanner"] = new Icons.Regular.Size20.BarcodeScanner();
        Icons["Print"] = new Icons.Regular.Size20.Print();
        Icons["Report"] = new Icons.Regular.Size20.Receipt();
    }

    public Icon GetIcon(string name)
    {
        return Icons[name];
    }
}
