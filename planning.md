## Planning

Ok lets think about what the assignment spec is and what we actually have to do...

There are 4 deliverables:

### Working Implementation

This is a ton of code in a repo (this one). It has to be in a single repository, with a README explaining how to build and run it.
When I'm finished I'll push it to GitHub.

### Track Specific Artifact

Track A: the benchmarking harness, data generation, and results;
Track B: the working tool with its interface (I want to do this!)
Track C: the correctness commentary (this is also really cool, I am a big fan of lean but legitimately do not know if I have the time to do this. There is another problem with this, which is that I would want to develop everything in C or C++ or C#, but to interface with a language where I can do serious theorem proving, like Idris (through foreign function interface) or Lean, I would need some kind of dodgy wiring. I can do it natively in Coq but I don't know Coq.).

### A Written Report

It must be structured as follows:

#### What Did I Build

A short description of the algorithm or data structure that was implemented and the key design decisions I made.

This must include

1. language
2. data representation (how you chose to represent data, structure of data and objects)
3. any simplifications or extensions relative to the textbook version
4. anything a reader needs to know to navigate the code

#### Track Specific Writeup

This part is again relative to which track you choose:

Track A needs to have the findings of the empirical study, things like:

- hypothesis
- what was compared
- how the data was generated
- how the data was measured
- results with plots
- analysis at a low computational level of why the results came out the way they did

Track B is more minimal:

- Explain the tool, what it does
- How does the algorithm sit inside it
- How did I make the interface decisions for connecting the tool to user input (UI)
- Worked example of it in use

- track specific artifact
- a written report
  - what I built
  - track specific writeup
  - what I learnt
  - AI use
- video walkthrough of demonstration

## Choice

First we choose the actual algorithm, I like maths so I am going to choose the Gauss Jordan elimination. I already have a good idea of how this works so I don't need to spend time reading.

-
