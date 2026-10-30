using System.Runtime.InteropServices;
using System.Text;
public static class MsiAudit
{
    [DllImport("msi.dll", CharSet = CharSet.Unicode)] static extern uint MsiOpenDatabase(string path, IntPtr persist, out uint handle);
    [DllImport("msi.dll", CharSet = CharSet.Unicode)] static extern uint MsiDatabaseOpenView(uint db, string query, out uint view);
    [DllImport("msi.dll")] static extern uint MsiViewExecute(uint view, uint record);
    [DllImport("msi.dll")] static extern uint MsiViewFetch(uint view, out uint record);
    [DllImport("msi.dll")] static extern uint MsiRecordGetFieldCount(uint record);
    [DllImport("msi.dll", CharSet = CharSet.Unicode)] static extern uint MsiRecordGetString(uint record, uint field, StringBuilder buffer, ref uint count);
    [DllImport("msi.dll")] static extern uint MsiCloseHandle(uint handle);
    public static Dictionary<string, List<string[]>> Read(string path)
    {
        Check(MsiOpenDatabase(path, IntPtr.Zero, out var db)); try
        {
            var result = new Dictionary<string, List<string[]>>(); foreach (var q in new[] { ("File", "SELECT `File`,`Component_`,`FileName`,`FileSize`,`Sequence` FROM `File`"), ("Directory", "SELECT `Directory`,`Directory_Parent`,`DefaultDir` FROM `Directory`"), ("Component", "SELECT `Component`,`Directory_` FROM `Component`"), ("Media", "SELECT `DiskId`,`LastSequence`,`Cabinet` FROM `Media`") })
            {
                Check(MsiDatabaseOpenView(db, q.Item2, out var view)); try { Check(MsiViewExecute(view, 0)); var list = new List<string[]>(); while (true) { var status = MsiViewFetch(view, out var r); if (status == 259) break; Check(status); try { var a = new string[MsiRecordGetFieldCount(r)]; for (uint i = 1; i <= a.Length; i++) { uint length = 8192; var s = new StringBuilder((int)length); Check(MsiRecordGetString(r, i, s, ref length)); a[i - 1] = s.ToString(); } list.Add(a); } finally { MsiCloseHandle(r); } } result[q.Item1] = list; } finally { MsiCloseHandle(view); }
            }
            return result;
        }
        finally { MsiCloseHandle(db); }
    }
    static void Check(uint code) { if (code != 0) throw new Exception("Read-only MSI query error " + code); }
}
