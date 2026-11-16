# A05-P02 concrete SetView preflight and dependency mapping

Port the original633-byte input-validation helper after inspecting the virtual116-byte ON_Viewport::IsValid. The virtual call tests camera/frustum/port flags. The helper then checks all location/direction/up/basis components and six frustum scalars with original scalar validity, absolute width/height spans within inclusive1e-18..1e18, and near>=2^-32 for projection2. Do not normalize frame vectors, sort bounds, impose projection enums or extra near/far rules absent from source.

Ground thresholds in original file-backed doubles and preserve bit-level boundary cases. This preflight is a real SetView prerequisite, not a complete restore adapter. Actual SetView must preserve existing viewport UUID via display-conduit unbind/change/rebind, parent page/model/detail rules, page/position/cplane choice, names/images/display settings, serials and document effects. The378-byte UUID helper is not merely a property setter. Resolve its collection/callback semantics before implementation.

RED before code; Windows/native/adjacent, macOS and actual iPad/iPhone after implementation. All local files/tool caches/reports D-only, transient I/O waits five seconds; connected owner GitHub plugin for remote operations, no shared CLI. No full Pop/SetView/UI/PC/physical closure from boolean validation.
