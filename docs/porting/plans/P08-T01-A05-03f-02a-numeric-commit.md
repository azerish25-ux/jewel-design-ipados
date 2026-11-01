# P08-T01-A05-03f-02a numeric commit state

Implement the actual numerical state mutation of original SetVP plus view target setter/getter, consuming f-01 matrices. Parent f-02 remains incomplete until whole-view/document/history effects are integrated; this core must not implement the full e-02 commit interface prematurely. No no-op notification/history callbacks.

Own immutable raw viewport/target-mirror/clip-modification/screen-port/extra-transform/cache/serial values. Effective guards represent observed original conditions without inventing semantic native field names. Test rejected guards/invalid incoming flags; lock/symmetry clearing and restoration; raw invalid requested fields with true flags; ignored setter failures; target-distance mode; unchanged incoming target; stale mirror repair and NaN/signed-zero comparisons.

Cache correction from actual GetXform output address: failure resets cached matrix to identity while keeping its stale serial. Cache serial collision after wrap skips matrix AND screen reads. New serial skips zero. Width=right-left and height=bottom-top use unchecked32-bit differences. Invalid screen flag zeroes both dimensions.

RED missing API tests first; GREEN source-ordered numeric core; full host/adjacent/integrity checks, macOS and actual iPad/iPhone. Keep every local write/cache under D: through run-local; five-second transient drive retries. Record exact original hashes, known scope and Apple handles. Numeric bool acceptance does not claim document notification/history completion, full native setter or rendered preset parity.
