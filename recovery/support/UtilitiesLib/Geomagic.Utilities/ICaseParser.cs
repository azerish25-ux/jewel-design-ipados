namespace Geomagic.Utilities;

public interface ICaseParser<T>
{
	bool Read(string FilePath, out T Obj);

	bool Write(string Dest, T Obj);
}
