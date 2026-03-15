using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SubjectsManager.CommonComponents
{
    /// <summary>
    /// Запис, який поєднує значення переліку (enum) та його відображуване ім'я.
    /// </summary>
    /// <typeparam name="TEnum">Тип переліку.</typeparam>
    /// <param name="Value">Значення переліку.</param>
    /// <param name="DisplayName">Відображуване ім'я.</param>
    public sealed record EnumWithName<TEnum>(TEnum Value, string DisplayName) where TEnum : struct, Enum;

    /// <summary>
    /// Розширення для роботи з переліками (enum), зокрема для отримання відображуваних імен
    /// з атрибута DisplayAttribute.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Отримує відображуване ім'я для значення переліку, використовуючи атрибут DisplayAttribute.
        /// Якщо атрибут відсутній, повертає звичайне ім'я константи.
        /// </summary>
        /// <param name="value">Значення переліку.</param>
        /// <returns>Відображуване ім'я або назва константи.</returns>
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name is null)
                return value.ToString();

            var field = type.GetField(name);
            var display = field?.GetCustomAttribute<DisplayAttribute>(inherit: false);

            // Повертаємо значення Name з атрибута, якщо воно є, інакше назву константи
            return display?.GetName() ?? name;
        }

        /// <summary>
        /// Створює об'єкт EnumWithName для заданого значення переліку.
        /// </summary>
        /// <typeparam name="TEnum">Тип переліку.</typeparam>
        /// <param name="value">Значення переліку.</param>
        /// <returns>Запис зі значенням та його відображуваним іменем.</returns>
        public static EnumWithName<TEnum> GetEnumWithName<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            return new EnumWithName<TEnum>(value, value.GetDisplayName());
        }

        /// <summary>
        /// Отримує масив усіх значень переліку разом з їх відображуваними іменами.
        /// </summary>
        /// <typeparam name="TEnum">Тип переліку.</typeparam>
        /// <returns>Масив записів EnumWithName для всіх констант переліку.</returns>
        public static EnumWithName<TEnum>[] GetValuesWithNames<TEnum>() where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            var valuesWithNames = new EnumWithName<TEnum>[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                valuesWithNames[i] = values[i].GetEnumWithName();
            }
            return valuesWithNames;
        }
    }
}