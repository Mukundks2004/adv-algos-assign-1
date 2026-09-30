# Implementation Design

So we know fundamentally that a lot of the operations that we are going to be doing will depend on matrices, which will be represented as 2D arrays. But the full interface (literally) of matrix operations requires division- this means entries for the matrix, if they are integers, will lose information as they are converted to floats when we do things like find inverses.

Solution? Use a dedicated fraction class to preserve information.

Since the fraction class and the matrix class both share operations, we can use a shared interface- one that includes the add, subtract and multiply operations. Call this `IRing`. There might be some kind of tricky self referential definition required.

`Fraction` also has the `/` operation, which means it implements `IField` that inherits from `IRing`.

Then implement all of these classes.
