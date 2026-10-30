# P05-T02-A07 source-derived ring rail subcomponent

Goal: execute the inspected RingRail_V1 radius/default/ZX geometry plan using actual candidate NURBS curves, including edit/save/reopen/rebuild of the available stored radius. Generic curve native and CSharp/Windows tests passed; Apple generic proof is still live. This is a subcomponent experiment, not full CSBlock.Update, channel evaluation or P06 design/PC parity.

Create Features.Candidate project referencing Geometry.Candidate. RingRailV1Candidate.Build(context,radius=7) owns a native Curve and exact Radius metadata. It uses only explicit ZX plane and genuine native creation; invalid requests propagate typed native errors. No managed synthetic curve, cached preview, imported Rhino DLL, activation or server.

- [ ] RED: managed contract uses missing Features.Candidate builder; observe missing project/type failure before implementation.
- [ ] Implement source-derived input plan plus owned output; clean native shape if wrapper allocation fails. Do not emulate unobserved channel GetDouble/IsOn/override/default failures or CSRingRailData copy/export/user-string/UI lifecycle.
- [ ] Embed existing library fixture/reference link oracle in test assembly. Assert rail4 points to source2/channel0 and RingSize1.2 stored-radius units. Supply the recorded scalar explicitly (not an evaluated channel), build real curve, edit radius +.5 through existing100-handle session, save/reopen, rebuild and inspect length/seam/degree/poles; undo restores original bytes and geometry input.
- [ ] Run real Windows native/DLL contracts with cached D: SDK, shared regressions and Mac native/simulator/device workflows. Keep source and output hashes; no original-PC equivalence claim.

Owned: src/Port/Features.Candidate; tests/Port/Geometry.Candidate.Contracts references/new RingRailAssertions and fixtures; Native workflow filters; P05-T02 evidence/ADR0011. Native API stays unchanged.
