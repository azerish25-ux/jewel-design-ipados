# P06 — first real offline edit/save/reopen

## Selection rule

Do not choose an arbitrary toy mesh. Use a PC-created fixture with a supported real recovered builder, non-default inputs and known dependency closure. Record its PC build, full input hash, builder GUID/version, linked nodes, document units/tolerance, material IDs and exact edit sequence. The library sample alone is insufficient unless Windows behavior is captured and its builder closure is ported.

## Test before implementation

Freeze expected graph/parameter state, geometry/topology/volume and a fixed-camera image. Define one valid non-default edit, one boundary edit and one invalid edit. Confirm how Windows handles each, including undo, failure diagnostics and whether invalid geometry changes the saved graph.

## Required sequence

1. Open the unmodified PC project on iPad offline through the actual codec.
2. Display actual generated geometry from the selected kernel, not a retained mesh/preview.
3. Change the named parameter and execute the adapted original builder.
4. Compare graph identity/values and generated geometry separately to the Windows oracle.
5. Undo and redo; verify both states and output geometry.
6. Save atomically; force-quit; reopen offline; verify the edited document.
7. Transfer the saved files/sidecars to the original PC app and reopen/regenerate them.
8. Confirm parameters remain editable and exports preserve units, object inclusion and dimensions.

Deliver: fixture/oracle bundle; adapted source provenance; real codec/kernel/UI integration; normal/boundary/failure results; measurements of retained, adapted and newly written code; review. Only after this should implementation estimates be narrowed. Screenshots, a binary value patch or a registered class cannot satisfy the sequence.
