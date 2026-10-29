namespace CS.CSValues.Interfaces;

public interface ICSValue<T>
{
	T Value { get; }

	bool SetValue(T newValue);
}
