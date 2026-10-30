using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Geomagic.Utilities;

public class LocalCaseManager<T>
{
	private Dictionary<string, T> _FilePathToCaseLookup = new Dictionary<string, T>();

	private Dictionary<T, string> _CaseToFilePathLookup = new Dictionary<T, string>();

	private List<FileSystemWatcher> _FileWatchers = new List<FileSystemWatcher>();

	private List<FileSystemWatcher> _DirectoryWatchers = new List<FileSystemWatcher>();

	private List<FileSystemWatcher> _RootWatchers = new List<FileSystemWatcher>();

	private List<DirectoryInfo> _WatchedDirectories = new List<DirectoryInfo>();

	private bool _IncludeSubdirectories;

	private bool _IncludeRoot;

	private string _Filter = "*.*";

	private ICaseParser<T> _Parser = new DefaultCaseParser<T>();

	private bool _IsRunning;

	public IEnumerable<T> Cases => _CaseToFilePathLookup.Keys;

	public bool IncludeSubdirectories
	{
		get
		{
			return _IncludeSubdirectories;
		}
		set
		{
			_IncludeSubdirectories = value;
		}
	}

	public bool IncludeRoot
	{
		get
		{
			return _IncludeRoot;
		}
		set
		{
			_IncludeRoot = value;
		}
	}

	public string Filter
	{
		get
		{
			return _Filter;
		}
		set
		{
			_Filter = value;
		}
	}

	public ICaseParser<T> Parser
	{
		get
		{
			return _Parser;
		}
		set
		{
			_Parser = value;
		}
	}

	public bool IsRunning
	{
		get
		{
			return _IsRunning;
		}
		private set
		{
			_IsRunning = value;
		}
	}

	public event CaseEventHandler<T> CaseAdded;

	public event CaseEventHandler<T> CaseDeleted;

	private void NotifyCaseAdded(T Case)
	{
		if (this.CaseAdded != null)
		{
			CaseEventArgs<T> args = new CaseEventArgs<T>(Case);
			this.CaseAdded(this, args);
		}
	}

	private void NotifyCaseDeleted(T Case)
	{
		if (this.CaseDeleted != null)
		{
			CaseEventArgs<T> args = new CaseEventArgs<T>(Case);
			this.CaseDeleted(this, args);
		}
	}

	public void Start(string FolderPath)
	{
		if (string.IsNullOrEmpty(FolderPath))
		{
			throw new ArgumentNullException("FolderPath");
		}
		string[] folderPaths = new string[1] { FolderPath };
		Start(folderPaths);
	}

	public void Start(string[] FolderPaths)
	{
		if (IsRunning)
		{
			Stop();
		}
		if (FolderPaths == null || FolderPaths.Length == 0)
		{
			throw new ArgumentNullException("FolderPaths");
		}
		foreach (string path in FolderPaths)
		{
			if (!Directory.Exists(path))
			{
				throw new DirectoryNotFoundException();
			}
		}
		foreach (string text in FolderPaths)
		{
			_WatchedDirectories.Add(new DirectoryInfo(text));
			FindAndAddNewCasesToCaseList(text);
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(text);
			fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
			fileSystemWatcher.Filter = Filter;
			fileSystemWatcher.IncludeSubdirectories = IncludeSubdirectories;
			fileSystemWatcher.Changed += OnFileChanged;
			fileSystemWatcher.Renamed += OnFileChanged;
			fileSystemWatcher.Created += OnFileChanged;
			fileSystemWatcher.Deleted += OnFileChanged;
			fileSystemWatcher.EnableRaisingEvents = true;
			_FileWatchers.Add(fileSystemWatcher);
			if (IncludeSubdirectories)
			{
				FileSystemWatcher fileSystemWatcher2 = new FileSystemWatcher(text);
				fileSystemWatcher2.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
				fileSystemWatcher2.IncludeSubdirectories = IncludeSubdirectories;
				fileSystemWatcher2.Changed += OnSubdirectoryChanged;
				fileSystemWatcher2.Renamed += OnSubdirectoryChanged;
				fileSystemWatcher2.Created += OnSubdirectoryChanged;
				fileSystemWatcher2.Deleted += OnSubdirectoryChanged;
				fileSystemWatcher2.EnableRaisingEvents = true;
				_DirectoryWatchers.Add(fileSystemWatcher2);
			}
			if (IncludeRoot)
			{
				string directoryRoot = Directory.GetDirectoryRoot(text);
				FileSystemWatcher fileSystemWatcher3 = new FileSystemWatcher(directoryRoot);
				fileSystemWatcher3.NotifyFilter = NotifyFilters.DirectoryName;
				fileSystemWatcher3.IncludeSubdirectories = false;
				fileSystemWatcher3.Changed += OnRootChanged;
				fileSystemWatcher3.Renamed += OnRootChanged;
				fileSystemWatcher3.Created += OnRootChanged;
				fileSystemWatcher3.Deleted += OnRootChanged;
				fileSystemWatcher3.EnableRaisingEvents = true;
				_RootWatchers.Add(fileSystemWatcher3);
			}
		}
		IsRunning = true;
	}

