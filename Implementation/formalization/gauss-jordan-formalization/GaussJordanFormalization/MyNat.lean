import Mathlib.Tactic

/-!
# MyNat

A hand-rolled copy of the natural numbers, used purely as a smoke test that the
project is wired up correctly: it checks that custom inductive types, recursive
definitions, `simp` lemmas and Mathlib tactics all work.
-/

namespace GaussJordanFormalization

/-- The natural numbers, defined from scratch. -/
inductive MyNat where
  | zero : MyNat
  | succ : MyNat → MyNat
  deriving Repr, DecidableEq

namespace MyNat

/-- Addition by recursion on the second argument. -/
def add : MyNat → MyNat → MyNat
  | m, .zero => m
  | m, .succ n => .succ (add m n)

instance : Add MyNat := ⟨add⟩

instance : Zero MyNat := ⟨MyNat.zero⟩

@[simp]
theorem add_zero (m : MyNat) : m + 0 = m := rfl

@[simp]
theorem add_succ (m n : MyNat) : m + succ n = succ (m + n) := rfl

@[simp]
theorem zero_add (n : MyNat) : 0 + n = n := by
  induction n with
  | zero => rfl
  | succ n ih => simp [ih]

theorem succ_add (m n : MyNat) : succ m + n = succ (m + n) := by
  induction n with
  | zero => rfl
  | succ n ih => simp [ih]

theorem add_comm (m n : MyNat) : m + n = n + m := by
  induction n with
  | zero => simp
  | succ n ih => simp [ih, succ_add]

theorem add_assoc (m n k : MyNat) : m + n + k = m + (n + k) := by
  induction k with
  | zero => simp
  | succ k ih => simp [ih]

/-- Convert to Lean's built-in `Nat`, so results can be compared with Mathlib's. -/
def toNat : MyNat → Nat
  | .zero => 0
  | .succ n => n.toNat + 1

@[simp]
theorem toNat_zero : toNat 0 = 0 := rfl

theorem toNat_add (m n : MyNat) : (m + n).toNat = m.toNat + n.toNat := by
  induction n with
  | zero => simp
  | succ n ih => simp [toNat, ih, Nat.add_assoc]

end MyNat

end GaussJordanFormalization
