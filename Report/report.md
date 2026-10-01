# Advanced Algorithms Assignment 1 Report

Name: Mukund S

ID: 24832432

Algorithm: Gauss-Jordan Elimination

Track: B

# What Did I Build

I built multiple solvers that accept a system of linear equations and solve them for each variable.

I built this in the language C# because I am very comfortable programming in that language, and because C# has good contract support especially for parametric polymorphism, co/contravariance and other tricky type level constructs that mathematical solvers benefit from.

For example, solvers often work best over mathematical fields (such as the field of rationals or reals) and so developers often want to constrain logic at the type level to avoid loss of information like dodgy floating point arithmetic that may give incorrect answers.

Data representation is complex, since I chose to use a nice type safe and very abstract implementation rather than a fast but janky C solution (maybe I'll do this next time). But in essence, a system of linear equations is mathematically encoded as an augmented matrix. This is an $n \times m$ grid over a field $\mathbb{F}$ whose rows represent equations and whose columns represent coefficients of independent variables, joined to a matrix of width $1$ whose rows are constants. This is represented as a 2D array of doubles for the demo, but is a generic (type indexed by a type) for developer convenience.

The signature of the main solver algorithm is:

```cs
public (T[,] Result, List<SolverStep<T>> Steps) SolveWithSteps<T>(T[,] matrix) where T : class, IMatrixEntry<T>, new()
```

It looks a bit intimidating, but all it does is take a 2D array and return a 2D array (with some extra stuff).

In terms of how similar this algorithm is to the textbook version, it is pretty spot on, there is little variation in functionality. However, as mentioned the types are slightly more complex for developer convenience and safety.

The code is fairly organized, I did most of the high level planning and low level implementation by hand so the organization is fairly textbook and intuitive.

- The code the marker is most interested in is located in `Implementation\GaussJordanElim\GaussJordanElim`
- `Abstractions` contains the interfaces for the matrix elements, this is used to control the domain of discourse- what types of quantities can be considered solutions
- `MatrixEntities` are concrete implementations of the above
- `Solvers` is the most interesting, it has the code for the actual algorithms
- `Utils` has some utils

The core algorithm is located in the file `Implementation\GaussJordanElim\GaussJordanElim\Solvers\GaussJordanSolver.cs`

# Track Specific Writeup

As I chose track B, the majority of this writeup will focus on the use of the Gauss-Jordan algorithm to solve magic squares.

However, before that I want to briefly mention my progress with tracks A and C.

## Track A: Gauss Jordan vs LU Decomposition

For track A I implemented another more commonly used linear equation solver, the LU Decomposition method via Doolittle's Algorithm. This is a direct competitor to Gauss-Jordan (GJ). You can find the implementation in `\Implementation\GaussJordanElim\GaussJordanElim\Solvers\LuSolver.cs`. Unfortunately, both algorithms are highly similar to the point where an empirical study comparing them is not very interesting. For this reason, I did not investigate very deeply. One notable difference, is that LU only needs to calulate the lower and upper matrices once, and these can then be reused to do the lightning fast forward and back substitution with different 'constants vectors'. However, for GJ, the whole elimination process needs to be redone if the constants vector changes.

## Track C: Formal Verification with Lean4 and Mathlib

I have some experience with formal verification in both Idris and Agda, and I've played the lean natural numbers game so I thought I would try my hand at actual theorem proving. Unfortunately, it was way harder than I thought and I did not get very far. You can see my progress in `\Implementation\formalization\gauss-jordan-formalization`.

What I did get done:

- Planned out what definitions I need
- Planned out what kind of variants and properties I want to prove
- Organized this into files (you can see this in the `README.md`)
- Wrote out some of the definitions (about half) and AI generated the rest
- Went through all the definitions to confirm I understood them

At this stage I asked the AI to generate the headers for the other sections I had outlined, since I was not experienced enough to write them myself, but the result was thousands of lines long (just the headers!) and impossible to read, so I decided to submit my own work and abandoned this.

## Track B: The Gauss Jordan Algorithm for Solving Magic Squares

The tool I have chosen to build is a magic square solver.

![interface](/Report/Resources/image.png)

You can build and run it by following the instructions in the README at the root of the repo.

What it does is solve magic squares.

![Magic square](/Report/Resources/image2.png)

A magic square is a 2D array of numbers, usually integers, arranged such that the sum of every row column and diagonal add to some unknown quantity that is called the magic constant- this is a constant. So given any row, any diagonal, etc, they all add to this sum. In the above this is $15$.

So naturally we must ask, when only some information is provided, is a solution calculable?

In some cases this is pretty straightforward:

![Simple Square](/Report/Resources/simple.png)

It is reasonably simple to see how to go about solving the above square. But in other cases not so much.

![Complex](/Report/Resources/complex.png)

Does the above square have one solution? Many? None?

And of course, we can get bigger magic squares too...

![Big](/Report/Resources/big.png)

It turns out, solving squares in the genearl case is not an impossible problem- it could be tedious for a human but perfect for a computer. Any of these systems of equations can be tackled algebraically, you just have to give every blank a variable name and write out your system of equations and start substituting and solving.

## How Does the Algorithm Sit Inside It

The algorithm to use here is called Gauss-Jordan Elimination, if you represent a system of equations as a matrix, GJ will manipulate the matrix in a way that keeps it consistent mathematically, but simplifies it for humans, the end result is a computer friendly result that is easily readable by a computer as "x = 1, y = 2, ...".

The process to get from the UI to the algorithm is a bit lengthy. Essentially, the UI takes a magic square with some cells filled and some empty, then on "solve" it pads all empty cells with nulls and sends it to the backend via a http request. The backend instantiates the right solver via a factory.

This solver formats it the following way:

![AugmentedMatrix](/Report/Resources/aug.png)

The Gauss Jordan algorithm is used to reduce it to RREF:

![RREF](/Report/Resources/rref.png)

From which the results can be read off one by one and input back into the square matrix to be presented as a solution.

The steps are also saved and presented to the user, for them to scroll forwards and backwards to see how the state of the square changes over time.

## Interface Decisions

I had the following requirements:

- user must be able to enter any value in the cell
- user must be able to see the steps of the algorithm to understand that GJ is happening

These minimal requirements made it pretty straightforward to invoke the tool because the input hardly has to be formatted after it is taken from the user, so there is fundamentally not much connecting that needs to be done in the first place.

HTTP WebApis are pretty basic.

I still tried to make the interface nice to use, and added an 'i' in the top right for information.

## Worked Examples

The maths is pretty intensive to actually do any of the operations, but I thought I could take screenshots of an example output since I was smart and put the actual algorithm steps in the UI.

Entering the data:

![DataEntry](/Report/Resources/Practice/image.png)

![CoefficientMatrix](/Report/Resources/Practice/coeff.png)

![Condensed](/Report/Resources/Practice/cond.png)

![Appended](/Report/Resources/Practice/app.png)

![Swap](/Report/Resources/Practice/swap.png)

![Normalize](/Report/Resources/Practice/norm.png)

![Eliminate](/Report/Resources/Practice/elim.png)

Repeat swap, normalize and eliminate 10 more times.

![Final](/Report/Resources/Practice/fin.png)

![Copy](/Report/Resources/Practice/copy.png)

# What I Learned

## What surprised you?

Most of the standard stuff in this assignment (implementing the algo and learning the language) didn't surprise me. I deliberately picked an algorithm I had heard of before, and a language I knew how to program in. The lean stuff took some getting used to- I didn't expect mathlib to be so big, it took almost 2 hours to download and type check on my poor quality laptop.

I didn't expect C# contracts to support me as much as they did, I remember attempting something similar a few years ago- a computational algebra library, and every step was painful because I kept trying to abstract away things and lock them in at the type level but C# didn't have enough support for it. Since then they've added default implementations in interfaces, static abstract members for interfaces, operators in interfaces, all of this made writing and editing contracts so much easier. When I was able to do a perfect polymorphic swap between the `Fraction` class and the `Real` class when I went from testing my own algorithm to the UI, it took 0 effort which was nice.

I was surprised by how crappy the copilot line autocomplete is. I had to switch that off within 10 minutes of starting development on a new laptop.

I was surprised that noone had attempted this approach to solving magic squares before. In fact, most people don't really appreciate the depth of the problem, and restrict magic squares to only 3x3, or only having fixed digits (from 1 to $n^2$), or being overdetermined to a point where solving them is either easy or impossible but never in between. When I googled for this on the internet after completing my implementation I couldn't find any other hits.

![BadExample](/Report/Resources/badex.png)

For example, even this website, the top hit, requires the magic constant to solve the square. Other websites are no better.

The first time I heard about this algorithm, in discrete maths in my first year, I was surprised such a thing was possible! I'm happy I've progressed to a point where I can understand how it works and implement it.

In fact, what surprises me today is that there is no faster method (excluding the ones that approximate). Both this algorithm, and the only other comparable one (LU decomposition) are on the order of $n^3$ for size $n$ in the implementation that I've made, and while they can be improved they can't be improved by much.

Larger systems of linear equations, such as those solved by NESA to calculate how to scale schools in NSW for the HSC, are numerically approximated by other algorithms such as GMRES.

## What did you initially get wrong?

I messed up the contracts, loads of times.

![Contract](/Report/Resources/contract.png)

You can see in the git blame I've edited it at least 6 times. In reality it was probably more like 30 or 40 times.

Requirements change, and how I think something is about to work changes.

On my first go through I only included add, subtract, and multiply. Then I changed the members to make them static. Then I made them operators. Then I moved `Zero` and `One` in here from a different interface. I had `IRing` inherit from `Cloneable`, which was changed to a custom `IDeepCloneable<T>` at one point. The non type constraints, such as `new` and `class` came at differnt times when I realized the GJ algorithm needed this or the magic square algorithm needed that.

I made tons of low level mistakes. For example, in the following code:

```cs
int candidateNonZeroCellRowIndex = rowToGiveLeading1Index;
while (matrix[candidateNonZeroCellRowIndex, pivotColumnIndex].IsZero())
{
	candidateNonZeroCellRowIndex++;
	if (candidateNonZeroCellRowIndex == rowCount)
	{
		candidateNonZeroCellRowIndex++; // <- this line was wrong
		pivotColumnIndex++;

		if (pivotColumnIndex == colCount)
		{
			pivotColumnIndex--;
			break;
		}
	}
}
```

I made a mistake, incrementing a variable when I should have set it to `rowToGiveLeading1Index;`.

I think my variable names, as much as I tried to be incredibly specific about what they were, were too long and often made it hard to read my own code quickly as the codebase got bigger. That contributed to how long it took me to find the above bug.

I didn't scope the requirements of the project accurately enough at the beginning which is why I had to change the interface a million times and waste time on refactors.

I really should have had more helpers. It would have helped to have so many helpers, in the form of a fluent API or extension methods, to have so much logic tucked away in methods that accurately describe what they do from a _business_ perspective that reading the code is a non technical activity. For example,

```cs
int rowToBeSwappedWithCurrentRowToGetLeading1Index = candidateNonZeroCellRowIndex;
for (int columnIndex = 0; columnIndex < colCount; columnIndex++)
{
	(matrix[rowToBeSwappedWithCurrentRowToGetLeading1Index, columnIndex], matrix[rowToGiveLeading1Index, columnIndex]) =
		(matrix[rowToGiveLeading1Index, columnIndex], matrix[rowToBeSwappedWithCurrentRowToGetLeading1Index, columnIndex]);
}
```

This is a swap rows procedure, I only used it once or twice so I didn't need to put it into a function for the sake of DRY, but if I had encapsulated it in a `Swap()` method, it would be clear from a high level perspective that this code was performing an elementary row operation; 5 or 6 long lines becomes a single function call. This really only occured to me when I was doing the lean definitions and I had to be extremely specific and low level in translating code constructs into business objects.

## What is something you understand now that you didn't before starting?

The biggest one is how the doolittle algorithm (part of LU solver) works, coming in I already knew GJ fairly well but learning doolittle/LU gave me a sense of scale. Now I know that forward/back substitution is a standard part of solving algorithms, and is common between Cholesky decomposition, QR decomposition, etc.

Learning LU was good from an intuition perspective, because it is an algorithm traditionally executed via pen and paper in exams since it relies on a trick that hides how it works (similar to integration by parts, or the chain rule in maths) which is writing to L while eliminating in U, while preserving $L \cdot U = A$.

> Specific examples carry far more weight than general statements. A sentence like "I eas surprised that the Fibonacci heap was slower than std::priority_queue on every workload I tried, and I now believe this is because of cache behaviour" is the kind of content that is being looked for.

### AI use

The AI use section must address the following:

- Which tools were used and roughly how much were they used. The example given is "Claude was used for the implementation and ChatGPT was used for the matplotlib API", this amount of detail is good.
- What did I use them for? For example, scaffolding the implementation, debugging, writing tests, generating plot code, drafting parts of the report, explaining concepts I didn't understand and so on
- At least two specific examples of where the AI was wrong, unhelpful, or misleading- and then, what I did about it. For example, the API may be hallucinated, the implementation may be wrong or buggy, an explanation might be wrong or even misleading, a benchmark might not measure what it aims to, or any of the other ways AIs can go wrong.
  - Every student that uses these tools will encounter such cases. If you can't think of any, you probably didn't use the tools enough- or used them without checking the output, which is the failure mode these tests are designed to catch.
- What was understood truly when the output was generated vs what was taken on trust. Be honest about parts of the code or analysis where we are not fully sure what the AI produced is correct. This is not penalized, this is expected. What is penalized is claiming to understand something we didn't.

The goal here is honest engagement, not performance. For another example of a student who is doing what the assignment is asking for, consider the comment: "I used claude heavily, it produced a working implementation in an hour, but then I spent ten hours benchmarking and discovered the implementation had a subtle off-by-one in the merge operation that only showed up on certain inputs".

```
A linear equation is an expression of the form $k_1x_1 + k_2x_2 + k_3x_3 + ... + k_nx_n = y$. This expression is dependent on the variables $x_i$ but is linear in every variable, making it a linear equation. Linear equations are very important since many relationships in the real world are linear- for example, the relationship between speed and distance travelled over a fixed amount of time is linear.

Often in the real world, there are multiple linear relationships that exist simultaneously as lots of quantities vary together and affect each other. For example, maybe cars have to pay 10 dollars to park and bikes have to pay 5. Both these quantities scale linearly. So the total amount of money the carpark generates is a product (not literally) of multiple linear variables.
```
