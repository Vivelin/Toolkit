using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Toolkit
{
    [ValueConversion(typeof(ReadOnlyMemory<byte>), typeof(string))]
    public class ReadOnlyMemoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            var memory = (ReadOnlyMemory<byte>)value;
            var bytes = memory.ToArray();
            return string.Join(' ', bytes.Select(x => x.ToString("X2")));
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
