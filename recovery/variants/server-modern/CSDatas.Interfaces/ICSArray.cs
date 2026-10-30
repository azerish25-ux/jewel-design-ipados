using System.Collections.Generic;

namespace CSDatas.Interfaces;

public interface ICSArray
{
	int GetArrayCount();

	CSData GetArrayItem(int index);

	CSDataArray GetFlattenArray();

	List<CSData> GetFlattenedList();
}
