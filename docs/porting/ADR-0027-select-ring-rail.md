# ADR0027: known SelectRingRail forwarding

Status: experimental candidate, local native/source checks pass; Apple validation required.

CSSelectRingRail1.0 is present as node16, [4,13,15]/[0,0,0], using the inherited payload-free common header. Admit its exact GUID/version through the existing structural codec; future versions stay opaque. The inventory becomes109 known headers/132 opaque, with100 scalar handles unchanged.

Update reads the control as Boolean defaultfalse, then both raw CSData inputs. Both null -> null; one null -> other; otherwise the Boolean selects. CSInputChannel.Data returns null for a disconnected block, whereas inactive scalar/invalid CSBlock outputs return an actual empty CSData. LegacyChannelValue.Missing explicitly represents null; Empty continues to represent data with no supported conversion. Conversion failures preserve ref defaults. The chooser with no source still returns Empty as the source does.

The candidate provider forwards every supported kind, including numbers/profiles/empty/missing despite the constructor Curve filter: source SetData performs no type filter. Curve outputs receive a retained native alias plus their immutable rail metadata. A selector curve token addresses the selector cache, so rebuilding only an upstream rail cannot silently change the cached selection. Readers retained before replacement or disposal remain owned independently. Noncurve entries own no native handle and curve/metadata getters reject them.

Source Update is extracted verbatim into a test-only channel observation harness.150 graph cases compare null/empty/number/profile/native data and Boolean threshold/NaN/infinity against it; reversed choice and null-test source tampering are rejected. Actual project selection, source0 initial control, edit/save/reopen, native downstream consumer, old-output lifetime, cycle rejection and zero native-entry cleanup are separate assertions. The independent Node reader fully consumes the selector frame; managed tests compare all common fields/links and preserve bytes/future schemas.

No new native algorithm or downloaded dependency. Known dependency preflight still traverses both alternatives, including invalid output indices, and rejects unsupported producers/cycles explicitly. This is not original dirty/parallel/error/material/display/export scheduling or source alias/disposal equivalence. Original PC, full-design, physical-device and complete application parity remain unverified; no full parity row closes.
