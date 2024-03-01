using System;
using System.Globalization;
using System.Windows.Data;

namespace UNO_Spielprojekt.MultiplayerLobby;

public class IdToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int id && parameter is int targetId)
        {
            return id == targetId ? true : false;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
