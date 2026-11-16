namespace JewelDesign.Rendering.Candidate;

/// <summary>Current-host measured default Win32 HALFTONE arithmetic for full positive BI_RGB24/32 rectangles.
/// Original vendor/DC profiles/clipping and composed image preparation remain separate. No alpha repair is applied.
/// Fast/general enlargement and truecolor depth conversion are source-backed; dimensions beyond16384 and over-budget working storage reject.</summary>
public static class Rhino5GdiHalftoneStretch
{
 private readonly record struct Weight(int Index,int Value);
 public static byte[] ResizeStored(ReadOnlySpan<byte> source,int sw,int sh,int depth,int w,int h,byte destinationSeed=0,LocalImageLimits? limits=null,long maxWorkingBytes=64*1024*1024,int? destinationDepth=null)
 {
  if(depth is not (24 or 32))throw new NotSupportedException("HALFTONE requires BI_RGB24/32 stored source bytes.");
  int outputDepth=destinationDepth??depth;if(outputDepth is not(24 or 32))throw new NotSupportedException("HALFTONE destination requires BI_RGB24/32.");
  limits??=new();limits.ValidateDimensions(sw,sh);limits.ValidateDimensions(w,h);
  if(sw>16384||sh>16384||w>16384||h>16384)throw new InvalidDataException("HALFTONE component dimension budget is16384.");
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxWorkingBytes);
  int pixel=depth/8,destinationPixel=outputDepth/8,ss=(sw*pixel+3)&~3,stride=(w*destinationPixel+3)&~3;
  if(source.Length!=ss*sh)throw new ArgumentException("Source storage differs from positive DWORD rows.",nameof(source));
  // Conservative peak by actual stage shape, including a possible immutable-source fixup clone.
  long sourcePixels=(long)sw*sh,targetPixels=(long)w*h,stages;
  if(sw>w&&sh>h)stages=24*sourcePixels+12L*w*sh+24*targetPixels;
  else if(sw>w)stages=24*sourcePixels+36L*w*sh+12*targetPixels;
  else if(sh>h)stages=24*sourcePixels+36L*sw*h+12*targetPixels;
  else if(sw<w&&sh<h&&w<=5*sw&&h<=5*sh)stages=24*sourcePixels+12*targetPixels+12L*sw;
  else if(sw<w&&sh<h)stages=36*sourcePixels+12L*w*sh+12*targetPixels;
  else stages=36*sourcePixels+12*targetPixels;
  long working=stages+source.Length+(long)stride*h+64L*(sw+sh+w+h)+65536;
  if(working>maxWorkingBytes)throw new InvalidDataException("HALFTONE conservative working storage budget exceeded.");
  int[] raw=new int[sw*sh*3];for(int y=0;y<sh;y++)for(int x=0;x<sw;x++){int s=(sh-1-y)*ss+x*pixel,i=(y*sw+x)*3;raw[i]=source[s+2];raw[i+1]=source[s+1];raw[i+2]=source[s];}
  var policy=Policy(raw,sw,sh,w,h);int[] input=policy.Fixup?Fixup(raw,sw,sh):raw,output;
  if(policy.NoSharpen){output=new int[w*h*3];for(int y=0;y<h;y++)for(int x=0;x<w;x++){int i=(Sample(sh,h,y)*sw+Sample(sw,w,x))*3,j=(y*w+x)*3;Array.Copy(input,i,output,j,3);}}
  else if(sw>w&&sh>h){var horizontal=ApplyX(input,sw,sh,w,Shrink(sw,w),true);var vertical=ApplyY(horizontal,w,sh,h,Shrink(sh,h),false);output=Sharp2(vertical,w,h);}
  else if(sw>w){var horizontal=SharpX(ApplyX(input,sw,sh,w,Shrink(sw,w),false),w,sh,15);output=sh<h?ApplyY(SharpY(horizontal,w,sh,2),w,sh,h,Expand(sh,h),true):horizontal;}
  else if(sh>h){var vertical=SharpY(ApplyY(input,sw,sh,h,Shrink(sh,h),false),sw,h,15);output=sw<w?ApplyX(SharpX(vertical,sw,h,2),sw,h,w,Expand(sw,w),true):vertical;}
  else if(sw==w&&sh==h)output=input;
  else if(sw<w&&sh<h){if(w<=5*sw&&h<=5*sh)output=FastBoth(input,sw,sh,w,h);else output=ApplyY(ApplyX(SharpInput2(input,sw,sh),sw,sh,w,Expand(sw,w),true),w,sh,h,Expand(sh,h),true);}
  else if(sw<w)output=ApplyX(SharpX(input,sw,sh,2),sw,sh,w,Expand(sw,w),true);
  else output=ApplyY(SharpY(input,sw,sh,2),sw,sh,h,Expand(sh,h),true);
  byte[] result=new byte[stride*h];if(destinationSeed!=0)Array.Fill(result,destinationSeed);
  for(int y=0;y<h;y++)for(int x=0;x<w;x++){int d=(h-1-y)*stride+x*destinationPixel,i=(y*w+x)*3;result[d]=(byte)output[i+2];result[d+1]=(byte)output[i+1];result[d+2]=(byte)output[i];if(outputDepth==32)result[d+3]=0;}
  return result;
 }
 private static int Clamp(int v)=>Math.Clamp(v,0,255);
 private static int Sample(int n,int d,int i)=>n>d?(2*n*(i+1)-d)/(2*d):Math.Max(0,((2*i+1)*n+2*d-1)/(2*d)-1);
 private static (bool Fixup,bool NoSharpen) Policy(int[] rgb,int sw,int sh,int w,int h)
 {
  if((w*1000+500)/sw<=667||(h*1000+500)/sh<=667)return(false,false);
  int area=sw*sh;if(area<=2304)return(true,area<=w*h);
  int step=area>16384?6:1,limit=step==6?20:area>>3,remaining=area;bool noSharpen=false;var colors=new HashSet<int>();
  for(int y=0;y<sh;y+=step){bool added=false;for(int x=0;x<sw;x++){int i=(y*sw+x)*3,r=rgb[i],g=rgb[i+1],b=rgb[i+2];if(r==b){r&=252;g&=252;b&=252;}if(colors.Add((r<<16)|(g<<8)|b)){added=true;if(colors.Count>limit)break;}}
   if(limit!=20&&!added){remaining-=sw;if(remaining<2305){noSharpen=true;break;}limit=remaining>>4;}if(colors.Count>limit)break;
  }
  if(colors.Count<20)noSharpen=true;
  bool fixup=limit==20?colors.Count<=20:!(colors.Count>20||colors.Count<=limit);
  return(fixup,noSharpen&&area<=w*h);
 }
 private static int[] Fixup(int[] raw,int w,int h)
 {
  int[] output=(int[])raw.Clone();if(w<2||h<2)return output;
  int At(int x,int y){x=x<0?-x:x>=w?2*w-2-x:x;y=y<0?-y:y>=h?2*h-2-y:y;return(y*w+x)*3;}
  bool Same(int a,int b)=>raw[a]==raw[b]&&raw[a+1]==raw[b+1]&&raw[a+2]==raw[b+2];
  int Luma(int p)=>4*raw[p]+8*raw[p+1]+raw[p+2];
  void Smooth(int p,int a,int b,int c,int d){for(int k=0;k<3;k++)output[p+k]=(12*output[p+k]+raw[a+k]+raw[b+k]+raw[c+k]+raw[d+k]+8)>>4;}
  for(int y=0;y<h-1;y++)for(int x=0;x<w-1;x++){int a=At(x,y),b=At(x+1,y),c=At(x,y+1),d=At(x+1,y+1);if(Same(a,b)||!Same(c,b)||!Same(d,a))continue;
   int l=At(x-1,y),r=At(x+2,y),bl=At(x-1,y+1),br=At(x+2,y+1),ua=At(x,y-1),ub=At(x+1,y-1),fa=At(x,y+2),fb=At(x+1,y+2);
   if((Same(l,b)&&Same(r,a)&&Same(bl,d)&&Same(br,c))||(Same(ua,c)&&Same(ub,d)&&Same(fa,a)&&Same(fb,b))){for(int k=0;k<3;k++){int v=(raw[a+k]+raw[b+k]+1)>>1;output[a+k]=output[b+k]=output[c+k]=output[d+k]=v;}}
   else if(Luma(a)<Luma(b)){Smooth(b,d,l,fb,a);Smooth(c,d,ua,br,a);}else{Smooth(a,fa,b,r,c);Smooth(d,c,ub,bl,b);}
  }
  return output;
 }
 private static Weight[][] Shrink(int n,int d)
 {
  var rows=new Weight[d][];for(int x=0;x<d;x++){var row=new List<Weight>();int total=0,previous=0;for(int i=0;i<n;i++){int overlap=Math.Max(0,Math.Min((x+1)*n,(i+1)*d)-Math.Max(x*n,i*d));if(overlap==0)continue;total+=overlap;int cumulative=(int)((long)total*8192/n);row.Add(new(i,cumulative-previous));previous=cumulative;}if(previous!=8192)throw new InvalidOperationException("Q13 shrinking coefficient sum.");rows[x]=row.ToArray();}return rows;
 }
 private static int Floor(int a,int b){int q=a/b;return a<0&&a%b!=0?q-1:q;}
 private static Weight[][] Expand(int n,int d)
 {
  int radius=(d+n-1)/n-1;var kernel=new int[2*radius+1];for(int j=0;j<kernel.Length;j++){int distance=Math.Abs(j-radius);if(distance==0)kernel[j]=1000000;else{int r=DecimalKernel.Divide(d-distance*n,d);kernel[j]=r==500000?r:DecimalKernel.Power(r,r>500000);}}
  long denominator=(long)n*kernel.Sum(v=>(long)v);var rows=new Weight[d][];
  for(int x=0;x<d;x++){var raw=new Dictionary<int,long>();for(int k=-radius;k<=radius;k++){int l=(x+k)*n,r=(x+1+k)*n;for(int i=Floor(l,d);i<-Floor(-r,d);i++){int overlap=Math.Max(0,Math.Min(r,(i+1)*d)-Math.Max(l,i*d)),index=Math.Clamp(i,0,n-1);raw[index]=raw.GetValueOrDefault(index)+(long)overlap*kernel[k+radius];}}
   long sum=0;int previous=0;var row=new List<Weight>();foreach(var pair in raw.OrderByDescending(p=>p.Key)){sum+=pair.Value;int cumulative=(int)(sum*8192/denominator);row.Add(new(pair.Key,cumulative-previous));previous=cumulative;}if(previous!=8192)throw new InvalidOperationException("Q13 expanding coefficient sum.");rows[x]=row.ToArray();
  }
  return rows;
 }
 private static int[] ApplyX(int[] input,int sw,int h,int w,Weight[][] weights,bool round)
 {int[] output=new int[w*h*3];for(int y=0;y<h;y++)for(int x=0;x<w;x++)for(int k=0;k<3;k++){int sum=0;foreach(var v in weights[x])sum+=input[(y*sw+v.Index)*3+k]*v.Value;output[(y*w+x)*3+k]=round?(sum+4096)>>13:sum;}return output;}
 private static int[] ApplyY(int[] input,int w,int sh,int h,Weight[][] weights,bool round)
 {int[] output=new int[w*h*3];for(int y=0;y<h;y++)for(int x=0;x<w;x++)for(int k=0;k<3;k++){int sum=0;foreach(var v in weights[y])sum+=input[(v.Index*w+x)*3+k]*v.Value;output[(y*w+x)*3+k]=round?(sum+4096)>>13:sum;}return output;}
 private static int[] SharpX(int[] input,int w,int h,int shift)
 {int[] output=new int[input.Length];for(int y=0;y<h;y++)for(int x=0;x<w;x++)for(int k=0;k<3;k++){int i=(y*w+x)*3+k;output[i]=Clamp((6*input[i]-input[(y*w+Math.Max(0,x-1))*3+k]-input[(y*w+Math.Min(w-1,x+1))*3+k])>>shift);}return output;}
 private static int[] SharpY(int[] input,int w,int h,int shift)
 {int[] output=new int[input.Length];for(int y=0;y<h;y++)for(int x=0;x<w;x++)for(int k=0;k<3;k++){int i=(y*w+x)*3+k;output[i]=Clamp((6*input[i]-input[(Math.Max(0,y-1)*w+x)*3+k]-input[(Math.Min(h-1,y+1)*w+x)*3+k])>>shift);}return output;}
 private static int[] Sharp2(int[] input,int w,int h)
 {int[] output=new int[input.Length];for(int y=0;y<h;y++)for(int x=0;x<w;x++)for(int k=0;k<3;k++){int i=(y*w+x)*3+k;output[i]=Clamp((12*input[i]-input[(y*w+Math.Max(0,x-1))*3+k]-input[(y*w+Math.Min(w-1,x+1))*3+k]-input[(Math.Max(0,y-1)*w+x)*3+k]-input[(Math.Min(h-1,y+1)*w+x)*3+k])>>16);}return output;}
 private static int[] SharpInput2(int[] input,int w,int h)
 {int[] output=new int[input.Length];for(int y=0;y<h;y++)for(int x=0;x<w;x++)for(int k=0;k<3;k++)output[(y*w+x)*3+k]=InputSharpAt(input,w,h,x,y,k);return output;}
 private static int InputSharpAt(int[] input,int w,int h,int x,int y,int k)
 {int At(int px,int py)=>input[(Math.Clamp(py,0,h-1)*w+Math.Clamp(px,0,w-1))*3+k];return Clamp((12*At(x,y)-At(x-1,y)-At(x+1,y)-At(x,y-1)-At(x,y+1))>>3);}
 private readonly record struct FastStencil(int Source,int Previous,int Current,int Next,int Shift);
 // Integer run stencils from native FastExpAA_CX/CY; valid only for strict enlargement up to5x.
 private static readonly int[][][] FastStencils=[[],[[5,22,5,5]],[[1,3,0,2],[0,3,1,2]],[[3,5,0,3],[1,14,1,4],[0,5,3,3]],[[3,5,0,3],[3,12,1,4],[1,12,3,4],[0,5,3,3]],[[13,19,0,5],[6,25,1,5],[3,26,3,5],[1,25,6,5],[0,19,13,5]]];
 private static FastStencil[] FastRows(int n,int d)
 {int[] counts=new int[n];for(int i=0;i<d;i++)counts[Sample(n,d,i)]++;var rows=new List<FastStencil>(d);for(int source=0;source<n;source++)foreach(var s in FastStencils[counts[source]])rows.Add(new(source,s[0],s[1],s[2],s[3]));return rows.ToArray();}
 private static int[] FastBoth(int[] input,int sw,int sh,int w,int h)
 {
  var rowsX=FastRows(sw,w);var rowsY=FastRows(sh,h);int[] output=new int[w*h*3],vertical=new int[sw*3];
  for(int y=0;y<h;y++){var row=rowsY[y];for(int x=0;x<sw;x++)for(int k=0;k<3;k++)vertical[x*3+k]=(row.Previous*InputSharpAt(input,sw,sh,x,row.Source-1,k)+row.Current*InputSharpAt(input,sw,sh,x,row.Source,k)+row.Next*InputSharpAt(input,sw,sh,x,row.Source+1,k)+(1<<(row.Shift-1)))>>row.Shift;
   // Raw virtual Y scans are sharpened before the vertical stencil; X padding repeats the already filtered edge.
   for(int x=0;x<w;x++){var q=rowsX[x];for(int k=0;k<3;k++)output[(y*w+x)*3+k]=(q.Previous*vertical[Math.Max(0,q.Source-1)*3+k]+q.Current*vertical[q.Source*3+k]+q.Next*vertical[Math.Min(sw-1,q.Source+1)*3+k]+(1<<(q.Shift-1)))>>q.Shift;}
  }
  return output;
 }
 private static class DecimalKernel
 {
  private static readonly int[] Log=Enumerable.Range(0,901).Select(i=>RoundedLog((100+i)/100.0)).ToArray();
  private static readonly int[][] Slopes=Enumerable.Range(0,900).Select(i=>Enumerable.Range(0,10).Select(j=>RoundedLog((100+i+(j+1)/10.0)/100)-RoundedLog((100+i+j/10.0)/100)).ToArray()).ToArray();
  private static int RoundedLog(double v)=>(int)Math.Round(Math.Log10(v)*1000000,MidpointRounding.AwayFromZero);
  private static long Round(long a,long b)=>(a+b/2)/b;
  public static int Divide(int a,int b)=>(int)(Math.Sign((long)a*b)*Round(Math.Abs((long)a)*1000000,Math.Abs((long)b)));
  private static int Multiply(int a,int b)=>(int)(Math.Sign((long)a*b)*Round(Math.Abs((long)a*b),1000000));
  private static int Logarithm(int value){long scaled=(long)value*100;int exponent=0;while(scaled<100000000){scaled*=10;exponent--;}int i=(int)(scaled/1000000)-100,fraction=(int)(scaled%1000000),segment=fraction/100000,result=Log[i];if(fraction!=0){for(int j=0;j<segment;j++)result+=Slopes[i][j];result+=(int)Round((long)Slopes[i][segment]*(fraction%100000),100000);}return result+exponent*1000000;}
  private static long Pow10(int exponent){long n=1;for(int i=0;i<exponent;i++)n*=10;return n;}
  private static int AntiLog(int value){if(value<-5999999)return 1;int exponent=Floor(value,1000000),rem=value-exponent*1000000;if(rem==0)return(int)Round(1000000,Pow10(-exponent));int lo=0,hi=900;while(lo+1<hi){int mid=(lo+hi)>>1;if(Log[mid]<=rem)lo=mid;else hi=mid;}int m=rem-Log[lo],fraction=0;for(int j=0;j<10;j++){if(m<=Slopes[lo][j]){fraction=j*100000+(int)Round((long)m*100000,Slopes[lo][j]);break;}m-=Slopes[lo][j];}return(int)Round((lo+100L)*1000000+fraction,Pow10(2-exponent));}
  public static int Power(int value,bool inverse)=>value==0?0:AntiLog(inverse?Divide(Logarithm(value),1414214):Multiply(Logarithm(value),1414214));
 }
}
