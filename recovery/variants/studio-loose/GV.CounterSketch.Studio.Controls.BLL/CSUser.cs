using System;

namespace GV.CounterSketch.Studio.Controls.BLL;

[Serializable]
public class CSUser : BindableObject
{
	public int UserId { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string EMail { get; set; }

	public string Phone { get; set; }

	public bool IsAdmin { get; set; }
}
