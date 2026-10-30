extern alias Legacy;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Utilities.Helpers;
static void Equal(double expected,double actual,string name){if(expected!=actual)throw new Exception(name+": expected "+expected+", actual "+actual);}
Equal(105,(int)GemMaterial.Diamond,"Serialized diamond ID");
Equal(3.52,GemCaratHelper.GetSpecificGravityForMaterial(GemMaterial.Diamond),"Diamond");
Equal(5.8,GemCaratHelper.GetSpecificGravityForMaterial(GemMaterial.CZWhite),"CZ");
Equal(4.0,GemCaratHelper.GetSpecificGravityForMaterial(GemMaterial.SapphireWhite),"Sapphire");
int count=0;foreach(var material in Enum.GetValues<GemMaterial>()){Equal(Legacy::GV.CounterSketch.Shared.Utilities.Helpers.GemCaratHelper.GetSpecificGravityForMaterial((Legacy::GV.CounterSketch.Shared.GemMaterial)(int)material),GemCaratHelper.GetSpecificGravityForMaterial(material),material.ToString());count++;}
foreach(int invalid in new[]{int.MinValue,-1,int.MaxValue})Equal(3.52,GemCaratHelper.GetSpecificGravityForMaterial((GemMaterial)invalid),"Unknown-material fallback");
Console.WriteLine($"PASS production domain: {count} recovered-baseline material comparisons, fixed ID/density cases and three unknown-ID fallbacks; no PC binary/geometry claim.");
