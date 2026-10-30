namespace GV.CounterSketch.Shared.DataAccess;

public sealed class CSGDataAccess : DataAccess
{
	private const string _connString = "REDACTED_EMBEDDED_CREDENTIAL";

	public CSGDataAccess()
		: base("REDACTED_EMBEDDED_CREDENTIAL")
	{
	}

	public void AddGemToDesign(int designVersionComponentId, string name, string shape, double gemX, double gemXMax, double gemXMin, double gemY, double gemYMax, double gemYMin, double gemZ, double fFactor, double gemW, double gemWMax, double gemWMin, string mounting, int flags, int gemPlacement, double mountingVolume, string graphId = "")
	{
		string query = $"INSERT INTO DesignVersionComponentGems(DesignVersionComponentId, Name, Shape, GemX, GemXMax, GemXMin, GemY, GemYMax, GemYMin, GemZ, FFactor, GemW, GemWMax, GemWMin, Mounting, Flags, GemPlacement, MountingVolume, GraphId, Mounting2) values ({designVersionComponentId}, '{name}', '{shape}', {gemX}, {gemXMax}, {gemXMin}, {gemY}, {gemYMax}, {gemYMin}, {gemZ}, {fFactor}, {gemW}, {gemWMax}, {gemWMin}, '{mounting}', {flags}, {gemPlacement}, {mountingVolume}, '{graphId}')";
		ExecuteNonQuery(query);
	}

	public void DeleteGemsFromDesignVersion(int designVersionId)
	{
		ExecuteNonQuery("DELETE FROM DesignVersionComponentGems WHERE DesignVersionComponentId IN (SELECT DesignVersionComponentId FROM DesignVersionComponents WHERE DesignVersionId=@designVersionId)", "designVersionId", designVersionId);
	}

	public int GetDesignVersionComponentId(int designVersionId, int component)
	{
		return ExecuteScalarInt("SELECT DesignVersionComponentId FROM DesignVersionComponents WHERE DesignVersionId = @designVersionId AND ComponentId = @componentId", "designVersionId", designVersionId, "componentId", component);
	}
}
