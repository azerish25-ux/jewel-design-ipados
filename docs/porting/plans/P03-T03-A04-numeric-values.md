# P03-T03-A04 numeric value state implementation plan

> Execute sequentially with the existing P03 authorization and the test-driven-development and verification-before-completion skills. No delegation.

Goal: preserve CSValue.SetValue numeric decisions and observable state order as an immutable plan before native undo/UI integration.

Architecture: Domain owns NumericValueState and NumericValueEditPlan. PlanSetValue returns before, optional state at PreChange, and after. It does not call a host, perform undo, clamp or convert units. Whole-class copying is blocked by Rhino/channels/undo globals; a whole undo rewrite is premature. The scalar state contract is independently usable by future graph/document commands.

Tech stack: C# net10.0; Domain.Contracts, Console and existing iPad/iPhone simulator harness; Node source-body verification.

Prerequisites: P03-T01/T02 complete; P03-T03-A02 verified. General document units and persistent identity remain parent blockers; this child does not require or resolve them.

Source: recovery/canonical/cad/CS.CSValues/CSValue.cs SHA256 a7ccbf583e3576daf067140b5af52b21941bfc316e905cfebb48402b0ece6210. SetValue returns true without mutation when Math.Abs(delta)<1E-05. Otherwise previous is set to old current before PreChange, then current is assigned before Change. No range/step/finite guard occurs here. UI gesture tolerance is separate.

Owned files: src/Port/Domain/NumericValueState.cs; tests/Port/Domain.Contracts/NumericValueAssertions.cs and Recovered/LegacyNumericValue.cs; scripts/verify-numeric-oracle.cjs; existing shared runners, production verifier, iOS workflow, classification/evidence.

Acceptance: P03-A1 no Windows/Rhino dependencies; P03-A2 normal values, exact epsilon boundaries, prior-value preservation, out-of-range requests, signed zeros, infinities and payload NaNs. Compare double bits before/prechange/after with an unchanged original-method body in a test-only instrumented harness. Verify that body against canonical source. This is recovered-method control-flow evidence, not native PC execution or callback/undo integration. New production code is adapted, not unchanged recovered LOC.

- [x] Write shared assertions and canonical-method harness; run Domain.Tests RED for missing API.
- [x] Implement immutable state/plan; preserve no-op fields and change notifications even for equal-looking nonfinite values.
- [x] Run Domain.Tests, Graph.Tests, Console, source/body/hash and repository checks; review aliasing, dependencies and scope.
- [x] Commit/push implementation; inspect macOS iPad/iPhone artifacts before claiming simulator verification.
- [x] Commit/push evidence and remaining work. P03-T03 stays incomplete; no codec, geometry or PC roundtrip claim.
