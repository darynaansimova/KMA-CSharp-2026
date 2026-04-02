using System;
using System.Globalization;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.Tools.Converters
{
    /// <summary>
    /// Конвертер для перетворення значення переліку (enum) у його відображуване ім'я,
    /// використовуючи атрибут DisplayAttribute.
    /// Реалізує інтерфейс IValueConverter для використання у прив'язках XAML.
    /// </summary>
    public class EnumToDisplayNameConverter : IValueConverter
    {
        /// <summary>
        /// Конвертує значення enum у рядок для відображення.
        /// Якщо значення null, повертає порожній рядок.
        /// Якщо значення не є enum, повертає його ToString().
        /// Інакше повертає результат методу GetDisplayName() з розширення для enum.
        /// </summary>
        /// <param name="value">Значення для конвертації (очікується enum).</param>
        /// <param name="targetType">Тип, до якого потрібно конвертувати (не використовується).</param>
        /// <param name="parameter">Додатковий параметр (не використовується).</param>
        /// <param name="culture">Культура для конвертації (не використовується).</param>
        /// <returns>Відображуване ім'я enum або порожній рядок.</returns>
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            if (value is not Enum castedEnum)
                return value.ToString() ?? string.Empty;

            // Використовуємо розширення для отримання відображуваного імені
            return castedEnum.GetDisplayName();
        }

        /// <summary>
        /// Зворотна конвертація не підтримується, оскільки це конвертер лише для читання.
        /// </summary>
        /// <exception cref="NotSupportedException">Завжди викидається.</exception>
        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}