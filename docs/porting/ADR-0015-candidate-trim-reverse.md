# ADR0015: copied candidate NURBS trim and reverse

Status: actual Windows native/CSharp verification passed; bounded Apple runtime/device proof recorded in [trim/reverse results](../evidence/apple/native-trim-reverse-results.json).

Trim accepts finite increasing intervals within current edge domain. The located NURBS geometry is deep-copied and actually segmented, then wrapped in a newly owned native edge. Reverse first restricts a copied curve to its edge interval and reverses its geometry/parameter mapping; it does not merely toggle topology orientation. Native failures propagate with zero output handles.

The open quarter-arc regression exposed default length quadrature error: actual10.995575914690008 versus analytic10.995574287564276. Curve inspection now requests explicit native1e-10 integration tolerance; the strict1e-8 test was retained. This is candidate inspection accuracy, not proof of Rhino tolerance policy.

Tests pass actual quarter length/domain/endpoints, reverse mapping, source preservation, bounded closest queries beyond both open ends, invalid intervals and earlier native/managed/ring cases. Source caller extrapolation, periodic/seam/join/intersection behavior, complete OutsideRail_V1, PC/physical/cancellation/process-memory parity remain incomplete.
