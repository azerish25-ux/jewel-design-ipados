using System;
using System.Diagnostics;

namespace GV.CounterSketch.Shared.Utilities;

public sealed class DeltaHelper
{
	public static void ApplyPatch(string pathToxdelta, string pathToOriginalFile, string pathToPatchFile, string pathToDestinationFile)
	{
		string arg = $"-f -q -d -s \"{pathToOriginalFile}\" \"{pathToPatchFile}\" \"{pathToDestinationFile}\"";
		string arguments = $"/c {pathToxdelta} {arg}";
		Process process = Process.Start(new ProcessStartInfo("cmd.exe", arguments)
		{
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true,
			WindowStyle = ProcessWindowStyle.Hidden
		});
		if (process != null)
		{
			process.WaitForExit();
			process.Close();
		}
	}

	public static void GeneratePatch(string pathToxdelta, string pathToOriginalFile, string pathToChangedFile, string pathToPatchFile)
	{
		GeneratePatch(pathToxdelta, pathToOriginalFile, pathToChangedFile, pathToPatchFile, out var _);
	}

	public static bool GeneratePatch(string pathToxdelta, string pathToOriginalFile, string pathToChangedFile, string pathToPatchFile, out string results)
	{
		string text = $"-9 -f -q -e -Sdjw -s \"{pathToOriginalFile}\" \"{pathToChangedFile}\" \"{pathToPatchFile}\"";
		string text2 = $"\"{pathToxdelta}\"";
		results = $"Executing patch command:\n{text2} {text}\n";
		try
		{
			Process process = Process.Start(new ProcessStartInfo(text2, text)
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			});
			if (process != null)
			{
				process.WaitForExit();
				results += process.StandardOutput.ReadToEnd();
				process.Close();
				return true;
			}
		}
		catch (Exception ex)
		{
			results = results + "Error Generating Patch!\n" + ex;
			return false;
		}
		results += "Error Generating Patch! Process failed to start.";
		return false;
	}
}
