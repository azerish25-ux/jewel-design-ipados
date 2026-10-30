using System.Runtime.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Shared.Model;

[DataContract]
public class BitmapSourceByteArrayInfo
{
	public BitmapSource ItemImageSource => BitmapSource.Create(ItemImageWidth, ItemImageHeight, 96.0, 96.0, PixelFormats.Bgra32, null, ItemImageSourceByteArray, ItemImageStride);

	[DataMember]
	public int ItemImageHeight { get; set; }

	[DataMember]
	public byte[] ItemImageSourceByteArray { get; set; }

	[DataMember]
	public int ItemImageStride { get; set; }

	[DataMember]
	public int ItemImageWidth { get; set; }
}
