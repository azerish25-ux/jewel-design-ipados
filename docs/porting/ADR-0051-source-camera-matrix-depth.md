# ADR0051: double projective camera matrices and explicit Metal depth mapping

Status: local compiled-source and independent endpoint tests pass; Apple/compiler/platform checks pending.

CandidateCameraTransform stores all16 double row-major coefficients. The affine document DTO deliberately fixes its last row and cannot represent perspective projection. WorldToCamera/CameraToWorld use supplied basis/translation directly, preserving published source behavior rather than introducing orthogonalization. CameraToClip retains prior state on exactly equal paired bounds; other raw invalid inputs follow source math and may produce nonfinite coefficients.

The GPU adapter is explicit and separate from numerical source semantics. Source near/far maps to clip z/w=+1/-1. Metal conversion computes homogeneous z_m=(w_s-z_s)/2, then transposes into System.Numerics row-vector storage so shader column-major interpretation recovers the intended transform. Nonfinite and float-overflow coefficients reject packing. No invented replacement matrix is returned as successful output.

The native reference compiles exact pinned openNURBS methods with observation point/vector/matrix containers.118 cases compare all16 coefficients, retained-failure state and raw nonfinite behavior.16 independent perspective/parallel corner/near/far endpoints plus behind-camera and asymmetric packing checks guard depth/transposition separately. The reference is not a private Rhino5/native viewport or GPU implementation.

Camera frame construction/normalization, lens-derived framing, perspective/parallel transitions, mesh world-center normalization, source preset application and actual Metal preset frames remain separate integration requirements. Original-PC/private/physical acceptance is open. [Plan](plans/P08-T01-A05-camera-matrices.md).
