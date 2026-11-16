#include <CoreFoundation/CoreFoundation.h>
#include <CoreGraphics/CoreGraphics.h>
#include <ImageIO/ImageIO.h>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <vector>
#include <string>
#include <stdexcept>
template<class T>struct Owned{T value;Owned(T p):value(p){}~Owned(){if(value)CFRelease(value);}Owned(const Owned&)=delete;};
static std::string quoted(const std::string&s){std::string out="\"";for(unsigned char c:s){if(c=='"'||c=='\\'){out+='\\';out+=c;}else if(c<32){const char*h="0123456789abcdef";out+="\\u00";out+=h[c>>4];out+=h[c&15];}else out+=c;}return out+"\"";}
static std::string text(CFStringRef s){if(!s)return "";std::vector<char> b(CFStringGetMaximumSizeForEncoding(CFStringGetLength(s),kCFStringEncodingUTF8)+1);if(!CFStringGetCString(s,b.data(),b.size(),kCFStringEncodingUTF8))throw std::runtime_error("CFString conversion");return b.data();}
int main(int argc,char**argv){
 try{
  if(argc<3)throw std::runtime_error("usage: probe output-folder input-files...");
  const std::filesystem::path output=argv[1];std::filesystem::create_directories(output);
  for(int index=2;index<argc;index++){
   std::ifstream in(argv[index],std::ios::binary|std::ios::ate);if(!in)throw std::runtime_error("input open failed");
   auto size=in.tellg();if(size<1||size>4*1024*1024)throw std::runtime_error("fixture encoded budget");
   std::vector<unsigned char> encoded(static_cast<size_t>(size));in.seekg(0);if(!in.read(reinterpret_cast<char*>(encoded.data()),encoded.size()))throw std::runtime_error("short fixture");
   Owned<CFDataRef> data(CFDataCreate(kCFAllocatorDefault,encoded.data(),encoded.size()));
   const void*keys[]={kCGImageSourceShouldCache};const void*values[]={kCFBooleanFalse};
   Owned<CFDictionaryRef> options(CFDictionaryCreate(kCFAllocatorDefault,keys,values,1,&kCFTypeDictionaryKeyCallBacks,&kCFTypeDictionaryValueCallBacks));
   Owned<CGImageSourceRef> source(CGImageSourceCreateWithData(data.value,options.value));if(!source.value)throw std::runtime_error("source unavailable");
   Owned<CGImageRef> image(CGImageSourceCreateImageAtIndex(source.value,0,options.value));if(!image.value)throw std::runtime_error("decode unavailable");
   size_t width=CGImageGetWidth(image.value),height=CGImageGetHeight(image.value);if(width<1||height<1||width>4096||height>4096)throw std::runtime_error("fixture dimensions");
   Owned<CFDataRef> provider(CGDataProviderCopyData(CGImageGetDataProvider(image.value)));if(!provider.value)throw std::runtime_error("provider bytes unavailable");
   auto count=CFDataGetLength(provider.value);if(count<1||count>1024*1024)throw std::runtime_error("provider budget");
   std::string name=std::filesystem::path(argv[index]).filename().string();auto bytesPath=output/(name+".provider.bin");
   std::ofstream out(bytesPath,std::ios::binary);out.write(reinterpret_cast<const char*>(CFDataGetBytePtr(provider.value)),count);if(!out)throw std::runtime_error("provider write failed");out.close();
   auto color=CGImageGetColorSpace(image.value);Owned<CFStringRef> colorName(color?CGColorSpaceCopyName(color):nullptr);
   std::cout<<"{\"file\":"<<quoted(name)<<",\"width\":"<<width<<",\"height\":"<<height
    <<",\"bitsPerComponent\":"<<CGImageGetBitsPerComponent(image.value)<<",\"bitsPerPixel\":"<<CGImageGetBitsPerPixel(image.value)
    <<",\"bytesPerRow\":"<<CGImageGetBytesPerRow(image.value)<<",\"alphaInfo\":"<<CGImageGetAlphaInfo(image.value)
    <<",\"bitmapInfo\":"<<CGImageGetBitmapInfo(image.value)<<",\"colorSpaceName\":"<<quoted(text(colorName.value))
    <<",\"colorSpaceModel\":"<<(color?CGColorSpaceGetModel(color):-1)<<",\"providerBytes\":"<<count
    <<",\"sourceStatus\":"<<CGImageSourceGetStatusAtIndex(source.value,0)<<",\"sourceType\":"<<quoted(text(CGImageSourceGetType(source.value)))<<"}\n";
  }
 }catch(const std::exception&e){std::cerr<<e.what()<<"\n";return 1;}return 0;
}
