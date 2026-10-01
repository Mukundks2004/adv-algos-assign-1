# gauss-jordan-formalization

This codebase has a lean formalization of the Gauss Jordan elimination algorithm and the reduced row echelon form (RREF), mirroring the C# implementation.

Matrices are `Matrix (Fin m) (Fin n) ℚ` (abbreviated `Mat m n`), so matrix dimensions are carried in the type (unlike in the more basic C# implementation)

Here are the contents of each file:

- `Defs.lean`: elementary operations, solution sets
- `RowOps.lean`: that row operations preserve row equivalence and solutions
- `Structure.lean`: The four RREF structural properties- that zero rows are at the bottom, all pivots are 1, pivots move strictly rightward, and a pivot column is 0 everywhere else
- `Algorithm.lean`: The algorithm, pivot step, and cases of guaranteed termination
- `Invariant.lean`: The loop invariant- a single theorem that encodes all the invariants
- `Uniqueness.lean`: That RREF is unique
- `Correctness.lean`: That the output of the algorithm is the unique RREF of the input
- `Solutions.lean`: Extra stuff that uses correctness, like row augmented matrices have equal soltions etc

Some of these have been implemented by me. Some have been implemented by AI. Some are still sorry.

Find remaining work with:

```
lake build 2>&1 | Select-String "uses .sorry."
```

## Building

```
lake build
```