	public void Stop()
	{
		if (!IsRunning)
		{
			return;
		}
		_FilePathToCaseLookup.Clear();
		_CaseToFilePathLookup.Clear();
		foreach (FileSystemWatcher fileWatcher in _FileWatchers)
		{
			if (fileWatcher != null)
			{
				fileWatcher.Changed -= OnFileChanged;
				fileWatcher.Renamed -= OnFileChanged;
				fileWatcher.Created -= OnFileChanged;
				fileWatcher.Deleted -= OnFileChanged;
				fileWatcher.Dispose();
			}
		}
		_FileWatchers.Clear();
		foreach (FileSystemWatcher directoryWatcher in _DirectoryWatchers)
		{
			if (directoryWatcher != null)
			{
				directoryWatcher.Changed -= OnSubdirectoryChanged;
				directoryWatcher.Renamed -= OnSubdirectoryChanged;
				directoryWatcher.Created -= OnSubdirectoryChanged;
				directoryWatcher.Deleted -= OnSubdirectoryChanged;
				directoryWatcher.Dispose();
			}
		}
		_DirectoryWatchers.Clear();
		foreach (FileSystemWatcher rootWatcher in _RootWatchers)
		{
			if (rootWatcher != null)
			{
				rootWatcher.Changed -= OnRootChanged;
				rootWatcher.Renamed -= OnRootChanged;
				rootWatcher.Created -= OnRootChanged;
				rootWatcher.Deleted -= OnRootChanged;
				rootWatcher.Dispose();
			}
		}
		_RootWatchers.Clear();
	}

	private void OnFileChanged(object sender, FileSystemEventArgs e)
	{
		string fullPath = e.FullPath;
		switch (e.ChangeType)
		{
		case WatcherChangeTypes.Created:
			AddToCaseList(fullPath);
			break;
		case WatcherChangeTypes.Changed:
			AddToCaseList(fullPath, RefreshIfAlreadyExists: true);
			break;
		case WatcherChangeTypes.Deleted:
			FindAndRemoveDeletedCasesFromCaseList();
			break;
		case WatcherChangeTypes.Created | WatcherChangeTypes.Deleted:
			break;
		}
	}

	private void OnSubdirectoryChanged(object sender, FileSystemEventArgs e)
	{
		string fullPath = e.FullPath;
		FindAndRemoveDeletedCasesFromCaseList();
		if (Directory.Exists(fullPath))
		{
			FindAndAddNewCasesToCaseList(fullPath);
		}
	}

	private void OnRootChanged(object sender, FileSystemEventArgs e)
	{
		string fullPath = e.FullPath;
		if (IsWatched(fullPath))
		{
			FindAndRemoveDeletedCasesFromCaseList();
		}
	}

	private void AddToCaseList(string FilePath, bool RefreshIfAlreadyExists = false)
	{
		T Obj = default(T);
		if (!Parser.Read(FilePath, out Obj))
		{
			return;
		}
		if (!Cases.Contains(Obj))
		{
			_FilePathToCaseLookup.Add(FilePath, Obj);
			_CaseToFilePathLookup.Add(Obj, FilePath);
			NotifyCaseAdded(Obj);
		}
		else if (RefreshIfAlreadyExists)
		{
			string text = _CaseToFilePathLookup[Obj];
			if (text != FilePath)
			{
				_CaseToFilePathLookup[Obj] = FilePath;
				_FilePathToCaseLookup.Remove(text);
				_FilePathToCaseLookup.Add(FilePath, Obj);
			}
		}
	}

	private void FindAndAddNewCasesToCaseList()
	{
		foreach (DirectoryInfo watchedDirectory in _WatchedDirectories)
		{
			string fullName = watchedDirectory.FullName;
			FindAndAddNewCasesToCaseList(fullName);
		}
	}

	private void FindAndAddNewCasesToCaseList(string FolderPath)
	{
		SearchOption searchOption = (IncludeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		string[] files = Directory.GetFiles(FolderPath, Filter, searchOption);
		string[] array = files;
		foreach (string filePath in array)
		{
			try
			{
				AddToCaseList(filePath, RefreshIfAlreadyExists: true);
			}
			catch (Exception)
			{
			}
		}
	}

	private void FindAndRemoveDeletedCasesFromCaseList()
	{
		List<string> list = new List<string>();
		foreach (string key in _FilePathToCaseLookup.Keys)
		{
			if (!File.Exists(key))
			{
				list.Add(key);
			}
		}
		foreach (string item in list)
		{
			T val = _FilePathToCaseLookup[item];
			_CaseToFilePathLookup.Remove(val);
			_FilePathToCaseLookup.Remove(item);
			NotifyCaseDeleted(val);
		}
	}

	private bool IsWatched(string FolderPath)
	{
		foreach (DirectoryInfo watchedDirectory in _WatchedDirectories)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(FolderPath);
			if (directoryInfo.FullName == watchedDirectory.FullName)
			{
				return true;
			}
		}
		return false;
	}
}
