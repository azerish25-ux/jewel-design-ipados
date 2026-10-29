namespace GV.CounterSketch.Studio.Controls.BLL;

public class CSUIAttribute
{
	public int Id { get; }

	public string Name { get; }

	public double X { get; set; }

	public double Y { get; set; }

	public bool IsSelected { get; set; }

	public CSUIAttribute(int id, string name)
	{
		Id = id;
		Name = name;
		X = 0.0;
		Y = 0.0;
	}

	public CSUIAttribute(int id, string name, double x)
	{
		Id = id;
		Name = name;
		X = x;
		Y = 0.0;
	}

	public CSUIAttribute(int id, string name, double x, double y)
	{
		Id = id;
		Name = name;
		X = x;
		Y = y;
	}
}
