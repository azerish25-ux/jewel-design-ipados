# P05-T02-A01 pinned native CAD candidate experiment

Goal: compile and execute genuine OCCT BRep/curve/mesh operations on iOS simulators using a pinned public source candidate. This is an independent backend experiment, not backend selection, CounterSketch builder regeneration or PC geometry parity.

Source candidate: official Open-Cascade-SAS/OCCT V8.0.1, commit b8f597c677811d1f9f4d8a97f5ae2825c0353a42. Third-party source is separate from recovery and excluded from recovered/new port LOC. Local source/cache under .local/native only. Source official CMake/iOS script and API headers before using them. Licensing/distribution and all operation gaps remain decision gates; upload text evidence, not SDK/app binaries.

Recovered requirement evidence: 315 lexical static wrapper/kernel calls plus instance/native operations (not exhaustive semantic coverage). BooleanDifference uses tolerance .01, flips to intersection and falls back to duplicated input on null/empty. Probe output must not claim that caller policy or tolerance equivalence.

Fixture assertions: true cylinder cut annulus with analytic volume and solid validity; overlapping-box common/union and disjoint empty result; rational BSpline circle evaluation/length; native triangulation of actual BRep. Fail on wrong topology/mass/null mesh or native exceptions; no synthetic mesh or cached viewport substitute. Repeated RAII object lifetimes are exercised but cancellation/leak/physical-device/PC oracle remain unverified.

Owned: native/GeometryBridge/CandidateProbe CMake/CPP/header; tests/iOS.Native; scripts/native candidate simulator runner; .github/workflows/native-geometry.yml; candidate pin/source requirement subset/evidence/ADR. Keep candidate self-test entry separate from a production operation ABI with handles.

- [x] Record exact source pin, build flags, native licenses and recovered requirement subset; write analytic fixture tests against documented API.
- [ ] Build/run host and compile/run simulator experiment in GitHub Actions; inspect actual outputs and failures before changing code.
- [ ] Verify iPad/iPhone runtime families and native case results; preserve build/source logs and bounded results.
- [ ] Commit/push all authored progress; keep full P05-T01/T02/T03 and native ABI/cancellation/PC/native archive requirements incomplete.
