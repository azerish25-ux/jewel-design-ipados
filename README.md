# Jewel Design — offline iPadOS / iOS port

Recovered CounterSketch code, reproducible portability experiments and an implementation roadmap. **This is not the finished iPad application.**

## Start here

1. [Agent entry guide](docs/porting/START_HERE.md) and [verified current state](docs/porting/CURRENT_STATE.md).
2. [16-phase roadmap](docs/porting/ROADMAP.md): 96 work cards with dependencies and acceptance gates.
3. [Recovery map](recovery/README.md): canonical source, variants, resources and provenance.
4. Read [AGENTS.md](AGENTS.md) and [build instructions](docs/porting/BUILDING.md) before editing.

| Location                                 | Purpose                                                           |
| ---------------------------------------- | ----------------------------------------------------------------- |
| [recovery/canonical](recovery/canonical) | Primary desktop/CAD reference source                              |
| [recovery/support](recovery/support)     | Nine supporting assemblies                                        |
| [recovery/variants](recovery/variants)   | Alternate/partial recoveries and 113 editable XAML files          |
| [recovery/resources](recovery/resources) | 744 unique objects mapped to original semantic filenames          |
| [recovery/manifests](recovery/manifests) | Complete source/resource/hash/redaction records                   |
| [src/Recovered](src/Recovered)           | Hash-locked unchanged-source portability experiment               |
| [src/Port](src/Port) / [native](native)  | Planned production implementation boundaries                      |
| [docs/porting](docs/porting)             | Agent protocol, blockers and 1,451 granular requirement/type rows |
| [docs/evidence](docs/evidence)           | Source-backed audits and measured results                         |
| [reference](reference)                   | Independent legacy CSG file-format proof                          |

See [available production domain helpers](docs/porting/DOMAIN_HELPERS.md) for the adopted material and ring-size APIs and their limits.

## What is present

10,405 text/source/project/markup files, including 10,238 CSharp files across canonical/support/variants. Duplicates are not additional features. Canonical main baseline: 2,783 files / 441,299 nonblank lines. All 2,241 binary resource paths map to 744 unique objects. Remote credentials in 31 files are explicitly redacted; local originals are unchanged.

The ledgers cover 68 UI groups, 52 CAD groups, 58 PC requirements, 654 serializer identities and 619 builders. Serializer/builder catalogs overlap. Evidence includes an unsigned ios-arm64 Release/Mono AOT build, signed iPad simulator math/domain execution and a limited CSG edit independently reread by CSharp. These are not full CAD/UI/PC-project parity. A large-input compression defect remains open.

## Quick commands

    node scripts/recovery.cjs summary
    node scripts/recovery.cjs find CSValueRange
    node scripts/porting.cjs next
    node scripts/porting.cjs show P01
    node tools/repository/check.cjs

Windows SDK, cache, auth and temp files stay under the D: repository:

    node scripts/bootstrap-dotnet.cjs
    node scripts/run-local.cjs dotnet run --project tests/Console
    node scripts/run-local.cjs node reference/file-proof/run-file-proof.cjs
    node scripts/run-local.cjs node reference/file-proof/run-independent-check.cjs

Do not build every archived csproj or install the original software on the host. Read [BUILDING](docs/porting/BUILDING.md) and [security/provenance](docs/porting/SECURITY_AND_PROVENANCE.md).

## Completion policy

Full parity needs real on-device geometry, editable PC documents, complete touch workflows, local rendering/data and physical-device evidence. Compilation, classes, mocks, static panels and cached previews cannot close parity rows. No reliable new-code LOC minimum is established; historical 500k–1M allowances were withdrawn as requirements.
