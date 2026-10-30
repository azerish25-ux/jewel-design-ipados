using System.Drawing;
using System.Windows.Forms;

namespace CS;

public static class TopMostMessageBox
{
	public static DialogResult Show(string message)
	{
		return Show(message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
	}

	public static DialogResult Show(string message, string title)
	{
		return Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.None);
	}

	public static DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
	{
		Form form = new Form();
		form.Size = new Size(1, 1);
		form.StartPosition = FormStartPosition.CenterScreen;
		form.FormBorderStyle = FormBorderStyle.None;
		form.Show();
		form.Focus();
		form.BringToFront();
		form.TopMost = true;
		DialogResult result = MessageBox.Show(form, message, title, buttons, icon);
		form.Dispose();
		return result;
	}
}
