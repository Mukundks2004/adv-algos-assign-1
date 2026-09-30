# Planning

Ok lets think about what the assignment spec is and what we actually have to do...

There are 4 deliverables:

## Working Implementation

This is a ton of code in a repo (this one). It has to be in a single repository, with a README explaining how to build and run it.
When I'm finished I'll push it to GitHub.

## Track Specific Artifact

Track A: the benchmarking harness, data generation, and results;
Track B: the working tool with its interface (I want to do this!)
Track C: the correctness commentary (this is also really cool, I am a big fan of lean but legitimately do not know if I have the time to do this. There is another problem with this, which is that I would want to develop everything in C or C++ or C#, but to interface with a language where I can do serious theorem proving, like Idris (through foreign function interface) or Lean, I would need some kind of dodgy wiring. I can do it natively in Coq but I don't know Coq.).

## A Written Report

It must be structured as follows:

### What Did I Build

A short description of the algorithm or data structure that was implemented and the key design decisions I made.

This must include

1. language
2. data representation (how you chose to represent data, structure of data and objects)
3. any simplifications or extensions relative to the textbook version
4. anything a reader needs to know to navigate the code

### Track Specific Writeup

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

Track C is awesome but a shame I won't be using it:

- The correctness commentary
- A summary of the commentary if it lives in the code (damn, this guy really wants us to use lean :P it is a shame, I'll do it for the next assignment maybe)

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

## Video Walkthrough

We also need to record a 3-5 minute screencast walking through the implementation with the code on screen and the voice explaining it. The video must cover the following:

- The core of the implementation, where the main algorithm lives in the code and how it is organized
- One tricky part: the piece of the code I found hardest to get right and why
- One invariant: state the invariant, show where it is established and show where it is relied on
- One "what breaks" example: pick a specific line or step, and explain what would go on if it were removed

Production quality of the video does not matter at all. A single take with no editing is what is expected, do not spend time on slides or editing Submit the video file or an unlisted link alongside the repo.

The purpose of the video is to make one thing visible- can I explain my own code?

Given the AI policy, every line of code being self written is not expected, but "I can walk you through what this does and why it is correct" is expected.

A student who used AI heavily and can explain the submission is in a fine position, but a student who cannot explain is not regardless of how it is produced.

If the video raises questions, or the gap between the polish of the submission and the walkthrough is hard to explain, Troy may invite you to a short live follow up where we look at the code with Troy. If the conversation reveals a serious gap between the submission and my understanding of it, Troy may adjust the relevant components of the mark accordingly.

## Choice

First we choose the actual algorithm, I like maths so I am going to choose the Gauss Jordan elimination. I already have a good idea of how this works so I don't need to spend time reading.

-
