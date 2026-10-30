# Start here

Goal: complete offline CounterSketch-equivalent iPadOS/iOS behavior and editable PC-project compatibility. This is the recovered reference and work queue, not a finished app.

1. Read [CURRENT_STATE](CURRENT_STATE.md), [ARCHITECTURE](ARCHITECTURE.md) and [BLOCKERS](BLOCKERS.md).
2. Run node scripts/porting.cjs next and read the selected phase and ready task.
3. Use node scripts/porting.cjs backlog P07 10 and node scripts/recovery.cjs find CSValueRange to inspect bounded evidence.
4. Run node tools/repository/check.cjs. Windows cache-producing commands use scripts/run-local.cjs.
5. Select one independently testable work card. Define normal, boundary and failure cases and record the current failure before implementation.

[ROADMAP](ROADMAP.md) has 16 phases and 96 work cards. [AGENT_PLAYBOOK](AGENT_PLAYBOOK.md) defines execution and review. [Backlogs](backlog) contain all audited requirement/type rows. [Recovery map](../../recovery/README.md) identifies canonical code, variants and assets. [Build instructions](BUILDING.md) give executable commands and output paths. [Audit index](../evidence/audit/README.md) retains source-backed scope and limitations.

P01 still has differential/compression and wider device coverage work. P02 requires a contained licensed Windows reference and real project corpus. Missing external prerequisites must not be fabricated or prevent independent work.
