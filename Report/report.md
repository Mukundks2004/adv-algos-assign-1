# Advanced Algorithms Assignment 1 Report

Name: Mukund S

ID: 24832432

Algorithm: Gauss-Jordan Elimination

Track: B

# What Did I Build

I built multiple solvers that accept a system of linear equations and solve them for each variable.

In this repo you will find these solvers, a web UI you can use to interface with it, a web API that connects the frontend to the backend, and a partial formalization of one of the solvers.

The core focus of the repo is the solver `GaussJordanSolver<T>`.

I built this in the language C# because I am very comfortable programming in that language, and because C# has good contract support especially for parametric polymorphism, co/contravariance and other tricky type level constructs that mathematical solvers benefit from.

For example, solvers often work best over mathematical fields (such as the field of rationals or reals) and so developers often want to constrain logic at the type level to avoid loss of information like dodgy floating point arithmetic that may give incorrect answers when divisions happen.

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

These minimal requirements made it pretty straightforward to invoke the tool because the input hardly has to be formatted after it is taken from the user, so there is fundamentally not much connecting that needs to be done in the first place. User gives array, take array, give to algo, take result, give to user.

To meet the second criteria, I had the algorithms send back steps in the form of a string description alongside the state of the matrix, that was displayed in a slide format.

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

I think my variable names, as much as I tried to be incredibly specific about what they were, were too long and often made it hard to read my own code quickly as the codebase got bigger. That contributed to how long it took me to find the above bug. Most of the code I write is business logic, so I can use english words for things and they make sense. But when switching to algorithms, all of a sudden there are these weird esoteric rules like "move this counter up until this condition then move it down until this other condition, and use it to index into this array" which is just how algorithms work, but it makes it difficult to name things if you are not used to it. Especially if it is not a "standard" algorithm with widely known jargon.

https://martinfowler.com/bliki/TwoHardThings.html

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

I also made lots of mistakes when programming the frontend/UI, but I feel like that falls beyond what this report is asking about.

## What is something you understand now that you didn't before starting?

The biggest one is how the doolittle algorithm (part of LU solver) works, coming in I already knew GJ fairly well but learning doolittle/LU gave me a sense of scale. Now I know that forward/back substitution is a standard part of solving algorithms, and is common between Cholesky decomposition, QR decomposition, etc.

Learning LU was good from an intuition perspective, because it is an algorithm traditionally executed via pen and paper in exams since it relies on a trick that hides how it works (similar to integration by parts, or the chain rule in maths) which is how writing to L row by row while eliminating in U preserves $L \cdot U = A$. It's a bit hard to explain, and most videos I watched glossed over it, but this one explains it pretty well.

https://www.youtube.com/watch?v=BFYFkn-eOQk

I understand lean syntax much better than I started, that was my favourite part of the assignment but unfortunately I spent too much time on something that was effectively not getting me either progress nor marks. Despite not actually getting to do anything meaningful, when reading up I found it interesting that lean lets you program tactics in lean and then use them- this is not possible in both Agda and Idris. Additionally, lean is better equipped for general purpose programming than Agda, and if I had time I would have implemented the entire GJ algorithm in lean which would have helped prove things about it.

# AI use

## Which tools were used and roughly how much were they used

The actual algorithms were both 100% written by me. For LU decomposition I used the video linked above as the base, and for GJ I winged it from my previous understanding. Everything written around these such as the `SolveWithSteps` framework, the contracts, the `ISolver` interface, the `MatrixElement`s, the organization/architecture was also written by me 100% without AI.

But I did use AI heavily in this assignment.

I used copilot to generate the WebApi, including the DTOs, Api Mapper, the Roman Numerals Converter for the steps and the Factory to instantiate solvers as a reflection substitute.

I used copilot to debug and fix an out of bounds error that was caused by bad input (an unsolveable configuration). Some of these bugs still remain.

I used copilot to generate the frontend, but had to iterate on it multiple times after my own attempts to program the UI fell through.

I used copilot to generate some of the definitions in `Defs.lean` based on a detailed spec I wrote for it.

## Examples where the AI was wrong/unhelpful

The first one was an architectural misunderstanding. As part of creating the WebApi it needed a translation layer to take in loosely typed JSON data and enter it into the rock solid contractual ecosystem, so it whipped up a couple methods one being a "copy" method. This was a bit ridiculous because I already had a dedicated copy method hand written in the utils class. When questioned (I carefully review all AI output of course) it said the reason for creating a whole dedicated second utils class with one method was because at the call site, the indexing type `T` was not declared having constraint `class` (which stands for it being a reference type). This means there is a contention- you can't have a non reference type `T` work with the existing `Clone`. So either make a new `Clone`, which in this case took 40 or so lines and a new class. Or change `T` to give it the required constraint. Which is a one word change. This was a case of copilot not understanding that it had the authority to change a publicly shipped API, instead making the "safe" choice which only added code, at the cost of looking bizarre since it was essentially duplicated.

An example of when the AI output was unhelpful was when it needed access to internal methods in one project (Core) from another (WebApi). Any ordinary developer would understand that it is totally illogical for an entire module to be private, and at least some kind of basic API must be exposed for it to be useful. Instead of making those members public, copilot chose to add a directive to make the internals of the second assembly accessible in the first- which is a bit hacky and incorrect.

```cs
[assembly: InternalsVisibleTo("SolverApi")]
```

Another example of when the AI was unhelpful was when I provided it a specification for some contracts, theorems and lemmas I would like, in line with a well rounded verification that would formally verify 10 or so invariants. The points to cover are in the README in the formalization folder. Copilot spent well over 2 hours (during which I was doing other things and not paying too much attention) generating an output and finally produced an extremely thorough series of headers, across 10 different files averaging about 300 lines per file. While this could have been caught if I were more specific with my prompt, or if I had been paying attention, it is also a mistake on the AI's part as the definitions file that was already present was barely 100 lines in size.

## What Was Truly Understood?

100% of C# or algorithm code, every single line was meticulously reviewed and committed. I am ready to defend any piece of code in this repo.

The lean stuff I am pretty confident about given I discarded everything I didn't understand. But since it isn't functionally tested like the code is I might have missed something.

The frontend stuff not so much, I just wanted it to work so I hit go and stopped iterating when it looked good.
