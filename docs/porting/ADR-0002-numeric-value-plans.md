# ADR 0002: immutable numeric edit boundary states

Status: accepted for P03-T03-A04.

NumericValueState stores the current and previous scalar without units, validation bounds or UI semantics. PlanSetValue derives the strict epsilon decision from recovered CSValue.SetValue. NumericValueEditPlan exposes Before, optional AtPreChange and After. RequiresChange means the source setter would invoke its change path; it is not inferred from numeric equality. Equal infinities or NaNs can take that path. A no-op preserves the original previous value and both double bit patterns.

This adapts the scalar method into a pure immutable contract; it is not an applied edit. Native PreChange records undo state after previous was assigned old current. Change then marks UIChanged and ObjectLastChanged in the recovered graph. The plan captures the scalar states at those boundaries without claiming to implement those effects, exception ordering, graph invalidation or undo transactions. Before is not a substitute for an original undo capture, and a plan is not an undo record. CSValue.SetUndo also uses setters and CAM fields; it cannot be replaced by a naive state swap.

CSValueRange inherits SetValue and this operation does not clamp or quantize. Its UI SetValueFromUI has a separate application tolerance, transaction lifetime, graph generation and redraw sequence. CSValue.Update can also assign an override directly. Consumers must not apply this plan indiscriminately to every graph write, override, UI gesture or import. Field/type/version and UI policy remain separate explicit responsibilities.

The regression oracle embeds the unchanged recovered SetValue body in a test-only class. Native callbacks are replaced by observation-only recordings. verify-numeric-oracle.cjs requires the canonical file hash and matching method text after LF normalization. Six controls reject a modified epsilon, changed canonical input, missing or duplicated method and accept intended line-ending conversion. Domain.Contracts compares 909 before/prechange/after bit-pattern cases and uses explicit boundary expectations. This is a recovered-method control-flow comparison, not original PC/native execution.

Domain has no new platform dependency. Graph/document commands can consume the state plan later under a real parameter address. Persistent identity, units, field codecs, live callback failures, transaction grouping, geometry and PC save/reopen remain incomplete.
