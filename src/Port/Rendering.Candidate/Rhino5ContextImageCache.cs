namespace JewelDesign.Rendering.Candidate;

/// <summary>Explicit legacy filename operations. The original can use Windows NLS or a CRT locale;
/// no platform default comparer is silently substituted. The C profile is exactly UTF16/ASCII folding.</summary>
public sealed class Rhino5FilenameSemantics
{
 private readonly Func<string,string> lower;private readonly Comparison<string?> compare;
 public string Profile{get;}
 public Rhino5FilenameSemantics(string profile,Func<string,string> lower,Comparison<string?> compare){ArgumentException.ThrowIfNullOrEmpty(profile);ArgumentNullException.ThrowIfNull(lower);ArgumentNullException.ThrowIfNull(compare);Profile=profile;this.lower=lower;this.compare=compare;}
 public string Lower(string name){ArgumentNullException.ThrowIfNull(name);return lower(name);}
 public int Compare(string? first,string? second)=>compare(first,second);
 private static char Fold(char c)=>c>='A'&&c<='Z'?(char)(c+32):c;
 public static Rhino5FilenameSemantics CrtCLocale{get;}=new("original-NLS-disabled/CRT-C/UTF16",name=>{char[] chars=name.ToCharArray();for(int i=0;i<chars.Length&&chars[i]!=0;i++)chars[i]=Fold(chars[i]);return new(chars);},(left,right)=>{if(string.IsNullOrEmpty(right)||right[0]==0)return string.IsNullOrEmpty(left)?0:1;if(string.IsNullOrEmpty(left))return -1;int i=0;while(true){char a=i<left.Length?Fold(left[i]):'\0',b=i<right.Length?Fold(right[i]):'\0';if(a!=b)return a-b;if(a==0)return 0;i++;}});
}
public readonly record struct Rhino5ImageCacheKey(ulong Context,uint Kind,string Filename);
public readonly record struct Rhino5ImageCacheEntryInfo(Rhino5ImageCacheKey Key,uint UsageSerial);

/// <summary>Original sorted context/kind/name lookup and context purge with owned resources.
/// Cache leases retain resources beyond removal/disposal. Runtime NLS policy and source resolver are explicit owners.</summary>
public sealed class Rhino5ContextImageCache<T>:IDisposable where T:class,IDisposable
{
 private readonly object sync=new();private readonly Rhino5FilenameSemantics filenames;private readonly int maximumEntries;private readonly List<Entry> entries=[];private bool disposed;private uint usage;private long mutation;
 private sealed class Entry(Rhino5ImageCacheKey key,T resource,uint used)
 {
  internal readonly Rhino5ImageCacheKey Key=key;internal readonly T Value=resource;internal uint Usage=used;private int references=1;
  internal void Retain(){int n;do{n=Volatile.Read(ref references);ObjectDisposedException.ThrowIf(n==0,this);}while(Interlocked.CompareExchange(ref references,checked(n+1),n)!=n);}
  internal void Release(){int n=Interlocked.Decrement(ref references);if(n==0)Value.Dispose();else if(n<0)throw new InvalidOperationException("Image cache reference released twice.");}
 }
 public Rhino5ContextImageCache(Rhino5FilenameSemantics filenames,int maximumEntries=128){ArgumentNullException.ThrowIfNull(filenames);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumEntries);this.filenames=filenames;this.maximumEntries=maximumEntries;}
 public int Count{get{lock(sync)return entries.Count;}}
 public uint UsageSerial{get{lock(sync)return usage;}set{lock(sync){ObjectDisposedException.ThrowIf(disposed,this);usage=value;}}}
 public Rhino5ImageCacheEntryInfo[] Snapshot(){lock(sync)return entries.Select(e=>new Rhino5ImageCacheEntryInfo(e.Key,e.Usage)).ToArray();}
 public static int CompareKeys(Rhino5FilenameSemantics filenames,Rhino5ImageCacheKey first,Rhino5ImageCacheKey second){if(first.Context!=second.Context)return first.Context<second.Context?-1:1;if((first.Kind&second.Kind)!=0&&first.Kind!=second.Kind)return first.Kind<second.Kind?-1:1;return filenames.Compare(first.Filename,second.Filename);}
 public Lease? Lookup(ulong context,string? filename,bool gray,Func<T?> create)
 {
  ArgumentNullException.ThrowIfNull(create);lock(sync){ObjectDisposedException.ThrowIf(disposed,this);if(context==0||filename is null)return null;var key=new Rhino5ImageCacheKey(context,gray?3u:1u,filename);int low=0,high=entries.Count-1;
   while(low<=high){int mid=(low+high)/2,comparison=CompareKeys(filenames,entries[mid].Key,key);if(comparison<0)low=mid+1;else if(comparison>0)high=mid-1;else{entries[mid].Usage=usage;return NewLease(entries[mid]);}}
   if(entries.Count>=maximumEntries)throw new InvalidDataException("Source image cache entry budget reached; explicit invalidation is required.");long before=mutation;T? resource=create();if(resource is null)return null;
   if(disposed||mutation!=before){resource.Dispose();throw new InvalidOperationException("Source cache changed during image creation.");}
   Entry entry;try{entry=new(new(context,key.Kind,filenames.Lower(filename)),resource,usage);var sorted=entries.ToList();sorted.Add(entry);sorted.Sort((a,b)=>CompareKeys(filenames,a.Key,b.Key));entries.Clear();entries.AddRange(sorted);mutation++;}catch{resource.Dispose();throw;}return NewLease(entry);
  }
 }
 private Lease NewLease(Entry entry){entry.Retain();return new(entry.Value,entry.Release);}
 public int ClearContext(ulong context){List<Entry> removed=[];lock(sync){ObjectDisposedException.ThrowIf(disposed,this);for(int i=entries.Count-1;i>=0;i--)if(entries[i].Key.Context==context){removed.Add(entries[i]);entries.RemoveAt(i);}mutation++;}ReleaseAll(removed);return removed.Count;}
 public int Invalidate(string filename){ArgumentNullException.ThrowIfNull(filename);List<Entry> removed=[];lock(sync){ObjectDisposedException.ThrowIf(disposed,this);for(int i=entries.Count-1;i>=0;i--)if(filenames.Compare(entries[i].Key.Filename,filename)==0){removed.Add(entries[i]);entries.RemoveAt(i);}mutation++;}ReleaseAll(removed);return removed.Count;}
 private static void ReleaseAll(List<Entry> removed){List<Exception>? errors=null;foreach(var entry in removed)try{entry.Release();}catch(Exception e){(errors??=[]).Add(e);}if(errors is not null)throw new AggregateException("Cached image release failed.",errors);}
 public void Dispose(){List<Entry> removed;lock(sync){if(disposed)return;disposed=true;removed=entries.AsEnumerable().Reverse().ToList();entries.Clear();mutation++;}ReleaseAll(removed);}
 public sealed class Lease:IDisposable
 {private T? resource;private Action? release;internal Lease(T resource,Action release){this.resource=resource;this.release=release;}public T Resource=>resource??throw new ObjectDisposedException(nameof(Lease));public void Dispose(){var action=Interlocked.Exchange(ref release,null);if(action is null)return;resource=null;action();}}
}
