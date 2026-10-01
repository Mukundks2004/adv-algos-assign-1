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

As I chose track B, the majority of this writeup will focus on the inclusion of the Gauss-Jordan algorithm to solve magic squares.

However, before that I want to briefly mention my progress with tracks A and C.

## Track A: Gauss Jordan vs LU Decomposition

For track A I implemented another more commonly used linear equation solver, the LU Decomposition method via Doolittle's Algorithm. This is a direct competitor to Gauss-Jordan (GJ). You can find the implementation in `\Implementation\GaussJordanElim\GaussJordanElim\Solvers\LuSolver.cs`. Unfortunately, both algorithms are highly similar to the point where an empirical study comparing them is not very interesting. For this reason, I did not investigate very deeply. One notable difference, is that LU only needs to calulate the lower and upper matrices once, and these can then be reused to do the lightning fast forward and back substitution with different 'constants vectors'. However, for GJ, the whole elimination process needs to be redone if the constants vector changes.

## Track C: Formal Verification with Lean4 and Mathlib

I have some experience with formal verification in both Idris and Agda, and I've played the lean natural numbers game so I thought I would try my hand at actual theorem proving. Unfortunately, it was way harder than I thought and I did not get very far. You can see my progress in `\Implementation\formalization\gauss-jordan-formalization`.

What I did get done:

- Planned out what definitions I need
- Planned out what kind of variants and properties I want to prove
- Organized this into files (you can see this in the `README.md`)
- Wrote out some of the definitions and AI generated the rest
- AI generated the implementation
- Went through all the definitions to confirm I understood them

At this stage I asked the AI to generate the headers for the other sections I had outlined, but the result was thousands of lines long (just the headers!) and impossible to read myself, so I decided to submit my own work and abandoned this.

## Track B: The Gauss Jordan Algorithm for Solving Magic Squares

The tool I have chosen to build is a magic square solver. You can build and run it by following the instructions in the README at the root of the repo.

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

The algorithm to use here is called Gauss-Jordan Elimination, if you represent a system of equations as a matrix, GJ will manipulate the matrix in a way that keeps it consistent mathematically, but simplifies it for humans, the end result is a computer friendly result that is easily readable by a computer as "x = 1, y = 2, ...".

The process to get from the UI to the algorithm is a bit lengthy. Essentially, the UI takes a magic square with some cells filled and some empty, then on "solve" it pads all empty cells with nulls and sends it to the backend via a http request. The backend instantiates the right solver via a factory.

This solver formats

Track B is more minimal:

- Explain the tool, what it does
- How does the algorithm sit inside it
- How did I make the interface decisions for connecting the tool to user input (UI)
- Worked example of it in use

### What I Learned

- What surprised you?
- What did you initially get wrong?
- What is something you understand now that you didn't before starting?

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
