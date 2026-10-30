using JewelDesign.Graph;
namespace GraphContractChecks;
public static class Assertions {
 static void Check(bool ok,string message){if(!ok)throw new Exception(message);}
 static void Throws<T>(Action action) where T:Exception {try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 public static string Run(){
 ParameterAddressAssertions.Run();
 var undoResult = UndoJournalAssertions.Run();
 var identityResult = SessionIdentityAssertions.Run();
 var type=Guid.Parse("56b84606-7f1d-41fb-b81f-f521d9a9577d");
 var raw=new byte[]{0,255,10,0,127};var links=new List<SerializedChannelLink>{SerializedChannelLink.Disconnected,new(-1,-1),new(777,99)};
 var node=new GraphNodeSnapshot(9,type,new(1,1),"Top Thickness",null,uint.MaxValue,raw,links);
 raw[0]=3;links[0]=new(2,0);
 Check(node.CopyOriginalRecordBytes()[0]==0,"Input payload aliasing");var copy=node.CopyOriginalRecordBytes();copy[1]=0;Check(node.CopyOriginalRecordBytes()[1]==255,"Returned payload aliasing");
 Check(node.InputLinks[0]==new SerializedChannelLink(-1,-5),"Serialized disconnected sentinel");Check(node.InputLinks[1]==new SerializedChannelLink(-1,-1),"Runtime-like sentinel normalized");Check(node.InputLinks[2]==new SerializedChannelLink(777,99),"Unknown link lost");
 Check(node.TypeGuid==type&&node.Version==new RecordVersion(1,1)&&node.BitFlags==uint.MaxValue&&node.StoredInstanceGuid is null,"Header changed");
 var other=new GraphNodeSnapshot(2,Guid.Empty,new(99,7),"Top Thickness","malformed-preserve-me",65520,new byte[]{42},new[]{new SerializedChannelLink(9,0)});
 var list=new List<GraphNodeSnapshot>{node,other};var graph=new GraphSnapshot(list);list.Clear();Check(graph.Nodes.Count==2&&graph.Nodes[0].StoredTableIndex==9&&graph.Nodes[1].StoredTableIndex==2,"Order and stored index conflated");
 Check(graph.Nodes[0].Identifier==graph.Nodes[1].Identifier&&graph.Nodes[1].StoredInstanceGuid=="malformed-preserve-me","Labels/identity coerced");Check(graph.Nodes[1].Version.Major==99&&graph.Nodes[1].TypeGuid==Guid.Empty,"Unknown metadata lost");
 Throws<NotSupportedException>(()=>((IList<GraphNodeSnapshot>)graph.Nodes)[0]=other);Throws<NotSupportedException>(()=>((IList<SerializedChannelLink>)node.InputLinks)[0]=new(1,0));
 Throws<ArgumentNullException>(()=>new GraphSnapshot(null!));Throws<ArgumentException>(()=>new GraphSnapshot(new GraphNodeSnapshot[]{null!}));
 return identityResult+"; "+undoResult+"; PASS graph snapshot contracts: scoped parameter addresses, sentinel/header/order fidelity, opaque/list isolation, legacy/unknown identity and null guards; no codec or geometry claim.";
 }
}
