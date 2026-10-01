import Mathlib.Data.Matrix.Basic
import Mathlib.Data.Matrix.Mul
import Mathlib.LinearAlgebra.Matrix.RowCol
import Mathlib.Data.Rat.Defs
import Mathlib.Data.Fin.Tuple.Basic
import Mathlib.Algebra.BigOperators.Fin

namespace GaussJordan

open Matrix

/- This file has lots of basic definitions -/

/- A matrix is an array of rationals -/
abbrev Mat (m n : ℕ) := Matrix (Fin m) (Fin n) ℚ

variable {m n : ℕ}

/- Row and pivot definitions -/

def IsZeroRow (A : Mat m n) (i : Fin m) : Prop :=
  ∀ j, A i j = 0

def IsPivot (A : Mat m n) (i : Fin m) (j : Fin n) : Prop :=
  A i j ≠ 0 ∧ ∀ j' : Fin n, j' < j → A i j' = 0

def HasPivot (A : Mat m n) (i : Fin m) : Prop :=
  ∃ j, IsPivot A i j

def IsPivotColumn (A : Mat m n) (j : Fin n) : Prop :=
  ∃ i, IsPivot A i j

/- RREF definition -/

structure IsRREF (A : Mat m n) : Prop where
  zeroRowsAtBottom : ∀ i i' : Fin m, i < i' → IsZeroRow A i → IsZeroRow A i'
  pivotIsOne : ∀ i j, IsPivot A i j → A i j = 1
  pivotsOrdered : ∀ i i' j j', i < i' → IsPivot A i j → IsPivot A i' j' → j < j'
  pivotColumnClean : ∀ i j, IsPivot A i j → ∀ i', i' ≠ i → A i' j = 0


/- These are the elementary row operations, that preserve the system -/

def swapRows (A : Mat m n) (i k : Fin m) : Mat m n :=
  A.submatrix (Equiv.swap i k) id

def scaleRow (A : Mat m n) (i : Fin m) (c : ℚ) : Mat m n :=
  A.updateRow i (c • A i)

def addRowMul (A : Mat m n) (i k : Fin m) (c : ℚ) : Mat m n :=
  A.updateRow i (A i + c • A k)

/- Elementary row operations (EOR) can be one of three kinds -/
inductive ElementaryOp (m : ℕ) where
  | swap (i k : Fin m)
  | scale (i : Fin m) (c : ℚ) (hc : c ≠ 0)
  | addMul (i k : Fin m) (c : ℚ) (hik : i ≠ k)

/- Application has the following effects -/
def ElementaryOp.apply : ElementaryOp m → Mat m n → Mat m n
  | .swap i k, A => swapRows A i k
  | .scale i c _, A => scaleRow A i c
  | .addMul i k c _, A => addRowMul A i k c

/- Inverses have the following effect -/
def ElementaryOp.inv : ElementaryOp m → ElementaryOp m
  | .swap i k => .swap i k
  | .scale i c hc => .scale i c⁻¹ (inv_ne_zero hc)
  | .addMul i k c hik => .addMul i k (-c) hik


/- Reachability- can you reach one matrix from another -/
inductive RowEquivalent {m n : ℕ} : Mat m n → Mat m n → Prop where
  | refl (A : Mat m n) : RowEquivalent A A
  | step {A B : Mat m n} (e : ElementaryOp m) :
      RowEquivalent A B → RowEquivalent A (e.apply B)

/-! Defining solution sets -/

def SolutionSet (A : Mat m n) (b : Fin m → ℚ) : Set (Fin n → ℚ) :=
  {x | A.mulVec x = b}

def AugSolutionSet (A : Mat m (n + 1)) : Set (Fin n → ℚ) :=
  {x | ∀ i, ∑ j : Fin n, A i j.castSucc * x j = A i (Fin.last n)}

end GaussJordan
